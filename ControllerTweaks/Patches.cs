// using HarmonyLib;
// using Wish;
//
// namespace ControllerTweaks;
//
// [HarmonyPatch]
// public static class Patches
// {
//     private static void Notify(string message, int id = 0, bool error = false)
//     {
//         SingletonBehaviour<NotificationStack>.Instance.SendNotification(message, id, 0, error);
//     }
//
//     [HarmonyPostfix]
//     [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.OpenInventory))]
//     public static void UIHandler_OpenInventory(ref UIHandler __instance)
//     {
//         __instance._inventoryUI.GetComponentInChildren<tabs>()
//     }
// }