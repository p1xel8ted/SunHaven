using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wish;

namespace UIScales;

public partial class Plugin
{
    private void LateUpdate()
    {
        if (Player.Instance != null && ZoomModEnabled.Value)
        {
            Player.Instance.SetZoom(ZoomLevel.Value, true);
        }

        if (MainMenuController.Instance != null && UIModEnabled.Value)
        {
            var newGameValue = InGameUiScale.Value;
            var newMenuValue = MainMenuUiScale.Value;


            if (SceneManager.GetActiveScene().name.Contains("MainMenu"))
            {
                if (newMenuValue < 0) return;
                var canvas = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
                if (canvas == null) return;
                canvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                canvas.scaleFactor = newMenuValue;
            }
            else
            {
                var canvas = GameObject.Find("Manager/UI").GetComponent<CanvasScaler>();
                if (canvas != null)
                {
                    canvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    canvas.scaleFactor = newGameValue;
                }


                var actionbarCanvas = GameObject.Find("Player(Clone)/UI").GetComponent<CanvasScaler>();
                if (actionbarCanvas == null) return;
                actionbarCanvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                actionbarCanvas.scaleFactor = newGameValue;
            }
        }
    }
}