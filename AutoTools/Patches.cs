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

    private static Pickaxe PickaxeItem { get; set; }
    private static Axe AxeItem { get; set; }
    private static WateringCan WateringCanItem { get; set; }
    private static Sword SwordItem { get; set; }
    private static Hoe HoeItem { get; set; }

    internal static void UpdateToolIndexes()
    {
        foreach (var tool in ToolIndexUpdater.Keys)
        {
            ToolIndexUpdater[tool](-1); // Reset all indices
        }

        foreach (var item in Player.Instance.PlayerInventory._actionBarIcons)
        {
            foreach (var kvp in ToolIndexUpdater.Where(kvp => item.ItemImage._image.sprite.name.Contains(kvp.Key)))
            {
                kvp.Value(item.ItemImage.slotIndex);
                break;
            }
        }

        foreach (var kvp in ToolIndexUpdater)
        {
            switch (kvp.Key)
            {
                case "_pickaxe":
                    if (PickaxeIndex is not -1)
                    {
                        SetActionBar(PickaxeIndex);
                        PickaxeItem = Player.Instance._useItem as Pickaxe;
                    }

                    break;
                case "_axe":
                    if (AxeIndex is not -1)
                    {
                        SetActionBar(AxeIndex);
                        AxeItem = Player.Instance._useItem as Axe;
                    }

                    break;
                case "_watering_can":
                    if (WateringCanIndex is not -1)
                    {
                        SetActionBar(WateringCanIndex);
                        WateringCanItem = Player.Instance._useItem as WateringCan;
                    }

                    break;

                case "_sword":
                    if (SwordIndex is not -1)
                    {
                        SetActionBar(SwordIndex);
                        SwordItem = Player.Instance._useItem as Sword;
                    }

                    break;
                case "_hoe":
                    if (HoeIndex is not -1)
                    {
                        SetActionBar(HoeIndex);
                        HoeItem = Player.Instance._useItem as Hoe;
                    }

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

        var enemy = collider.GetComponent<EnemyAI>();
        var npc = collider.GetComponent<NPCAI>();
        var isEnemy = enemy is not null && npc is null;
        var rock = collider.GetComponent<Rock>();
        var tree = collider.GetComponent<Tree>();
        var crop = collider.GetComponent<Crop>();
        var wood = collider.GetComponent<Wood>();

        // Only call UpdateToolIndexes() if one of the components is found.
        if (enemy is not null || rock is not null || tree is not null || crop is not null || wood is not null)
        {
            UpdateToolIndexes();
        }

        if (isEnemy && Plugin.EnableAutoSword.Value)
        {
            if (SwordItem is not null && CanUse(SwordItem.toolData))
            {
                HandleToolInteraction(SwordIndex, "No sword on action bar!");
            }
            else
            {
                Notify("No usable sword on the action bar!", true);
            }
        }
        else if (rock && Plugin.EnableAutoPickaxe.Value)
        {
            if (PickaxeItem is not null && CanUse(PickaxeItem.toolData))
            {
                HandleToolInteraction(PickaxeIndex, "No pickaxe on action bar!");
            }
            else
            {
                Notify("No usable pickaxe on the action bar!", true);
            }
        }
        else if ((tree || wood) && Plugin.EnableAutoAxe.Value)
        {
            if (AxeItem is not null && CanUse(AxeItem.toolData))
            {
                HandleToolInteraction(AxeIndex, "No axe on action bar!");
            }
            else
            {
                Notify("No usable axe on the action bar!", true);
            }
        }
        else if (crop is not null)
        {
            // Specific logic for crops, as it has additional conditions
            if ((!crop.data.watered || crop.data.onFire) && Plugin.EnableAutoWateringCan.Value)
            {
                if (WateringCanItem is not null && CanUse(WateringCanItem.toolData))
                {
                    HandleToolInteraction(WateringCanIndex, "No watering can on action bar!");
                }
                else
                {
                    Notify("No usable water can on the action bar!", true);
                }
            }
            else if (crop.FullyGrown && Plugin.EnableAutoScythe.Value)
            {
                HandleToolInteraction(ScytheIndex, "No scythe on action bar!");
            }
        }
        else if (Plugin.EnableAutoFishingRod.Value && Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < 20 &&
                 SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y)))
        {
            HandleToolInteraction(FishingRodIndex, "No fishing rod on action bar!");
        }
    }

    private static bool CanUse(ToolData toolData)
    {
        return SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[toolData.profession].level >= toolData.requiredLevel;
    }

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