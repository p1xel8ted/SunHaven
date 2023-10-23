using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Wish;

namespace MuseumSellPrice;

[Harmony]
public static class Patches
{
    private const string WouldLookGoodInAMuseum = "would look good in a museum";
    private const string LeafieTrinket = "Leafie Trinket";
    private const string FairyWings = "Fairy Wings";
    private const string ManaSap = "Mana Sap";
    private const string MysteriousAntler = "Mysterious Antler";
    private const string MonsterCandy = "Monster Candy";
    private const string DragonFang = "Dragon Fang";
    private const string UnicornHairTuft = "Unicorn Hair Tuft";
    private const string NelVarianRunestone = "Nel'Varian Runestone";
    private const string AncientElvenHeaddress = "Ancient Elven Headdress";
    private const string AncientMagicStaff = "Ancient Magic Staff";
    private const string TentacleMonsterEmblem = "Tentacle Monster Emblem";
    private const string AncientNagaCrook = "Ancient Naga Crook";
    private const string AncientAngelQuill = "Ancient Angel Quill";
    private const string AncientNelVarian = "Ancient Nel'Varian";
    private const string AncientWithergate = "Ancient Withergate";
    private const string AncientSunHaven = "Ancient Sun Haven";
    private const string OriginsOfSunHavenAndElios = "Origins of Sun Haven and Elios";
    private const string OriginsOfTheGrandTree = "Origins of the Grand Tree";
    private const string OriginsOfDynus = "Origins of Dynus";

    private static readonly HashSet<string> ExcludedNames = new()
    {
        AncientNelVarian,
        AncientWithergate,
        AncientSunHaven,
        FairyWings,
        ManaSap,
        MysteriousAntler,
        MonsterCandy,
        DragonFang,
        UnicornHairTuft,
        NelVarianRunestone,
        AncientElvenHeaddress,
        AncientMagicStaff,
        TentacleMonsterEmblem,
        AncientNagaCrook,
        AncientAngelQuill
    };

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.ConstructDatabase), typeof(ItemData[]))]
    private static void ItemDatabase_ConstructDatabase()
    {
        if (!Plugin.Enabled.Value) return;
        
        foreach (var item in ItemDatabase.items.Where(a => a != null))
        {
            if (item.description != null && !item.description.Contains(WouldLookGoodInAMuseum)) continue;
        
            if (item.sellPrice <= 11f)
            {
                if (ExcludedNames.Contains(item.name))
                {
                    AdjustSellPriceForExcludedNames(item);
                }
                else if (item.name == LeafieTrinket)
                {
                    item.sellPrice *= Plugin.Multiplier.Value / 3;
                }
                else
                {
                    item.sellPrice *= Plugin.Multiplier.Value;
                }
            }
        
            AdjustOtherConditions(item);
        }
    }

    private static void AdjustSellPriceForExcludedNames(ItemData item)
    {
        if (item.name.Contains(FairyWings) || item.name.Contains(ManaSap) || item.name.Contains(MysteriousAntler))
        {
            item.sellPrice = 0f;
            item.orbsSellPrice = Plugin.Multiplier.Value;
        }
        else if (item.name.Contains(MonsterCandy) || item.name.Contains(DragonFang) || item.name.Contains(UnicornHairTuft))
        {
            item.sellPrice = 0f;
            item.ticketSellPrice = Plugin.Multiplier.Value;
        }
        else if (item.name.Contains(NelVarianRunestone) || item.name.Contains(AncientElvenHeaddress) || item.name.Contains(AncientMagicStaff))
        {
            item.sellPrice = 0f;
            item.orbsSellPrice = Plugin.Multiplier.Value;
        }
        else if (item.name.Contains(TentacleMonsterEmblem) || item.name.Contains(AncientNagaCrook) || item.name.Contains(AncientAngelQuill))
        {
            item.sellPrice = 0f;
            item.ticketSellPrice = Plugin.Multiplier.Value;
        }
        else if (item.name.Contains(AncientNelVarian) && item.sellPrice <= 101f)
        {
            item.orbsSellPrice *= Plugin.Multiplier.Value / 2;
        }
        else if (item.name.Contains(AncientWithergate) && item.sellPrice <= 101f)
        {
            item.ticketSellPrice *= Plugin.Multiplier.Value / 2;
        }
        else if (item.name.Contains(AncientSunHaven) && item.sellPrice <= 101f)
        {
            item.sellPrice *= Plugin.Multiplier.Value / 3;
        }
    }

    private static void AdjustOtherConditions(ItemData item)
    {
        if (item.name.Contains(OriginsOfSunHavenAndElios) && item.sellPrice <= 2f)
        {
            item.sellPrice *= 10 * Plugin.Multiplier.Value / 2;
        }
        else if (item.name.Contains(OriginsOfTheGrandTree) && item.sellPrice <= 2f)
        {
            item.orbsSellPrice = Plugin.Multiplier.Value / 2;
            item.sellPrice = 0f;
        }
        else if (item.name.Contains(OriginsOfDynus) && item.sellPrice <= 2f)
        {
            item.ticketSellPrice = Plugin.Multiplier.Value / 2;
            item.sellPrice = 0f;
        }
    }
}