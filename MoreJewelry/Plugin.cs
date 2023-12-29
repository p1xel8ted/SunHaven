﻿namespace MoreJewelry;

/// <summary>
/// Main plugin class for More Jewelry!, responsible for initializing and managing plugin configurations and behaviors.
/// </summary>
/// <remarks>
/// This class handles the setup and teardown of the plugin, including configuration settings, event subscriptions, and Harmony patching.
/// </remarks>
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.morejewelry";
    private const string PluginName = "More Jewelry!";
    private const string PluginVersion = "0.1.1";

    /// <summary>
    /// Gets the logging source for the plugin.
    /// </summary>
    internal static ManualLogSource LOG { get; private set; }

    /// <summary>
    /// Configuration entry for enabling debug logging.
    /// </summary>
    internal static ConfigEntry<bool> Debug { get; private set; }

    /// <summary>
    /// Configuration entry for showing the panel toggle.
    /// </summary>
    internal static ConfigEntry<bool> ShowPanelToggle { get; private set; }

    /// <summary>
    /// Configuration entry for showing tooltips.
    /// </summary>
    internal static ConfigEntry<bool> ShowTooltips { get; private set; }

    /// <summary>
    /// Configuration entry for ignoring toggle with controller.
    /// </summary>
    internal static ConfigEntry<bool> IgnoreToggleWithController { get; private set; }
    
    /// <summary>
    /// Configuration entry for new ring/amulet/keepsake swap behaviour.
    /// </summary>
    internal static ConfigEntry<bool> UseAdjustedEquipping { get; private set; }

    /// <summary>
    /// Initialization logic for the plugin.
    /// </summary>
    private void Awake()
    {
        UIHandler.OnInventoryOpened += UI.UIHandler_OpenInventory;
        UIHandler.OnInventoryClosed += UI.UIHandler_CloseInventory;
        LOG = Logger;
        Debug = Config.Bind("00. Debug", "Debug", false, new ConfigDescription("Enable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = false, Order = 99}));
        UseAdjustedEquipping = Config.Bind("01. General", "Use Adjusted Equipping", true, new ConfigDescription("Use adjusted equipping logic for rings, amulets, and keepsakes.", null, new ConfigurationManagerAttributes {Order = 0}));
        ShowPanelToggle = Config.Bind("01. General", "Show Panel Toggle", true, new ConfigDescription("Show the panel toggle in the character panel.", null, new ConfigurationManagerAttributes {Order = 1}));
        ShowPanelToggle.SettingChanged += (_, _) =>
        {
            UI.UpdatePanelVisibility();
            UI.UpdateNavigationElements();
        };
        ShowTooltips = Config.Bind("01. General", "Show Tooltips", true, new ConfigDescription("Show tooltips.", null, new ConfigurationManagerAttributes {Order = 2}));
        ShowTooltips.SettingChanged += (_, _) =>
        {
            if (UI.LeftArrowPopup != null)
            {
                UI.LeftArrowPopup.enabled = ShowTooltips.Value;
            }
            if (UI.RightArrowPopup != null)
            {
                UI.RightArrowPopup.enabled = ShowTooltips.Value;
            }
        };
        IgnoreToggleWithController = Config.Bind("01. General", "Ignore Toggle With Controller", false, new ConfigDescription("Ignore toggle with controller.", null, new ConfigurationManagerAttributes {Order = 3}));
        IgnoreToggleWithController.SettingChanged += (_, _) =>
        {
            UI.UpdateNavigationElements();
        };
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    /// <summary>
    /// Logic to execute when the plugin is disabled.
    /// </summary>
    private void OnDisable()
    {
        LOG.LogError($"{PluginName} has been disabled!");
    }

    /// <summary>
    /// Cleanup logic for the plugin.
    /// </summary>
    private void OnDestroy()
    {
        LOG.LogError($"{PluginName} has been destroyed!");
    }
}