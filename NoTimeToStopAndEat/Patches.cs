using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using HarmonyLib;
using TranslatorPlugin;
using UnityEngine;
using Wish;

namespace NoTimeToStopAndEat;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Food), nameof(Food.EatFoodRoutine))]
    private static bool Food_EatFoodRoutine(ref Food __instance, bool fromLocal)
    {
        if (fromLocal)
        {
            PlayerInput.AllowChangeActionBarItem = false;
            __instance.player.moveSpeedMultipliers.Add(__instance.moveSpeedFloatRef);
            __instance.player.jumpMultipliers.Add(__instance.jumpFloatRef);
            var food = __instance;
            DOVirtual.Float(1f, 0f, 0.45f, delegate(float value) { food.moveSpeedFloatRef.value = value; });
            DOVirtual.Float(1f, 0f, 0.7f, delegate(float value) { food.jumpFloatRef.value = value; });
        }
    
    
        var vector = __instance.transform.position + new Vector3(0f, -0.15f, -1f);
        ParticleManager.Instance.InstantiateParticle(SingletonBehaviour<Prefabs>.Instance.eatParticles, vector);
    
        __instance._itemGraphics.gameObject.SetActive(false);
        if (fromLocal)
        {
            const float num = 1f;
            var num2 = 0f;
            if (__instance._foodItem.isFruit)
            {
                num2 = __instance.player.GetStat(StatType.FruitManaRestore);
            }
    
            if (__instance._foodItem.isMeal && GameSave.Farming.GetNode("Farming7b"))
            {
                num2 += GameSave.Farming.GetNodeAmount("Farming7b") * 5;
            }
    
            __instance.player.Heal(num * __instance._foodItem.health);
            __instance.player.AddMana(num * __instance._foodItem.mana + num2);
            __instance.HandleEXP(num);
            __instance.HandleStats();
            if (__instance._foodItem.isPotion && __instance._foodItem.statBuff.stats is {Count: > 0})
            {
                __instance.player.ReceiveBuff(__instance._foodItem.statBuff.buffType, new StatBuff
                {
                    buffType = __instance._foodItem.statBuff.buffType,
                    buffDescription = __instance._foodItem.statBuff.description,
                    buffName = __instance._foodItem.statBuff.buffName,
                    duration = __instance._foodItem.statBuff.duration,
                    entity = __instance.player,
                    stats = __instance._foodItem.statBuff.stats
                });
            }
    
            FishData fishData;
            if ((fishData = __instance._foodItem as FishData) != null && GameSave.Fishing.GetNode("Fishing8a"))
            {
                Food.wellFedBuff.entity = __instance.player;
                __instance.player.FinishBuff(BuffType.WellFed);
                var num3 = 0.5f + 0.5f * GameSave.Fishing.GetNodeAmount("FishinFishing8ag7a");
                switch (fishData.rarity)
                {
                    case ItemRarity.Common:
                    case ItemRarity.Uncommon:
                        Food.wellFedBuff.stats =
                        [
                            new Stat(StatType.ManaRegen, 0.075f * num3),
                            new Stat(StatType.Movespeed, 0.02f * num3)
                        ];
                        break;
                    case ItemRarity.Rare:
                    case ItemRarity.Epic:
                        Food.wellFedBuff.stats =
                        [
                            new Stat(StatType.ManaRegen, 0.125f * num3),
                            new Stat(StatType.Movespeed, 0.0275f * num3)
                        ];
                        break;
                    case ItemRarity.Legendary:
                        Food.wellFedBuff.stats =
                        [
                            new Stat(StatType.ManaRegen, 0.175f * num3),
                            new Stat(StatType.Movespeed, 0.035f * num3)
                        ];
                        break;
                }
    
                __instance.player.ReceiveBuff(BuffType.WellFed, Food.wellFedBuff);
            }
    
            Dictionary<int, int> foodStats = SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.foodStats;
            if (!foodStats.ContainsKey(__instance._foodItem.id))
            {
                foodStats[__instance._foodItem.id] = 1;
            }
            else
            {
                foodStats[__instance._foodItem.id] += 1;
            }
    
            if (GameSave.Exploration.GetNode("Exploration6c") && __instance._foodItem.isFruit && SingletonBehaviour<GameSave>.Instance.GetProgressIntCharacter("Exploration6c") < 40 * GameSave.Exploration.GetNodeAmount("Exploration6c"))
            {
                SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.AddStatBonus(StatType.Mana, 0.25f);
                SingletonBehaviour<GameSave>.Instance.SetProgressIntCharacter("Exploration6c", SingletonBehaviour<GameSave>.Instance.GetProgressIntCharacter("Exploration6c") + 1);
            }
    
            __instance._foodItem.GetStatAdditionByNumEaten();
            if (!SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Food"))
            {
                SingletonBehaviour<HelpTooltips>.Instance.SendNotification((string) TranslationLayer.TranslateObject("Food", "UI.HelpTooltips.Food.Title"), (string) TranslationLayer.TranslateObject("Eating food also <color=#39CCFF>increases your stats permanently</color>!\n\nThis bonus <color=#39CCFF>decreases</color> each time you eat the same food, so eat lots of <color=#39CCFF>different foods!</color>", "UI.HelpTooltips.EatingFoodAlsoColorCCFFincreasesYourStatsPermanentlycolorThisBonusColorCCFFdecreasescolorEachTimeYouEatTheSameFoodSoEatLotsOfColorCCFFdifferentFoodscolor.Title"), [], 4, delegate { SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Food", true); });
            }
    
            if (GameSave.Farming.GetNode("Farming3b") && __instance._foodItem.isMeal && SingletonBehaviour<GameSave>.Instance.GetProgressIntCharacter("Farming3b") < GameSave.Farming.GetNodeAmount("Farming3b") * 20)
            {
                SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.AddStatBonus(StatType.Mana, 0.5f);
                var num4 = SingletonBehaviour<GameSave>.Instance.GetProgressIntCharacter("Farming3b") + 1;
                SingletonBehaviour<GameSave>.Instance.SetProgressIntCharacter("Farming3b", num4);
            }
    
            PlayerInput.AllowChangeActionBarItem = true;
            __instance.player.Inventory.RemoveItemAt(__instance.player.ItemIndex, 1);
            AudioManager.Instance.PlayOneShot(SingletonBehaviour<Prefabs>.Instance.eatSound, __instance._itemGraphics.transform.position, 1.3f, 1.2f, 1.3f);
            __instance.OnConsumeFood();
        }
        
        __instance.player.moveSpeedMultipliers.Remove(__instance.moveSpeedFloatRef);
        __instance.player.jumpMultipliers.Remove(__instance.jumpFloatRef);
        __instance._itemGraphics.gameObject.SetActive(true);
        if (!__instance._foodItem.hasReaction)
        {
            __instance.player.CancelEmote();
        }
    
        return false;
    }
}