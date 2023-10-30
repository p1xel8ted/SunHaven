using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using Wish;

// ReSharper disable SuggestBaseTypeForParameter

namespace AutoTools;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    private const string NoSuitableToolFoundOnActionBar = $"No suitable tool found on action bar!";
    private const string NoPickaxeOnActionBar = "No pickaxe on action bar!";
    private const string NoAxeOnActionBar = "No axe on action bar!";
    private const string Prop = "prop";
    private const string Foliage = "foliage";
    private const string NoWateringCanOnActionBar = "No watering can on action bar!";
    private const string NoScytheOnActionBar = "No scythe on action bar!";
    private const string NoHoeOnActionBar = "No hoe on action bar!";
    private const string NoFishingRodOnActionBar = "No fishing rod on action bar!";
    internal const string YourWateringCanIsEmpty = "Your watering can is empty!";

    private static int WateringCanIndex { get; set; } = -1;
    private static Rock Rock { get; set; }
    private static Tree Tree { get; set; }
    private static Crop Crop { get; set; }
    private static Wood Wood { get; set; }
    private static Plant Plant { get; set; }

    private enum Tool
    {
        Pickaxe,
        Axe,
        Scythe,
        FishingRod,
        WateringCan,
        Hoe,
    }

    private static (int index, ToolData toolData) FindBestTool(Tool tool)
    {
        var toolDict = tool switch
        {
            Tool.Pickaxe => ToolDictionaries.PickAxes,
            Tool.Axe => ToolDictionaries.Axes,
            Tool.Scythe => ToolDictionaries.Scythes,
            Tool.FishingRod => ToolDictionaries.FishingRods,
            Tool.WateringCan => ToolDictionaries.WateringCans,
            Tool.Hoe => ToolDictionaries.Hoes,
            _ => throw new ArgumentOutOfRangeException(nameof(tool), tool, null)
        };

        foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
        {
            Plugin.LOG.LogWarning($"Checking if we have and can use a {toolEntry.Value}");
            foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
            {
                var toolData = ItemDatabase.GetItemData(item.ItemImage.item) as ToolData;
                if (toolData == null || toolData.id != toolEntry.Key || !CanUse(toolData)) continue;
                if (tool == Tool.WateringCan)
                {
                    WateringCanIndex = item.ItemImage.slotIndex;
                }

                Plugin.LOG.LogWarning($"Found a {toolEntry.Value} that we can use on the action bar!");
                return (item.ItemImage.slotIndex, toolData);
            }
        }

        Plugin.LOG.LogWarning($"No suitable tool found on action bar!");
        return (-1, null);
    }


    private static void HandleToolInteraction(int toolIndex, string errorMessage)
    {
        if (toolIndex != -1)
        {
            SetActionBar(toolIndex);
        }
        else
        {
            Notify(errorMessage, true);
        }
    }

    private static bool IsInFarmTile()
    {
        return TileManager.Instance.HasTileOrFarmingTile(Player.Instance.Position, ScenePortalManager.ActiveSceneIndex);
    }

    private static void RunToolActions(Collider2D collider)
    {
        ToolAction(Tool.Pickaxe, Rock is not null && Rock.Pickaxeable, Plugin.EnableAutoPickaxe.Value, NoPickaxeOnActionBar);
        ToolAction(Tool.Axe, (Tree is not null && Tree.Axeable) || (Wood is not null && Wood.Axeable), Plugin.EnableAutoAxe.Value, NoAxeOnActionBar);
        ToolAction(Tool.Scythe, Plant is not null || (collider.name.Contains(Foliage) && !collider.name.Contains(Prop)) || (Crop is not null && Crop.FullyGrown), Plugin.EnableAutoScythe.Value, NoScytheOnActionBar);
        ToolAction(Tool.FishingRod, Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < Plugin.FishingRodWaterDetectionDistance.Value && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y)), Plugin.EnableAutoFishingRod.Value, NoFishingRodOnActionBar);
        ToolAction(Tool.Hoe, TileManager.Instance.IsHoeable(new Vector2Int((int) Player.Instance.ExactGraphicsPosition.x, (int) Player.Instance.ExactGraphicsPosition.y)), Plugin.EnableAutoHoe.Value, NoHoeOnActionBar);
        
        FindBestTool(Tool.WateringCan);
        ToolAction(Tool.WateringCan, !WateringCanHasWater() && (Wish.WateringCan.OverWaterSource || Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < 10 && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y))), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar);
        ToolAction(Tool.WateringCan, Crop is not null && (!Crop.data.watered || Crop.data.onFire), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar, WateringCanHasWater, YourWateringCanIsEmpty);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInteractions), nameof(PlayerInteractions.OnTriggerEnter2D))]
    public static void PlayerInteractions_OnTriggerEnter2D(PlayerInteractions __instance, Collider2D collider)
    {
        if (!Plugin.EnableAutoTool.Value) return;

        if (!EnableToolSwaps(__instance, collider)) return;

        if (IsInFarmTile() && !Plugin.EnableAutoToolOnFarmTiles.Value) return;

        UpdateColliders(collider);

        RunToolActions(collider);
    }


    private static void UpdateColliders(Collider2D collider)
    {
        Rock = collider.GetComponent<Rock>();
        Tree = collider.GetComponent<Tree>();
        Crop = collider.GetComponent<Crop>();
        Wood = collider.GetComponent<Wood>();
        Plant = collider.GetComponent<Plant>();
    }

    private static bool EnableToolSwaps(PlayerInteractions __instance, Collider2D collider)
    {
        return __instance is not null || collider is not null || SceneSettingsManager.Instance is not null || TileManager.Instance is not null;
    }

    private static bool WateringCanHasWater()
    {
        if (WateringCanIndex == -1)
        {
            Plugin.LOG.LogWarning($"No watering can on action bar!");
            return false;
        }
        var item = Player.Instance.PlayerInventory._actionBarIcons[WateringCanIndex].ItemImage.item;
        if (item is not WateringCanItem wc)
        {
            return false;
        }

        return wc.WaterAmount > 0;
    }

    private static void ToolAction(Tool tool, bool condition, bool pluginValue, string errorMessage, Func<bool> additionalCondition = null, string failedConditionMessage = null)
    {
        if (!condition || !pluginValue) return;

        if (additionalCondition != null && !additionalCondition())
        {
            if (!string.IsNullOrEmpty(failedConditionMessage))
            {
                Notify(failedConditionMessage, true);
            }

            return;
        }


        var toolData = FindBestTool(tool);
        if (toolData.toolData != null)
        {
            if (CanUse(toolData.toolData))
            {
                HandleToolInteraction(toolData.index, errorMessage);
            }
            else
            {
                Notify(ToolLevelTooLow(toolData.toolData), true);
            }
        }
        else
        {
            Notify(NoSuitableToolFoundOnActionBar, true);
        }
    }

    private static string ToolLevelTooLow(ToolData toolData)
    {
        return $"Your {toolData.profession} level is too low to use {toolData.name}!";
    }

    private static bool CanUse(ToolData toolData)
    {
        return SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[toolData.profession].level >= toolData.requiredLevel;
    }

    private const float TimeBetweenNotifications = 5f;
    private static float LastNotificationTime { get; set; }
    private static string PreviousMessage { get; set; }

    internal static void Notify(string message, bool error = false)
    {
        if (message == PreviousMessage && Time.time - LastNotificationTime < TimeBetweenNotifications) return;
        LastNotificationTime = Time.time;
        PreviousMessage = message;
        SingletonBehaviour<NotificationStack>.Instance.SendNotification(message, error: error);
    }

    private static void SetActionBar(int index)
    {
        PlayerInput.AllowChangeActionBarItem = true;
        Player.Instance.PlayerInventory.SetActionBarSlot(index);
        Player.Instance.PlayerInventory.SetIndex(index);
    }
}