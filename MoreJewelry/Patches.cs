namespace MoreJewelry;

/// <summary>
/// Contains Harmony patches for modifying and extending the functionality of various game methods.
/// </summary>
[Harmony]
public static class Patches
{
    /// <summary>
    /// A list of custom gear slots added by the mod.
    /// </summary>
    [UsedImplicitly]public static List<Slot> GearSlots = [];
    
    
    /// <summary>
    /// Harmony prefix patch for PlayerInventory's LoadPlayerInventory method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <remarks>
    /// Initializes and sets up custom gear slots and panels if they haven't been created already.
    /// </remarks>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LoadPlayerInventory))]
    private static void PlayerInventory_Initialize(PlayerInventory __instance)
    {
        if (UI.SlotsCreated && UI.GearPanel != null)
        {
            Utils.Log("Slots already created. Skipping slot creation etc.");
            return;
        }

        UI.InitializeGearPanel();
        UI.CreateSlots(__instance, ArmorType.Ring, 2);
        UI.CreateSlots(__instance, ArmorType.Keepsake, 2);
        UI.CreateSlots(__instance, ArmorType.Amulet, 2);

        __instance.SetUpInventoryData();

        var characterPanelSlots = GameObject.Find(Const.CharacterPanelSlotsPath);
        if (characterPanelSlots != null)
        {
            UI.GearPanel.transform.SetParent(characterPanelSlots.transform, true);
            UI.GearPanel.transform.SetAsLastSibling();
            UI.GearPanel.transform.localPosition = Const.ShowPosition;
        }
        else
        {
            Plugin.LOG.LogError("Character Panel Slots not found. Please report this.");
        }

        UI.UpdatePanelVisibility();
        UI.SlotsCreated = true;
    }


    /// <summary>
    /// Harmony postfix patch for TranslationLayer's ChangeLanguage method.
    /// </summary>
    /// <remarks>
    /// Updates the title text and popup text based on the current language setting. Relies on the <see cref="UI.UpdateTitleText"/> and <see cref="UI.UpdatePopupText"/> methods in the <see cref="UI"/> class.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TranslationLayer), nameof(TranslationLayer.ChangeLanguage))]
    private static void TranslationLayer_ChangeLanguage()
    {
        UI.UpdateTitleText(true);
        UI.UpdatePopupText();
    }


    /// <summary>
    /// Harmony postfix patch for PlayerInventory's GetStat method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <param name="stat">The type of stat being retrieved.</param>
    /// <param name="__result">The result value of the original GetStat method.</param>
    /// <remarks>
    /// Modifies the stat calculation to include custom gear slots.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.GetStat))]
    public static void PlayerInventory_GetStat(ref PlayerInventory __instance, StatType stat, ref float __result)
    {
        __result += __instance.GetStatValueFromSlot(ArmorType.Ring, 65, 2, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Ring, 66, 3, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Keepsake, 67, 1, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Keepsake, 68, 2, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Amulet, 69, 1, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Amulet, 70, 2, stat);
    }

    /// <summary>
    /// Harmony postfix patch for PlayerInventory's Awake method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <remarks>
    /// Attaches additional actions to the OnInventoryUpdated event for cleaning up armor dictionaries. This event is triggered when
    /// the player opens or closes their inventory.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.Awake))]
    public static void PlayerInventory_Awake(ref PlayerInventory __instance)
    {
        if (UI.ActionAttached) return;
        UI.ActionAttached = true;

        Utils.Log("Attaching RemoveNullValuesAndLogFromDictionary action to OnInventoryUpdated event.");
        var instance = __instance;
        __instance.OnInventoryUpdated += () =>
        {
            if (instance == null || instance.currentRealArmor == null || instance.currentArmor == null)
            {
                Plugin.LOG.LogError("OnInventoryUpdated: PlayerInventory instance is null. It is advised to restart your game.");
                return;
            }
            Utils.Log("OnInventoryUpdated: Cleaning armor dictionaries.");
            Utils.RemoveNullValuesAndLogFromDictionary(instance.currentRealArmor, "CurrentRealArmor");
            Utils.RemoveNullValuesAndLogFromDictionary(instance.currentArmor, "CurrentArmor");
        };
    }

    /// <summary>
    /// Harmony postfix patch for PlayerInventory's LateUpdate method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <remarks>
    /// Ensures custom gear slots are equipped with non-visual armor items. This is where the game begins to calculate stats the gear provides.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LateUpdate))]
    private static void PlayerInventory_EquipNonVisualArmor(ref PlayerInventory __instance)
    {
        if (!UI.SlotsCreated) return;
        __instance.EquipNonVisualArmor(65, ArmorType.Ring, 2);
        __instance.EquipNonVisualArmor(66, ArmorType.Ring, 3);
        __instance.EquipNonVisualArmor(67, ArmorType.Keepsake, 1);
        __instance.EquipNonVisualArmor(68, ArmorType.Keepsake, 2);
        __instance.EquipNonVisualArmor(69, ArmorType.Amulet, 1);
        __instance.EquipNonVisualArmor(70, ArmorType.Amulet, 2);
    }

    /// <summary>
    /// Harmony postfix patch for MainMenuController's EnableMenu method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="MainMenuController"/> being patched.</param>
    /// <param name="menu">The menu GameObject being enabled.</param>
    /// <remarks>
    /// Resets the mod to its initial state when certain menus are enabled.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    private static void MainMenuController_EnableMenu(ref MainMenuController __instance, ref GameObject menu)
    {
        if (menu == __instance.homeMenu || menu == __instance.newCharacterMenu || menu == __instance.loadCharacterMenu || menu == __instance.singlePlayerMenu || menu == __instance.multiplayerMenu)
        {
            Utils.ResetMod();
        }
    }


}