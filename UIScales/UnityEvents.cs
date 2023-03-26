using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void UpdateUiScale(bool isMainMenu)
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

            if (_enableNotifications.Value && NotificationStack.Instance != null)
            {
                NotificationStack.Instance.SendNotification("UI Scale: " + InGameUiScale.Value);
            }
        }
    }

    private void UpdateZoomLevel()
    {
        if (ZoomKeyboardShortcutIncrease.Value.IsUp() || ZoomKeyboardShortcutDecrease.Value.IsUp())
        {
            // ZoomNeedsUpdating = true;
            var zoomAdjustment = ZoomKeyboardShortcutIncrease.Value.IsUp() ? 0.25f : -0.25f;
            ZoomLevel.Value += zoomAdjustment;
            ZoomLevel.Value = Mathf.Max(Mathf.Round(ZoomLevel.Value / 0.25f) * 0.25f, 0.5f);

            if (Player.Instance != null)
            {
                Player.Instance.OverrideCameraZoomLevel = false;
                Player.Instance.SetZoom(ZoomLevel.Value, true);
            }

            if (_enableNotifications.Value && NotificationStack.Instance != null)
            {
                NotificationStack.Instance.SendNotification("Zoom Level: " + ZoomLevel.Value);
            }
        }
    }

    private static void UpdateCanvasScaleFactors()
    {
        if (MainMenuCanvas != null)
        {
            MainMenuCanvas.scaleFactor = MainMenuUiScale.Value;
        }

        if (UIOneCanvas != null)
        {
            UIOneCanvas.scaleFactor = InGameUiScale.Value;
        }

        if (UITwoCanvas != null)
        {
            UITwoCanvas.scaleFactor = InGameUiScale.Value;
        }

        if (QuantumCanvas != null)
        {
            QuantumCanvas.scaleFactor = CheatConsoleScale.Value;
        }
    }
}