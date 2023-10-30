using HarmonyLib;
using Wish;

namespace CustomTexturesRedux;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.Update))]
    private static void Player_Update()
    {
        if (!Plugin.ModEnabled.Value) return;

        if (!Plugin.ReloadKey.Value.IsDown()) return;
        
        TextureUtils.CachedTextureDict.Clear();
        TextureUtils.LoadCustomTextures();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AnimationHandler), nameof(AnimationHandler.Awake))]
    private static void AnimationHandler_Awake(ref AnimationHandler __instance)
    {
        if (!Plugin.ModEnabled.Value)
            return;
        
        foreach (var key in __instance._animationClips.Keys)
        {
            var clipFrames = __instance._animationClips[key].Frames;
            for (var i = 0; i < clipFrames.Count; i++)
            {
                clipFrames[i] = TextureUtils.TryGetReplacementSprite(clipFrames[i]);
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MeshGenerator), nameof(MeshGenerator.GenerateMesh))]
    private static void MeshGenerator_GenerateMesh(ref MeshGenerator __instance)
    {
        if (!Plugin.ModEnabled.Value || __instance.sprite == __instance._prevSprite)
            return;

        __instance.sprite = TextureUtils.TryGetReplacementSprite(__instance.sprite);
    }
}