using HarmonyLib;
using Wish;

namespace CheatEnabler;

[HarmonyPatch]
public partial class Plugin
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.Initialize))]
    public static void Player_Initialize(ref PlayerSettings __instance)
    {
        __instance.SetCheatsEnabled(true);
        _log.LogWarning("Cheat Menu Enabled!");
    }
}