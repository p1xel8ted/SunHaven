using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace KeepAlive
{
    [HarmonyPatch]
    [HarmonyPriority(1)]
    public static class Patches
    {
        private const string BepInEx = "bepinex";

        [HarmonyPostfix]
        [HarmonyPriority(1)]
        [HarmonyPatch(typeof(Scene), nameof(Scene.GetRootGameObjects), new Type[] { })]
        public static void Scene_GetRootGameObjects(ref GameObject[] __result)
        {
            var objectList = new List<GameObject>(__result);
            var items = objectList.FindAll(a => a.name.ToLowerInvariant().Contains(BepInEx));

            if (items.Count > 0)
            {
                var savedItemNames = string.Join(", ", items.Select(i => i.name));
                Plugin.LOG.LogInfo($"Prevented the following items from being modified: {savedItemNames}");
            }

            __result = objectList.ToArray();
        }
    }
}