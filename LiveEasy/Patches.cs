using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wish;

namespace LiveEasy;

[HarmonyPatch]
public static class Patches
{
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