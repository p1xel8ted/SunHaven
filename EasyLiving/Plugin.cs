using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wish;

namespace EasyLiving;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.easyliving";
    private const string PluginName = "Easy Living";
    private const string PluginVersion = "0.0.6";
    private static ConfigEntry<KeyboardShortcut> SaveShortcut { get; set; }
    private static ConfigEntry<bool> EnableSaveShortcut { get; set; }
    public static ConfigEntry<bool> SkipMuseumMissingItemsDialogue { get; private set; }
    private static ConfigEntry<bool> UnityLogging { get; set; }
    public static ConfigEntry<bool> RemoveUnneededButtonsInMainMenu { get; private set; }
    public static ConfigEntry<bool> AddQuitToDesktopButton { get; private set; }
    public static ConfigEntry<bool> EnableAdjustQuestTrackerHeightView { get; private set; }
    public static ConfigEntry<bool> AutoLoadMostRecentSave { get; private set; }
    public static ConfigEntry<KeyboardShortcut> SkipAutoLoadMostRecentSaveShortcut { get; private set; }
    public static ConfigEntry<int> AdjustQuestTrackerHeightView { get; private set; }
    public static ConfigEntry<bool> ApplyMoveSpeedMultiplier { get; private set; }
    public static ConfigEntry<float> MoveSpeedMultiplier { get; private set; }
    public static ConfigEntry<KeyboardShortcut> MoveSpeedMultiplierIncrease { get; private set; }
    public static ConfigEntry<KeyboardShortcut> MoveSpeedMultiplierDecrease { get; private set; }
    internal static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        LOG = new ManualLogSource("Easy Living");
        BepInEx.Logging.Logger.Sources.Add(LOG);
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        UnityLogging = Config.Bind("01. Debug", "Unity Logging", false, new ConfigDescription("Toggle Unity logging. Useful for debugging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = -1}));
        UnityLogging.SettingChanged += (_, _) => { Debug.unityLogger.logEnabled = UnityLogging.Value; };
        Debug.unityLogger.logEnabled = UnityLogging.Value;
        EnableSaveShortcut = Config.Bind("02. Keyboard Shortcuts", "Enable Quick Save", true, new ConfigDescription("Enable quick saving via the keybind below.", null, new ConfigurationManagerAttributes {Order = 20}));
        SaveShortcut = Config.Bind("02. Keyboard Shortcuts", "Quick Save", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("Keybind to press to manual save game. Note that it doesn't save location, just progress.", null, new ConfigurationManagerAttributes {Order = 19}));
        SkipMuseumMissingItemsDialogue = Config.Bind("03. Museum", "Skip Missing Items Dialogue", true, new ConfigDescription("Skip the 'missing items' dialogue when you interact with a museum display.", null, new ConfigurationManagerAttributes {Order = 1}));
        RemoveUnneededButtonsInMainMenu = Config.Bind("04. UI", "Remove Unneeded Buttons In Main Menu", true, new ConfigDescription("Remove the Discord/Twitter/etc buttons from  the main menu.", null, new ConfigurationManagerAttributes {Order = 17}));
        AddQuitToDesktopButton = Config.Bind("04. UI", "Add Quit To Desktop Button", true, new ConfigDescription("Add a 'Quit To Desktop' button to the main menu. Bottom right X.", null, new ConfigurationManagerAttributes {Order = 16}));
        EnableAdjustQuestTrackerHeightView = Config.Bind("04. UI", "Enable Adjust Quest Tracker Height", true, new ConfigDescription("Enable adjusting the height of the quest tracker.", null, new ConfigurationManagerAttributes {Order = 15}));
        AdjustQuestTrackerHeightView = Config.Bind("04. UI", "Adjust Quest Tracker Height", Display.main.systemHeight / 3, new ConfigDescription("Adjust the height of the quest tracker.", new AcceptableValueRange<int>(-2000, 2000), new ConfigurationManagerAttributes {Order = 14}));
        ApplyMoveSpeedMultiplier = Config.Bind("05. Player", "Apply Move Speed Multiplier", true, new ConfigDescription("Apply a multiplier to the players base speed.", null, new ConfigurationManagerAttributes {Order = 13}));
        MoveSpeedMultiplier = Config.Bind("05. Player", "Move Speed Multiplier", 1.5f, new ConfigDescription("Adjust the player's move speed.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes {Order = 12}));
        MoveSpeedMultiplier.SettingChanged += (_, _) =>
        {
            MoveSpeedMultiplier.Value = Mathf.Clamp(MoveSpeedMultiplier.Value, 1f, 10f);
        };
        MoveSpeedMultiplierIncrease = Config.Bind("05. Player", "Move Speed Multiplier Increase", new KeyboardShortcut(KeyCode.LeftBracket), new ConfigDescription("Keybind to increase the player's move speed multiplier.", null, new ConfigurationManagerAttributes {Order = 11}));
        MoveSpeedMultiplierDecrease = Config.Bind("05. Player", "Move Speed Multiplier Decrease", new KeyboardShortcut(KeyCode.RightBracket), new ConfigDescription("Keybind to decrease the player's move speed multiplier.", null, new ConfigurationManagerAttributes {Order = 10}));
        AutoLoadMostRecentSave = Config.Bind("06. Saves", "Auto Load Most Recent Save", true, new ConfigDescription("Automatically load the most recent save when starting the game.", null, new ConfigurationManagerAttributes {Order = 11}));
        SkipAutoLoadMostRecentSaveShortcut = Config.Bind("06. Saves", "Skip Auto Load Most Recent Save Shortcut", new KeyboardShortcut(KeyCode.LeftShift), new ConfigDescription("Keybind to hold to skip auto loading the most recent save.", null, new ConfigurationManagerAttributes {Order = 10}));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var refreshRate = Screen.resolutions.Max(a => a.refreshRate);
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, Screen.fullScreenMode == FullScreenMode.FullScreenWindow, refreshRate);
        Time.fixedDeltaTime = 1f / refreshRate;
        LOG.LogInfo($"Screen resolution set to {Display.main.systemWidth}x{Display.main.systemHeight} @ {refreshRate}Hz");
        LOG.LogInfo($"FixedDeltaTime set to {Time.fixedDeltaTime}");
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