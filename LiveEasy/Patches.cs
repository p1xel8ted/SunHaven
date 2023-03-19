using System;
using HarmonyLib;
using UnityEngine;
using Wish;
using Object = UnityEngine.Object;

namespace LiveEasy;

[HarmonyPatch]
public static class Patches
{
    private static GameObject _newButton;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.OnEnable))]
    public static void PlayerSettings_OnEnable(ref PlayerSettings __instance)
    {
        var exitButton = GameObject.Find("Player(Clone)/UI/Inventory/ExitButton");
        if (exitButton != null && _newButton == null)
        {
            _newButton = Object.Instantiate(exitButton, exitButton.transform.parent);
            _newButton.name = "ExitToDesktopButton";
            var pop = _newButton.AddComponent<Popup>();
            pop.name = "ExitToDesktopPop";
           
                pop.description = "Save progress and exit directly to the desktop.";   
            
            pop.description = "Exit directly to the desktop.";
            pop.text = "Exit to Desktop";
            _newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {

                NotificationStack.Instance.SendNotification("Game Saved! Exiting...");
                GC.Collect();
                Application.Quit();
                
            });
            exitButton.GetComponent<NavigationElement>().down = _newButton.GetComponent<NavigationElement>();
            _newButton.GetComponent<NavigationElement>().up = exitButton.GetComponent<NavigationElement>();

            var rectTransform = _newButton.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(265.5f, -147.5f);
            rectTransform.anchoredPosition3D = new Vector3(265.5f, -147.5f, 1);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    public static void MainMenuController_EnableMenu(ref MainMenuController __instance)
    {
        var a = GameObject.Find("Canvas/[HomeMenu]/SmallPixelSproutButton");
        if (a != null)
        {
            a.SetActive(false);
        }

        var b = GameObject.Find("Canvas/[HomeMenu]/TwitterButton");
        if (b != null)
        {
            b.SetActive(false);
        }

        var c = GameObject.Find("Canvas/[HomeMenu]/DiscordButton");
        if (c != null)
        {
            c.SetActive(false);
        }

        var d = GameObject.Find("Canvas/[HomeMenu]/PixelSproutButton");
        if (d != null)
        {
            d.SetActive(false);
        }

        var e = GameObject.Find("Canvas/[HomeMenu]/Image");
        if (e != null)
        {
            e.SetActive(false);
        }

        var f = GameObject.Find("Canvas/[HomeMenu]/Image (1)");
        if (f != null)
        {
            f.SetActive(false);
        }

        var g = GameObject.Find("Canvas/[HomeMenu]/Buttons/PlayButton (2)");
        if (g != null)
        {
            g.SetActive(false);
        }
    }
}