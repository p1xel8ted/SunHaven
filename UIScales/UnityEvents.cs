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
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if (SceneManager.GetActiveScene().name.Equals("MainMenu", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainMenuUiScale.Value += 0.25f;
                    if (MainMenuUiScale.Value < 0.5)
                    {
                        MainMenuUiScale.Value = 0.5f;
                    }

                    if (MainMenuCanvas != null)
                    {
                        MainMenuCanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                        MainMenuCanvas.scaleFactor = MainMenuUiScale.Value;
                    }
                }

                InGameUiScale.Value += 0.25f;
                if (InGameUiScale.Value < 0.5)
                {
                    InGameUiScale.Value = 0.5f;
                }

                if (UICanvas != null)
                {
                    UICanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    UICanvas.scaleFactor = InGameUiScale.Value;
                }

                if (SecondUICanvas != null)
                {
                    SecondUICanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    SecondUICanvas.scaleFactor = InGameUiScale.Value;
                }

                if (NotificationStack.Instance != null)
                {
                    NotificationStack.Instance.SendNotification("UI Scale: " + InGameUiScale.Value);
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (SceneManager.GetActiveScene().name.Equals("MainMenu", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainMenuUiScale.Value -= 0.25f;
                    if (MainMenuUiScale.Value < 0.5)
                    {
                        MainMenuUiScale.Value = 0.5f;
                    }

                    if (MainMenuCanvas != null)
                    {
                        MainMenuCanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                        MainMenuCanvas.scaleFactor = MainMenuUiScale.Value;
                    }
                }


                InGameUiScale.Value -= 0.25f;
                if (InGameUiScale.Value < 0.5)
                {
                    InGameUiScale.Value = 0.5f;
                }

                if (UICanvas != null)
                {
                    UICanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    UICanvas.scaleFactor = InGameUiScale.Value;
                }

                if (SecondUICanvas != null)
                {
                    SecondUICanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    SecondUICanvas.scaleFactor = InGameUiScale.Value;
                }

                if (NotificationStack.Instance != null)
                {
                    NotificationStack.Instance.SendNotification("UI Scale: " + InGameUiScale.Value);
                }
            }
        }

        if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            if (Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                ZoomNeedsUpdating = true;
                ZoomLevel.Value -= 0.25f;
                if (ZoomLevel.Value < 0.5)
                {
                    ZoomLevel.Value = 0.5f;
                }

                if (Player.Instance != null && ZoomNeedsUpdating)
                {
                    Player.Instance.OverrideCameraZoomLevel = false;
                    Player.Instance.SetZoom(ZoomLevel.Value, true);
                }


                if (NotificationStack.Instance != null)
                {
                    NotificationStack.Instance.SendNotification("Zoom Level: " + ZoomLevel.Value);
                }
            }

            if (Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                ZoomNeedsUpdating = true;
                ZoomLevel.Value += 0.25f;
                if (ZoomLevel.Value < 0.5)
                {
                    ZoomLevel.Value = 0.5f;
                }

                if (Player.Instance != null && ZoomNeedsUpdating)
                {
                    Player.Instance.OverrideCameraZoomLevel = false;
                    Player.Instance.SetZoom(ZoomLevel.Value, true);
                }

                if (NotificationStack.Instance != null)
                {
                    NotificationStack.Instance.SendNotification("Zoom Level: " + ZoomLevel.Value);
                }
            }
        }
    }
}