using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wish;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace EasyLiving;

[HarmonyPatch]
public static class Patches
{
    private const float BaseMoveSpeed = 4.5f;
    private const string SkippingLoadOfLastModifiedSave = "Skipping load of last modified save.";
    private static GameObject _newButton;
    private static bool PlayerReturnedToMenu { get; set; }
    private static readonly WriteOnce<Vector2> OriginalSize = new();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ExitGame))]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ExitGameImmediate))]
    public static void UIHandler_ExitGame()
    {
        PlayerReturnedToMenu = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.PlayGame), new Type[] { })]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.PlayGame), typeof(int))]
    public static void MainMenuController_PlayGame()
    {
        PlayerReturnedToMenu = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Player), nameof(Player.FixedUpdate))]
    public static void Player_FixedUpdate(ref Player __instance)
    {
        if (!Plugin.ApplyMoveSpeedMultiplier.Value) return;
        __instance.moveSpeed = BaseMoveSpeed * Plugin.MoveSpeedMultiplier.Value;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.MoveSpeed), MethodType.Getter)]
    public static void Player_MoveSpeed(ref Player __instance, ref float __result)
    {
        if (!Plugin.ApplyMoveSpeedMultiplier.Value) return;
       __result = BaseMoveSpeed * Plugin.MoveSpeedMultiplier.Value;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftingTable), nameof(CraftingTable.Interact))]
    public static void CraftingPanel_Interact(ref CraftingTable __instance)
    {
        if (!Plugin.MaterialsOnlyDefault.Value) return;
        __instance.craftingUI.sortHasMats.isOn = true;
        __instance.hasMats = true;
    }

    internal static bool SkipAutoLoad { get; set; }= false;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
    public static void GameSave_LoadAllCharacters(ref MainMenuController __instance)
    {
        if (!Plugin.AutoLoadMostRecentSave.Value) return;
        if (PlayerReturnedToMenu) return;
        if (SkipAutoLoad) 
        {
            Plugin.LOG.LogWarning(SkippingLoadOfLastModifiedSave);
            return;
        }
        var saves = SingletonBehaviour<GameSave>.Instance.Saves.OrderByDescending(save => save.worldData.saveTime).ToList();
        var lastModifiedSave = saves.FirstOrDefault();
        if (lastModifiedSave != null)
        {
            __instance.PlayGame(lastModifiedSave.characterData.characterIndex);
        }
    }

    private static string GetGameObjectPath(GameObject obj)
    {
        var path = obj.name;
        var parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.OnEnable))]
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.LateUpdate))]
    public static void ScrollRect_Initialize(ref ScrollRect __instance)
    {
        if (GetGameObjectPath(__instance.gameObject) != "Player(Clone)/UI/QuestTracker/Scroll View") return;

        if (!Plugin.EnableAdjustQuestTrackerHeightView.Value)
        {
            if (OriginalSize.Value != Vector2.zero)
            {
                __instance.viewport.GetComponent<RectTransform>().sizeDelta = OriginalSize.Value;
            }

            return;
        }

        if (OriginalSize.Value == Vector2.zero)
        {
            OriginalSize.Value = __instance.viewport.GetComponent<RectTransform>().sizeDelta;
        }

        var viewport = __instance.viewport.GetComponent<RectTransform>();
        var viewportHeight = Mathf.RoundToInt(Plugin.AdjustQuestTrackerHeightView.Value);
        viewport.sizeDelta = new Vector2(viewport.sizeDelta.x, viewportHeight);
    }

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


        _newButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (UIHandler.unloadingGame)
            {
                return;
            }

            Time.timeScale = 1f;
            NotificationStack.Instance.SendNotification($"Game Saved! Exiting...");
            SingletonBehaviour<GameSave>.Instance.SaveGame();
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