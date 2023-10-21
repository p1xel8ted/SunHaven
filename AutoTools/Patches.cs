using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using Wish;

namespace AutoTools;

[Harmony]
public static class Patches
{
    private const string Pickaxe = "_pickaxe";
    private const string Axe = "_axe";
    private const string Scythe = "_scythe";
    private const string FishingRod = "_Fishing_Rod";
    private const string WateringCan = "_watering_can";
    private const string Sword = "_sword";
    private const string Hoe = "_hoe";
    private static int PickaxeIndex { get; set; } = -1;
    private static int AxeIndex { get; set; } = -1;
    private static int ScytheIndex { get; set; } = -1;
    private static int FishingRodIndex { get; set; } = -1;
    private static int WateringCanIndex { get; set; } = -1;
    private static int SwordIndex { get; set; } = -1;
    private static int HoeIndex { get; set; } = -1;


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

    internal static void UpdateToolIndexes()
    {
        if (Player.Instance is null) return;
        if (Player.Instance.PlayerInventory is null) return;

        foreach (var tool in ToolIndexUpdater.Keys)
        {
            ToolIndexUpdater[tool](-1); // Reset all indices
        }

        foreach (var item in Player.Instance.PlayerInventory._actionBarIcons)
        {
            foreach (var kvp in ToolIndexUpdater.Where(kvp => item.ItemImage is not null && item.ItemImage._image.sprite.name.Contains(kvp.Key)))
            {
                kvp.Value(item.ItemImage.slotIndex);
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInteractions), nameof(PlayerInteractions.OnTriggerEnter2D))]
    public static void PlayerInteractions_OnTriggerEnter2D(ref PlayerInteractions __instance, ref Collider2D collider)
    {
        if (!Plugin.EnableAutoTool.Value) return;

        if (__instance is null || collider is null) return;

      //  Plugin.LOG.LogWarning($"PlayerInteractions_OnTriggerEnter2D: {collider.name}");

        var enemy = collider.GetComponent<EnemyAI>();
        var npc = collider.GetComponent<NPCAI>();
        var isEnemy = enemy is not null && npc is null;
        var rock = collider.GetComponent<Rock>();
        var tree = collider.GetComponent<Tree>();
        var crop = collider.GetComponent<Crop>();
        var wood = collider.GetComponent<Wood>();
        var plant = collider.GetComponent<Plant>();

        // Only call UpdateToolIndexes() if one of the components is found.
        if (enemy is not null || rock is not null || tree is not null || crop is not null || wood is not null || plant is not null)
        {
            UpdateToolIndexes();
        }

        if (isEnemy)
        {
            HandleToolInteraction(SwordIndex, "No sword on action bar!");
        }
        else if (rock && rock.Pickaxeable)
        {
            HandleToolInteraction(PickaxeIndex, "No pickaxe on action bar!");
        }
        else if ((tree && tree.Axeable) || (wood && wood.Axeable))
        {
            HandleToolInteraction(AxeIndex, "No axe on action bar!");
        }
        else if (plant is not null || (collider.name.Contains("foliage") && !collider.name.Contains("prop")))
        {
            HandleToolInteraction(ScytheIndex, "No scythe on action bar!");
        }
        else if (crop is not null)
        {
            // Specific logic for crops, as it has additional conditions
            if (!crop.data.watered || crop.data.onFire)
            {
                HandleToolInteraction(WateringCanIndex, "No watering can on action bar!");
            }
            else if (crop.FullyGrown)
            {
                HandleToolInteraction(ScytheIndex, "No scythe on action bar!");
            }
        }
        else if (Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < 20 &&
                 SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y)))
        {
            HandleToolInteraction(FishingRodIndex, "No fishing rod on action bar!");
        }
    }

    // private static bool CanUse(ToolData toolData)
    // {
    //     return SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[toolData.profession].level >= toolData.requiredLevel;
    // }

    private static void Notify(string message, bool error = false)
    {
        SingletonBehaviour<NotificationStack>.Instance.SendNotification(message, error: error);
    }

    private static void SetActionBar(int index)
    {
        PlayerInput.AllowChangeActionBarItem = true;
        Player.Instance.PlayerInventory.SetActionBarSlot(index);
        Player.Instance.PlayerInventory.SetIndex(index);
    }
}