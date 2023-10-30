using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomTexturesRedux;

[BepInPlugin("p1xel8ted.sunhaven.customtexturesredux", "Custom Textures Redux", "0.1.0")]
public class Plugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool> ModEnabled { get; private set; }
    private static ConfigEntry<bool> IsDebug { get; set; }
    internal static ConfigEntry<KeyboardShortcut> ReloadKey { get; private set; }
    private static Dictionary<Texture2D, Texture2D> TextureReplacements { get; } = new();
    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        ModEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");
        IsDebug = Config.Bind("General", "IsDebug", true, "Enable debug logs");
        ReloadKey = Config.Bind("General", "ReloadKey", new KeyboardShortcut(KeyCode.F5), "Key to press to reload textures from disk");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Info.Metadata.GUID);
        TextureUtils.LoadCustomTextures();
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public static void DebugLog(string str)
    {
        if (!IsDebug.Value) return;

        Log.LogInfo(str);
    }

    private static void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (!ModEnabled.Value) return;
        var textureCount = TextureUtils.CustomTextureDict.Count;
        var s = new Stopwatch();
        Log.LogInfo($"Replacing {textureCount} scene textures...");
        s.Start();

        // Optimized to check against CustomTextureDict before processing each texture.
        var allTextures = Resources.FindObjectsOfTypeAll<Texture2D>();
        foreach (var originalTexture in allTextures)
        {
            if (originalTexture == null || !TextureUtils.CustomTextureDict.ContainsKey(originalTexture.name))
                continue;

            var replacementTexture = TextureUtils.TryGetReplacementTexture(originalTexture);
            if (replacementTexture == null) continue;

            TextureReplacements[originalTexture] = replacementTexture;
            DebugLog($"Replaced texture '{originalTexture.name}' with '{replacementTexture.name}' from cache.");
        }

        var allRenderers = Resources.FindObjectsOfTypeAll<Renderer>();
        foreach (var renderer in allRenderers)
        {
            foreach (var material in renderer.materials)
            {
                foreach (var propertyName in material.GetTexturePropertyNames())
                {
                    if (material.GetTexture(propertyName) is not Texture2D currentTexture || !TextureReplacements.TryGetValue(currentTexture, out var newTexture)) continue;
                    material.SetTexture(propertyName, newTexture);
                    DebugLog($"Replaced texture '{currentTexture.name}' with '{newTexture.name}' in material '{material.name}' on renderer '{renderer.name}'");
                }
            }
        }

        var sprites = Resources.FindObjectsOfTypeAll<SpriteRenderer>();
        foreach (var spriteRenderer in sprites)
        {
            if (spriteRenderer.sprite == null || !TextureUtils.CustomTextureDict.ContainsKey(spriteRenderer.sprite.name))
                continue;

            var newSprite = TextureUtils.TryGetReplacementSprite(spriteRenderer.sprite);
            if (newSprite == null) continue;
            spriteRenderer.sprite = newSprite;
            DebugLog($"Replaced sprite '{spriteRenderer.sprite.name}' on sprite renderer '{spriteRenderer.name}'");
        }

        s.Stop();
        Log.LogInfo($"Time to replace {textureCount} textures: {s.ElapsedMilliseconds}ms");
    }
}