using System;
using System.Collections;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using Wish;
using Object = UnityEngine.Object;

namespace EasyLiving;

[HarmonyPatch]
public static class Patches
{
    private static GameObject _newButton;


    [HarmonyPrefix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.PushDialogue), typeof(DialogueNode), typeof(UnityAction), typeof(bool), typeof(bool))]
    public static bool DialogueController_PushDialogue(ref DialogueNode dialogue, ref UnityAction onComplete)
    {
        if (!Plugin.SkipMuseumMissingItemsDialogue.Value) return true;

        if (dialogue.dialogueText.Any(str => str.Contains("museum bundle") && str.Contains("missing items")))
        {
            onComplete?.Invoke();
            return false;
        }

        return true;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Crop), nameof(Crop.ReceiveDamage))]
    public static bool Crop_ReceiveDamage(ref Crop __instance, ref DamageInfo damageInfo, ref DamageHit __result)
    {
        if (!Plugin.AllowRemovalOfGrowingCrops.Value) return true;

        if (__instance.FullyGrown || __instance.data.onFire || __instance.data.entangled || __instance.data.frozen || damageInfo.hitType != HitType.Scythe || __instance._seedItem.pickUpAble || __instance.data.dead)
        {
            return true;
        }

        if (!__instance.FullyGrown)
        {
            __instance.DestroyCrop();
            if (UnityEngine.Random.value < 0.50f)
            {
                __instance.DropSeeds();
            }

            __result = new DamageHit
            {
                hit = true,
                damageTaken = 1f
            };
            return false;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.OnEnable))]
    public static void PlayerSettings_OnEnable(ref PlayerSettings __instance)
    {
        if (!Plugin.AddQuitToDesktopButton.Value)
        {
            if (_newButton != null)
            {
                Object.Destroy(_newButton);
            }

            return;
        }

        var exitButton = GameObject.Find("Player(Clone)/UI/Inventory/ExitButton");
        if (exitButton == null || _newButton != null) return;

        _newButton = Object.Instantiate(exitButton, exitButton.transform.parent);
        _newButton.name = "ExitToDesktopButton";
        var pop = _newButton.AddComponent<Popup>();
        pop.name = "ExitToDesktopPop";
        pop.description = "Save progress and exit directly to the desktop.";
        pop.text = "Exit to Desktop";


        _newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            NotificationStack.Instance.SendNotification($"Game Saved! Exiting...");
            GC.Collect();
            Application.Quit();
        });

        var nav = exitButton.GetComponent<NavigationElement>();
        nav.down = _newButton.GetComponent<NavigationElement>();
        _newButton.GetComponent<NavigationElement>().up = nav;

        var rectTransform = _newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(265.5f, -147.5f);
        rectTransform.anchoredPosition3D = new Vector3(265.5f, -147.5f, 1);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    public static void MainMenuController_EnableMenu(ref MainMenuController __instance)
    {
        string[] objectNames =
        {
            "Canvas/[HomeMenu]/SmallPixelSproutButton",
            "Canvas/[HomeMenu]/TwitterButton",
            "Canvas/[HomeMenu]/DiscordButton",
            "Canvas/[HomeMenu]/PixelSproutButton",
            "Canvas/[HomeMenu]/Image",
            "Canvas/[HomeMenu]/Image (1)",
            "Canvas/[HomeMenu]/Buttons/PlayButton (2)"
        };

        foreach (var name in objectNames)
        {
            var obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.SetActive(!Plugin.RemoveUnneededButtonsInMainMenu.Value);
            }
        }
    }
}