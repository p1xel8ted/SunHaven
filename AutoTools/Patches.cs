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

    private static Dictionary<int, ToolData> ToolIndexToItemMap { get; } = new();

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

        foreach (var item in Player.Instance.PlayerInventory._actionBarIcons)
        {
            foreach (var kvp in ToolIndexUpdater.Where(kvp => item.ItemImage is not null && item.ItemImage._image.sprite.name.Contains(kvp.Key)))
            {
                kvp.Value(item.ItemImage.slotIndex); // Update the tool's index
                ToolIndexToItemMap[item.ItemImage.slotIndex] = ItemDatabase.GetItemData(item.ItemImage.item) as ToolData; // Map the tool's index to the item's index
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


        if (__instance is null || collider is null || SceneSettingsManager.Instance is null || TileManager.Instance is null) return;

        if (Plugin.EnableDebug.Value)
        {
            Plugin.LOG.LogWarning($"PlayerInteractions_OnTriggerEnter2D: collider name: {collider.name}");
        }


        if (!Plugin.EnableAutoToolOnFarmTiles.Value && TileManager.Instance.HasFarmingTile(Player.Instance.Position, ScenePortalManager.ActiveSceneIndex))
        {
            return;
        }


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

        // if (isEnemy)
        // {
            ToolAction(SwordIndex, isEnemy, Plugin.EnableAutoSword.Value, "No sword on action bar!");
            
        //     if (Plugin.EnableAutoSword.Value)
        //     {
        //         var swordItemData = GetToolDataByToolIndex(SwordIndex);
        //         if (swordItemData is not null)
        //         {
        //             if (CanUse(swordItemData))
        //             {
        //                 HandleToolInteraction(SwordIndex, "No sword on action bar!");   
        //             }
        //             else
        //             {
        //                 Notify($"Your {swordItemData.profession} level is too low to use {swordItemData.name}!",true);
        //             }
        //         }
        //         else
        //         {
        //             Notify("Sword is null!", true); 
        //         }
        //
        //        
        //     }
        // }
        else if (rock && rock.Pickaxeable)
        {
            if (Plugin.EnableAutoPickaxe.Value)
            {
                var pickAxeToolData = GetToolDataByToolIndex(PickaxeIndex);
                if (pickAxeToolData is not null)
                {
                    if (CanUse(pickAxeToolData))
                    {
                        HandleToolInteraction(PickaxeIndex, "No pickaxe on action bar!");   
                    }
                    else
                    {
                        Notify($"Your {pickAxeToolData.profession} level is too low to use {pickAxeToolData.name}!",true);
                    }
                }
                else
                {
                    Notify("PickAxe is null!", true); 
                }
            }
        }
        else if ((tree && tree.Axeable) || (wood && wood.Axeable))
        {
            if (Plugin.EnableAutoAxe.Value)
            {
                var axeToolData = GetToolDataByToolIndex(AxeIndex);
                if (axeToolData is not null)
                {
                    if (CanUse(axeToolData))
                    {
                        HandleToolInteraction(AxeIndex, "No axe on action bar!");   
                    }
                    else
                    {
                        Notify($"Your {axeToolData.profession} level is too low to use {axeToolData.name}!",true);
                    }
                }
                else
                {
                    Notify("Axe is null!", true); 
                }
            }
        }
        else if (plant is not null || (collider.name.Contains("foliage") && !collider.name.Contains("prop")))
        {
            if (Plugin.EnableAutoScythe.Value)
            {
                var scytheToolData = GetToolDataByToolIndex(ScytheIndex);
                if (scytheToolData is not null)
                {
                    if (CanUse(scytheToolData))
                    {
                        HandleToolInteraction(ScytheIndex, "No scythe on action bar!");   
                    }
                    else
                    {
                        Notify($"Your {scytheToolData.profession} level is too low to use {scytheToolData.name}!",true);
                    }
                }
                else
                {
                    Notify("Scythe is null!", true); 
                }
                
            }
        }
        else if (crop is not null)
        {
            // Specific logic for crops, as it has additional conditions
            if (!crop.data.watered || crop.data.onFire)
            {
                if (Plugin.EnableAutoWateringCan.Value)
                {
                    var wateringCanToolData = GetToolDataByToolIndex(WateringCanIndex);
                    if (wateringCanToolData.GetItem() is WateringCanItem wc)
                    {
                        if(wc.WaterAmount > 0)
                        {
                            if (CanUse(wateringCanToolData))
                            {
                                HandleToolInteraction(WateringCanIndex, "No watering can on action bar!");   
                            }
                            else
                            {
                                Notify($"Your {wateringCanToolData.profession} level is too low to use {wateringCanToolData.name}!",true);
                            }
                        }
                        else
                        {
                            Notify($"Your {wateringCanToolData.name} is empty!",true);
                        }
                    }
                    else
                    {
                        Notify("Watering Can is null!", true); 
                    }
                }
            }
            else if (crop.FullyGrown)
            {
                if (Plugin.EnableAutoScythe.Value)
                {
                    var scytheToolData = GetToolDataByToolIndex(ScytheIndex);
                    if (scytheToolData is not null)
                    {
                        if (CanUse(scytheToolData))
                        {
                            HandleToolInteraction(ScytheIndex, "No scythe on action bar!");   
                        }
                        else
                        {
                            Notify($"Your {scytheToolData.profession} level is too low to use {scytheToolData.name}!",true);
                        }
                    }
                    else
                    {
                        Notify("Scythe is null!", true); 
                    }
                }
            }
        }
        else if (Vector2.Distance(Player.Instance.ExactGraphicsPosition, Utilities.MousePositionFloat()) < 20 &&
                 SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Utilities.MousePositionFloat().x, (int) Utilities.MousePositionFloat().y)))
        {
            if (Plugin.EnableAutoFishingRod.Value)
            {
                var rodToolData = GetToolDataByToolIndex(FishingRodIndex);
                if (rodToolData is not null)
                {
                    if (CanUse(rodToolData))
                    {
                        HandleToolInteraction(ScytheIndex, "No fishing rod on action bar!");   
                    }
                    else
                    {
                        Notify($"Your {rodToolData.profession} level is too low to use {rodToolData.name}!",true);
                    }
                }
                else
                {
                    Notify("Fishing Rod is null!", true); 
                }
            }
        }
    }
    
    private static void ToolAction(int tool, bool condition, bool pluginValue, string errorMessage)
    {
        if (condition && pluginValue)
        {
            var toolData = GetToolDataByToolIndex(tool);
            if (toolData != null)
            {
                if (CanUse(toolData))
                {
                    HandleToolInteraction(tool, errorMessage);
                }
                else
                {
                    Notify($"Your {toolData.profession} level is too low to use {toolData.name}!", true);
                }
            }
            else
            {
                Notify($"{tool} is null!", true);
            }
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