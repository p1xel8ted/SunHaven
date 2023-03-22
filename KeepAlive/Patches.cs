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
        var index = 0;
        for (var i = 0; i < __result.Length; i++)
        {
            if (!__result[i].name.Contains("BepInEx"))
            {
                __result[index++] = __result[i];
            }
        }
        
        Array.Resize(ref __result, index);
    }
}