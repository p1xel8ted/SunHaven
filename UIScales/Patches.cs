using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Wish;

namespace UIScales;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadCharacterMenu), nameof(LoadCharacterMenu.SetupSavePanels))]
    public static void LoadCharacterMenu_SetupSavePanels(ref LoadCharacterMenu __instance)
    {
        var backupButton = GameObject.Find("Canvas/[LoadCharacterMenu]/SwitchPanelButton").transform;
        var loadMenu = GameObject.Find("Canvas/[LoadCharacterMenu]/CurrentSaves").transform;
        if (backupButton == null || loadMenu == null)
        {
            return;
        }

        backupButton.SetParent(loadMenu);
        var rectTransform = backupButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(150f, -155f);
        rectTransform.anchoredPosition3D = new Vector3(150f, -155f, 1);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
    {
        if (__instance.name.Equals("UI", StringComparison.InvariantCultureIgnoreCase))
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning($"UI CanvasScaler.OnEnable: Name:{__instance.name}");
            }

            Plugin.UICanvas = __instance;
            Plugin.UICanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            Plugin.UICanvas.SetScaleFactor(Plugin.InGameUiScale.Value);
            Plugin.UICanvas.scaleFactor = Plugin.InGameUiScale.Value;
        }

        if (__instance.name.Equals("Quantum Console", StringComparison.InvariantCultureIgnoreCase))
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning($"CheatConsole CanvasScaler.OnEnable: Name:{__instance.name}");
            }

            Plugin.QuantumCanvas = __instance;
            Plugin.QuantumCanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            Plugin.QuantumCanvas.SetScaleFactor(Plugin.CheatConsoleScale.Value);
            Plugin.QuantumCanvas.scaleFactor = Plugin.CheatConsoleScale.Value;
        }

        if (__instance.name.Equals("Canvas", StringComparison.InvariantCultureIgnoreCase))
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning($"MainMenu CanvasScaler.OnEnable: Name:{__instance.name}");
            }

            Plugin.MainMenuCanvas = __instance;
            Plugin.MainMenuCanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            Plugin.MainMenuCanvas.SetScaleFactor(Plugin.MainMenuUiScale.Value);
            Plugin.MainMenuCanvas.scaleFactor = Plugin.MainMenuUiScale.Value;
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.SetZoom))]
    public static void Player_SetZoom()
    {
        Plugin.ZoomNeedsUpdating = false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsOwner))]
    public static void Player_Awake(ref Player __instance)
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning($"SecondUICanvas Player.InitializeAsOwner: Name:{__instance.name}");
        }

        Plugin.SecondUICanvas = GameObject.Find("Player(Clone)/UI").GetComponent<CanvasScaler>();
        Plugin.SecondUICanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        Plugin.SecondUICanvas.SetScaleFactor(Plugin.InGameUiScale.Value);
        Plugin.SecondUICanvas.scaleFactor = Plugin.InGameUiScale.Value;
    }
}