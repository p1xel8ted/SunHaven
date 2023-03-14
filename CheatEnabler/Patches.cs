using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wish;

namespace CheatEnabler;

[HarmonyPatch]
public partial class Plugin
{
    //the game unloads objects when quitting to menu, and BepInEx gets caught up in it, which kills any mods that use BepInEx. This fixes that.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scene), nameof(Scene.GetRootGameObjects), new Type[] { })]
    public static void Scene_GetRootGameObjects(ref GameObject[] __result)
    {
        var newList = __result.ToList();
        newList.RemoveAll(x => x.name.Contains("BepInEx"));
        __result = newList.ToArray();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.Initialize))]
    public static void Player_Initialize(ref PlayerSettings __instance)
    {
        __instance.SetCheatsEnabled(true);
        _log.LogWarning("Cheat Menu Enabled!");
    }
}