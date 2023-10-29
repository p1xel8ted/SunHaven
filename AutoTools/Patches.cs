using System;
using System.Collections.Generic;
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
    private const string Pickaxe = "pickaxe";
    private const string Axe = "_axe";
    private const string Scythe = "scythe";
    private const string FishingRod = "_Fishing_Rod";
    private const string WateringCan = "_watering_can";
    private const string Sword = "sword";
    private const string Hoe = "hoe";
    private const string NoSuitableToolFoundOnActionBar = $"No suitable tool found on action bar!";
    private const string NoSwordOnActionBar = "No sword on action bar!";
    private const string NoPickaxeOnActionBar = "No pickaxe on action bar!";
    private const string NoAxeOnActionBar = "No axe on action bar!";
    private const string Prop = "prop";
    private const string Foliage = "foliage";
    private const string NoWateringCanOnActionBar = "No watering can on action bar!";
    private const string NoScytheOnActionBar = "No scythe on action bar!";
    private const string NoHoeOnActionBar = "No hoe on action bar!";
    private const string NoFishingRodOnActionBar = "No fishing rod on action bar!";
    internal const string YourWateringCanIsEmpty = "Your watering can is empty!";
    
    private static readonly Dictionary<string, Action<int>> ToolIndexUpdater = new()
    {
        {Pickaxe, index => PickaxeIndex = index},
        {Axe, index => AxeIndex = index},
        {Scythe, index => ScytheIndex = index},
        {FishingRod, index => FishingRodIndex = index},
        {WateringCan, index => WateringCanIndex = index},
        {Sword, index => SwordIndex = index},
        {Hoe, index => HoeIndex = index}
    };

    private static int PickaxeIndex { get; set; } = -1;
    private static int AxeIndex { get; set; } = -1;
    private static int ScytheIndex { get; set; } = -1;
    private static int FishingRodIndex { get; set; } = -1;
    private static int WateringCanIndex { get; set; } = -1;
    private static int SwordIndex { get; set; } = -1;
    private static int HoeIndex { get; set; } = -1;
    private static Dictionary<int, ToolData> ToolIndexToItemMap { get; } = new();
    private static EnemyAI Enemy { get; set; }
    private static NPCAI Npc { get; set; }
    private static bool IsEnemy => Enemy is not null && Npc is null;
    private static Rock Rock { get; set; }
    private static Tree Tree { get; set; }
    private static Crop Crop { get; set; }
    private static Wood Wood { get; set; }
    private static Plant Plant { get; set; }

    private static ToolData GetToolDataByToolIndex(int toolIndex)
    {
        ToolIndexToItemMap.TryGetValue(toolIndex, out var value);
        return value;
    }


    internal static void UpdateToolIndexes()
    {
        if (Player.Instance is null || Player.Instance.PlayerInventory is null)
            return;

        foreach (var tool in ToolIndexUpdater.Keys)
        {
            ToolIndexUpdater[tool](-1);
        }

        ToolIndexToItemMap.Clear();
        
        foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
        {
            foreach (var kvp in ToolIndexUpdater.Where(kvp => item.ItemImage is not null && item.ItemImage._image.sprite.name.Contains(kvp.Key)))
            {
                kvp.Value(item.ItemImage.slotIndex);
                var toolData = ItemDatabase.GetItemData(item.ItemImage.item) as ToolData;
                ToolIndexToItemMap[item.ItemImage.slotIndex] = toolData;
                break;
            }
        }
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
        ToolAction(SwordIndex, IsEnemy, Plugin.EnableAutoSword.Value, NoSwordOnActionBar);
        ToolAction(PickaxeIndex, Rock is not null && Rock.Pickaxeable, Plugin.EnableAutoPickaxe.Value, NoPickaxeOnActionBar);
        ToolAction(AxeIndex, (Tree is not null && Tree.Axeable) || (Wood is not null && Wood.Axeable), Plugin.EnableAutoAxe.Value, NoAxeOnActionBar);
        ToolAction(ScytheIndex, Plant is not null || (collider.name.Contains(Foliage) && !collider.name.Contains(Prop)) || (Crop is not null && Crop.FullyGrown), Plugin.EnableAutoScythe.Value, NoScytheOnActionBar);
        ToolAction(FishingRodIndex, Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < 20 && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y)), Plugin.EnableAutoFishingRod.Value, NoFishingRodOnActionBar);
        ToolAction(HoeIndex, TileManager.Instance.IsHoeable(new Vector2Int((int) Player.Instance.ExactGraphicsPosition.x, (int) Player.Instance.ExactGraphicsPosition.y)), Plugin.EnableAutoHoe.Value, NoHoeOnActionBar);
        ToolAction(WateringCanIndex, !WateringCanHasWater() && (Wish.WateringCan.OverWaterSource || Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < 10 && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y))), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar);   
        ToolAction(WateringCanIndex, Crop is not null && (!Crop.data.watered || Crop.data.onFire), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar, WateringCanHasWater, YourWateringCanIsEmpty);
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInteractions), nameof(PlayerInteractions.OnTriggerEnter2D))]
    public static void PlayerInteractions_OnTriggerEnter2D(PlayerInteractions __instance, Collider2D collider)
    {
        if (!Plugin.EnableAutoTool.Value) return;

        if (!EnableToolSwaps(__instance, collider)) return;

        if (IsInFarmTile() && !Plugin.EnableAutoToolOnFarmTiles.Value) return;

        UpdateColliders(collider);

        UpdateToolIndexes();

        RunToolActions(collider);
    }


    private static void UpdateColliders(Collider2D collider)
    {
        Enemy = collider.GetComponent<EnemyAI>();
        Npc = collider.GetComponent<NPCAI>();
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
        if(WateringCanIndex == -1) return false;
        var item = Player.Instance.PlayerInventory._actionBarIcons[WateringCanIndex].ItemImage.item;
        if (item is not WateringCanItem wc)
        {
            return false;
        }
        return wc.WaterAmount > 0;
    }

    private static void ToolAction(int toolIndex, bool condition, bool pluginValue, string errorMessage, Func<bool> additionalCondition = null, string failedConditionMessage = null)
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


        var toolData = GetToolDataByToolIndex(toolIndex);
        if (toolData != null)
        {
            if (CanUse(toolData))
            {
                HandleToolInteraction(toolIndex, errorMessage);
            }
            else
            {
                Notify(ToolLevelTooLow(toolData), true);
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