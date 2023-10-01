using HarmonyLib;
using UnityEngine;
using Wish;

namespace AutoTool;

[Harmony]
public static class Patches
{

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.SetUseItem))]
    public static void Player_SetUseItem(ref Player __instance, ref ushort item, ref int index, ref bool fromLocal)
    {
        Plugin.LOG.LogWarning("Player_SetUseItem, item: " + item + ", index: " + index + ", local: " + fromLocal);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInteractions), nameof(PlayerInteractions.OnTriggerEnter2D))]
    public static void PlayerInteractions_OnTriggerEnter2D(ref PlayerInteractions __instance, ref Collider2D collider)
    {
        if (collider.name.ToLowerInvariant().Contains("tree"))
        {
            __instance.FirstInteractable.Target();
            Plugin.LOG.LogWarning("Trying to interact with a tree!");  
        } 
        
        if(collider.name.ToLowerInvariant().Contains("rock") ||
            collider.name.ToLowerInvariant().Contains("stone"))
        {
            __instance.FirstInteractable.Target();
            Plugin.LOG.LogWarning("Trying to interact with a rock!");
        }
    }
}