﻿using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (Plugin.UIOneCanvas != null && Plugin.UITwoCanvas != null && Plugin.QuantumCanvas != null && Plugin.MainMenuCanvas != null)
        {
            return;
        }

        var name = __instance.name;
        var path = Plugin.GetGameObjectPath(__instance.gameObject);

        switch (name)
        {
            case "UI" when Plugin.UIOneCanvas == null && path.Equals("Manager/UI"):
                Plugin.LOG.LogError($"Found top left and right UI!");
                Plugin.UIOneCanvas = __instance;
                ConfigureCanvasScaler(Plugin.UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.InGameUiScale.Value);
                break;
            case "UI" when Plugin.UITwoCanvas == null && path.Equals("Player(Clone)/UI"):
            {
                Plugin.LOG.LogError($"Found action bars/quest log etc!");
                Plugin.UITwoCanvas = __instance;
                ConfigureCanvasScaler(Plugin.UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.InGameUiScale.Value);
                break;
            }
            case "Quantum Console" when path.Equals("SharedManager/Quantum Console"):
                Plugin.LOG.LogError($"Found cheat console!");
                Plugin.QuantumCanvas = __instance;
                ConfigureCanvasScaler(Plugin.QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.CheatConsoleScale.Value);
                break;
            case "Canvas" when path.Equals("Canvas") && SceneManager.GetActiveScene().name == "MainMenu":
                Plugin.LOG.LogError($"Found menu??");
                Plugin.MainMenuCanvas = __instance;
                ConfigureCanvasScaler(Plugin.MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.MainMenuUiScale.Value);
                break;
        }
    }

    private static void ConfigureCanvasScaler(CanvasScaler canvasScaler, CanvasScaler.ScaleMode scaleMode, float scaleFactor)
    {
        if (canvasScaler == null)
        {
            return;
        }

        canvasScaler.uiScaleMode = scaleMode;
        canvasScaler.scaleFactor = scaleFactor;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Player), nameof(Player.SetZoom))]
    public static void Player_SetZoom_Prefix(ref float zoomLevel, ref bool immediate)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning($"PlayerSetZoom running: overriding requested zoom level.");
        }

        zoomLevel = Plugin.ZoomLevel.Value;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.ResetPlayerCamera), new Type[] { })]
    public static void Player_ResetPlayerCamera()
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);
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
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SettingsUI), nameof(SettingsUI.Start))]
    public static void SettingsUI_Start()
    {
        var zoomSlider = GameObject.Find("Player(Clone)/UI/Inventory/Settings/SettingsScroll View_Video/Viewport/Content/Setting_ZoomLevel");
        if (zoomSlider != null && zoomSlider.activeSelf)
        {
            zoomSlider.gameObject.SetActive(false);
        }
    }
}