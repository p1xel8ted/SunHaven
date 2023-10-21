using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wish;

namespace UIScales;

public partial class Plugin
{
    private void Update()
    {
        var isMainMenu = SceneManager.GetActiveScene().name.Equals("MainMenu", StringComparison.InvariantCultureIgnoreCase);
        UpdateUiScale(isMainMenu);
        UpdateZoomLevel();
        UpdateCanvasScaleFactors();
    }

    internal static void UpdateUiScale(bool isMainMenu)
    {
        float scaleAdjustment = 0;

        if (UIKeyboardShortcutIncrease.Value.IsUp())
        {
            scaleAdjustment = 0.25f;
        }
        else if (UIKeyboardShortcutDecrease.Value.IsUp())
        {
            scaleAdjustment = -0.25f;
        }

        if (scaleAdjustment != 0)
        {
            if (isMainMenu)
            {
                MainMenuUiScale.Value += scaleAdjustment;
                MainMenuUiScale.Value = Mathf.Max(Mathf.Round(MainMenuUiScale.Value / 0.25f) * 0.25f, 0.5f);
            }

            InGameUiScale.Value += scaleAdjustment;
            InGameUiScale.Value = Mathf.Max(Mathf.Round(InGameUiScale.Value / 0.25f) * 0.25f, 0.5f);

            if (_enableNotifications.Value && NotificationStack.Instance is not null)
            {
                SingletonBehaviour<NotificationStack>.Instance.SendNotification("UI Scale: " + InGameUiScale.Value);
            }
        }
    }

    internal static void UpdateZoomLevel()
    {
        if (ZoomKeyboardShortcutIncrease.Value.IsUp() || ZoomKeyboardShortcutDecrease.Value.IsUp())
        {
            // ZoomNeedsUpdating = true;
            var zoomAdjustment = ZoomKeyboardShortcutIncrease.Value.IsUp() ? 0.25f : -0.25f;
            ZoomLevel.Value += zoomAdjustment;
            ZoomLevel.Value = Mathf.Max(Mathf.Round(ZoomLevel.Value / 0.25f) * 0.25f, 0.5f);

            if (Player.Instance is not null)
            {
                Player.Instance.OverrideCameraZoomLevel = false;
                Player.Instance.SetZoom(ZoomLevel.Value, true);
            }

            if (_enableNotifications.Value && NotificationStack.Instance is not null)
            {
                SingletonBehaviour<NotificationStack>.Instance.SendNotification("Zoom Level: " + ZoomLevel.Value);
            }
        }
    }

    internal static void UpdateCanvasScaleFactors()
    {
        ConfigureCanvasScaler(MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, MainMenuUiScale.Value);
        ConfigureCanvasScaler(UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
        ConfigureCanvasScaler(UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
        ConfigureCanvasScaler(QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, CheatConsoleScale.Value);
    }
}