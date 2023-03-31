using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace EasyLiving;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.easyliving";
    private const string PluginName = "EasyLiving";
    private const string PluginVersion = "0.0.3";
    private static ConfigEntry<KeyboardShortcut> SaveShortcut { get; set; }
    private static ConfigEntry<bool> EnableSaveShortcut { get; set; }
    public static ConfigEntry<bool> SkipMuseumMissingItemsDialogue { get; private set; }
    public static ConfigEntry<bool> AllowRemovalOfGrowingCrops { get; private set; }

    public static ConfigEntry<bool> RemoveUnneededButtonsInMainMenu { get; private set; }
    public static ConfigEntry<bool> AddQuitToDesktopButton { get; private set; }
    public static ConfigEntry<bool> EnableAdjustQuestTrackerHeightView { get; private set; }
    public static ConfigEntry<int> AdjustQuestTrackerHeightView { get; private set; }
   
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        EnableSaveShortcut = Config.Bind("Keyboard Shortcuts", "Enable Quick Save", true, new ConfigDescription("Enable quick saving via the keybind below.", null, new ConfigurationManagerAttributes {Order = 7}));
        SaveShortcut = Config.Bind("Keyboard Shortcuts", "Quick Save", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("Keybind to press to manual save game. Note that it doesnt save location, just progress.", null, new ConfigurationManagerAttributes {Order = 6}));
        SkipMuseumMissingItemsDialogue = Config.Bind("Museum", "Skip Missing Items Dialogue", true, new ConfigDescription("Skip the 'missing items' dialogue when you interact with a museum display.", null, new ConfigurationManagerAttributes {Order = 5}));
        AllowRemovalOfGrowingCrops = Config.Bind("Crops", "Allow Removal Of Growing Crops", true, new ConfigDescription("Allow removal of growing crops. 50% chance to receive the seed back.", null, new ConfigurationManagerAttributes {Order = 4}));
        RemoveUnneededButtonsInMainMenu = Config.Bind("UI", "Remove Unneeded Buttons In Main Menu", true, new ConfigDescription("Remove the Discord/Twitter/etc buttons from  the main menu.", null, new ConfigurationManagerAttributes {Order = 3}));
        AddQuitToDesktopButton = Config.Bind("UI", "Add Quit To Desktop Button", true, new ConfigDescription("Add a 'Quit To Desktop' button to the main menu. Bottom right X.", null, new ConfigurationManagerAttributes {Order = 2}));
        EnableAdjustQuestTrackerHeightView = Config.Bind("UI", "Enable Adjust Quest Tracker Height", true, new ConfigDescription("Enable adjusting the height of the quest tracker.", null, new ConfigurationManagerAttributes {Order = 1}));
        AdjustQuestTrackerHeightView = Config.Bind("UI", "Adjust Quest Tracker Height", Display.main.systemHeight / 3, new ConfigDescription("Adjust the height of the quest tracker.", new AcceptableValueRange<int>(-2000, 2000), new ConfigurationManagerAttributes {Order = 0}));
  
        LOG = new ManualLogSource("Log");
        BepInEx.Logging.Logger.Sources.Add(LOG);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogWarning($"Plugin {PluginName} is loaded!");
    }

    private void OnDestroy()
    {
        LOG.LogError("I've been destroyed!");
    }

    private void OnDisable()
    {
        LOG.LogError("I've been disabled!");
    }
}