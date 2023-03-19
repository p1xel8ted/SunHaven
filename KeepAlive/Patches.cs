using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeepAlive;

[HarmonyPatch]
[HarmonyPriority(1)]
public static class Patches
{
    //the game manually unloads all loaded objects when quitting to menu, and BepInEx gets caught up in it, which kills any mods that use BepInEx. This fixes that.
    //hiding the bepinex object doesnt appear to make a difference
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(Scene), nameof(Scene.GetRootGameObjects), new Type[] { })]
    public static void Scene_GetRootGameObjects(ref GameObject[] __result)
    {
        var newList = __result.ToList();
        newList.RemoveAll(x => x.name.Contains("BepInEx"));
        __result = newList.ToArray();
    }
}