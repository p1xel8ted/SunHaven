using System;
using HarmonyLib;
using Wish;

namespace KeepAlive;

[Harmony]
public static class Actions
{
    public static Action OnGameExit { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ExitGame))]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ExitGameImmediate))]
    public static void UIHandler_ExitGame()
    {
        if (OnGameExit?.GetInvocationList().Length > 0)
        {
            var delegates = OnGameExit.GetInvocationList();
            Plugin.LOG.LogInfo($"Player exiting game. Invoking OnGameExit Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.LOG.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            OnGameExit.Invoke();
        }
        else
        {
            Plugin.LOG.LogInfo("Payer exiting game. No mods attached to OnGameExit Action.");
        }
    }
}