﻿using System.Collections.Generic;
using UnityEngine;
using Wish;
using Object = UnityEngine.Object;
using System.Reflection;

namespace MoreScythesRedux;

public static class ItemHandler
{
    private const int OriginalScytheId = 3000;
    private const int AdamantScytheId = 30000;
    private const int MithrilScytheId = 30020;
    private const int SuniteScytheId = 30030;
    private const int GloriteScytheId = 30090;
    private const string RecipeListAnvil = "RecipeList_Anvil";
    private const string RecipeListMonsterAnvil = "RecipeList_Monster Anvil";

    private static void CreateAndConfigureItem(int id, int speed, int damage)
    {
        var original = ItemDatabase.GetItemData(OriginalScytheId);

        var item = ScriptableObject.CreateInstance<ItemData>();
        JsonUtility.FromJsonOverwrite(
            FileLoader.LoadFile(Assembly.GetExecutingAssembly(), $"data.{id}.json"), 
            item);

        item.icon = SpriteUtil.CreateSprite(
            FileLoader.LoadFileBytes(Assembly.GetExecutingAssembly(), $"img.{id}.png"), 
            $"Modded item icon {id}");

        var useItem = Object.Instantiate(original.useItem);
        if (!useItem)
        {
            Plugin.Log.LogError("Original scythe has no useItem");
            return;
        }

        item.useItem = useItem;
        useItem.gameObject.GetComponent<Weapon>()._frameRate = speed;
        useItem.gameObject.GetComponent<DamageSource>()._damageRange.Set(damage - 8, damage);
        useItem.gameObject.SetActive(false);

        Object.DontDestroyOnLoad(useItem);
        ItemDatabase.items[item.id] = item;
        ItemDatabase.ids[item.name.RemoveWhitespace().ToLower()] = item.id;
        ItemDatabase.itemDatas[item.id] = item;

        Plugin.Log.LogInfo($"Created item {item.id} with name {item.name}");
    }

    private static void ConfigureRecipe(int itemId, string recipeListName, List<ItemInfo> inputs, float craftingHours)
    {
        foreach (var rl in Resources.FindObjectsOfTypeAll<RecipeList>())
        {
            if (!rl.name.Equals(recipeListName)) continue;

            if (rl.craftingRecipes.Exists(r => r.output.item.id == itemId))
            {
                continue;
            }

            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.output = new ItemInfo { item = ItemDatabase.GetItemData(itemId), amount = 1 };
            recipe.input = inputs;
            recipe.worldProgressTokens = new List<Progress>();
            recipe.characterProgressTokens = new List<Progress>();
            recipe.questProgressTokens = new List<QuestAsset>();
            recipe.hoursToCraft = craftingHours;

            rl.craftingRecipes.Add(recipe);
            Plugin.Log.LogInfo($"Added item {itemId} to {recipeListName}");
        }
    }

    public static void CreateScytheItems()
    {
        var scytheDefinitions = new List<(int id, int speed, int damage, string recipeList, List<ItemInfo> inputs, float craftingHours)>
        {
            (AdamantScytheId, 13, 14, RecipeListAnvil, new List<ItemInfo> { new() { item = ItemDatabase.GetItemData(ItemID.AdamantBar), amount = 10 } }, 6f),
            (MithrilScytheId, 14, 18, RecipeListAnvil, new List<ItemInfo> { new() { item = ItemDatabase.GetItemData(ItemID.MithrilBar), amount = 10 } }, 12f),
            (SuniteScytheId, 15, 22, RecipeListAnvil, new List<ItemInfo> { new() { item = ItemDatabase.GetItemData(ItemID.SuniteBar), amount = 10 } }, 24f),
            (GloriteScytheId, 16, 26, RecipeListMonsterAnvil, new List<ItemInfo> { new() { item = ItemDatabase.GetItemData(ItemID.GloriteBar), amount = 10 } }, 48f),
        };

        foreach (var (id, speed, damage, recipeList, inputs, craftingHours) in scytheDefinitions)
        {
            CreateAndConfigureItem(id, speed, damage);
            ConfigureRecipe(id, recipeList, inputs, craftingHours);
        }
    }
}