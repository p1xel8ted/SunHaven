using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Wish;
using System.Collections.Concurrent;

namespace CustomTexturesRedux;

internal static class TextureUtils
{
    internal static ConcurrentDictionary<string, string> CustomTextureDict { get; } = new(StringComparer.OrdinalIgnoreCase);
    internal static ConcurrentDictionary<string, Texture2D> CachedTextureDict { get; } = new(StringComparer.OrdinalIgnoreCase);
    private static ConcurrentDictionary<string, Sprite> CachedSprites { get; } = new(StringComparer.OrdinalIgnoreCase);

    internal static void LoadCustomTextures()
    {
        CustomTextureDict.Clear();
        var path = Path.Combine(BepInEx.Paths.PluginPath, "CustomTextures");
        Plugin.Log.LogInfo($"Loading custom textures from {path}");
        try
        {
            Parallel.ForEach(Directory.GetFiles(path, "*.png", SearchOption.AllDirectories), file =>
            {
                var key = Path.GetFileNameWithoutExtension(file);
                if (!CustomTextureDict.TryAdd(key, file))
                {
                    Plugin.DebugLog($"Duplicate texture key found: {key}, file: {file}");
                }
            });

            Plugin.Log.LogInfo($"Loaded {CustomTextureDict.Count} textures");

            if (!DialogueController.Instance) return;

            DialogueController.Instance.enabled = false;
            DialogueController.Instance.enabled = true;
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Error loading custom textures: {e.Message}");
        }
    }


    private static Texture2D GetTexture(string path)
    {
        return CachedTextureDict.GetOrAdd(path, p =>
        {
            try
            {
                var imageBytes = File.ReadAllBytes(p);
                var tex = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
                tex.LoadImage(imageBytes);

                tex.filterMode = FilterMode.Point;
                tex.wrapMode = TextureWrapMode.Clamp;
                tex.wrapModeU = TextureWrapMode.Clamp;
                tex.wrapModeV = TextureWrapMode.Clamp;
                tex.wrapModeW = TextureWrapMode.Clamp;

                return tex;
            }
            catch (IOException e)
            {
                Plugin.Log.LogError($"Error loading texture from path {p}: {e.Message}");
                return null;
            }
        });
    }


    internal static ClothingLayerSprites HandleResult(ClothingLayerSprites result)
    {
        if (result == null || result._clothingLayerInfo == null)
        {
            return result;
        }

        Parallel.ForEach(result._clothingLayerInfo, layerInfo =>
        {
            if (layerInfo.sprites == null)
                return;

            for (var j = 0; j < layerInfo.sprites.Length; j++)
            {
                var sprite = layerInfo.sprites[j];
                if (sprite == null || sprite.texture == null)
                    continue;

                layerInfo.sprites[j] = TryGetReplacementSprite(sprite);
            }
        });

        return result;
    }

    internal static Sprite TryGetReplacementSprite(Sprite oldSprite)
    {
        if (oldSprite == null || oldSprite.texture == null)
            return null;

        var textureName = oldSprite.texture.name;
        if (string.IsNullOrEmpty(textureName))
            return oldSprite;

        var spriteKey = oldSprite.name + "_" + textureName;

        if (CachedSprites.TryGetValue(spriteKey, out var newSprite))
            return newSprite;

        if (!CustomTextureDict.TryGetValue(textureName, out var path))
            return oldSprite;

        var newTex = GetTexture(path);
        newTex.name = textureName;

        newSprite = Sprite.Create(newTex, oldSprite.rect, new Vector2(oldSprite.pivot.x / oldSprite.rect.width, oldSprite.pivot.y / oldSprite.rect.height), oldSprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, oldSprite.border, true);
        newSprite.name = oldSprite.name;

        CachedSprites[spriteKey] = newSprite;

        return newSprite;
    }


    internal static Texture2D TryGetReplacementTexture(Texture2D oldTexture)
    {
        if (oldTexture == null)
            return null;

        var textureName = oldTexture.name;
        if (string.IsNullOrEmpty(textureName) || !CustomTextureDict.TryGetValue(textureName, out var path))
            return oldTexture;

        var newTex = GetTexture(path);
        // Unity's way of null checking
        if (!newTex) return oldTexture;
        newTex.name = textureName;
        return newTex;
    }
}