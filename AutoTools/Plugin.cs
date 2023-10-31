using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Wish;

namespace AutoTools;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.autotools";
    private const string PluginName = "Auto Tools";
    private const string PluginVersion = "0.0.4";
    private static ManualLogSource LOG { get; set; }
    internal static ConfigEntry<bool> EnableAutoTool { get; private set; }
    internal static ConfigEntry<bool> EnableAutoToolOnFarmTiles { get; private set; }
    internal static ConfigEntry<bool> EnableAutoPickaxe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoAxe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoScythe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoFishingRod { get; private set; }
    internal static ConfigEntry<bool> EnableAutoWateringCan { get; private set; }
    internal static ConfigEntry<bool> EnableAutoHoe { get; private set; }
    private static ConfigEntry<bool> EnableDebug { get; set; }
    internal static ConfigEntry<int> FishingRodWaterDetectionDistance { get; private set; }

    private void Awake()
    {
        LOG = new ManualLogSource("Auto Tools");
        BepInEx.Logging.Logger.Sources.Add(LOG);
        WateringCan.onWateringCanEmpty += FindNextWateringCan;
        EnableAutoTool = Config.Bind("01. General", "Enable AutoTools", true, new ConfigDescription("Enable AutoTools.", null, new ConfigurationManagerAttributes {Order = 26}));
        EnableAutoToolOnFarmTiles = Config.Bind("01. General", "Enable AutoTools On Farm Tiles", true, new ConfigDescription("Enable AutoTools On Farm Tiles.", null, new ConfigurationManagerAttributes {Order = 25}));
        EnableAutoPickaxe = Config.Bind("02. Specific Tools", "Enable AutoPickaxe", true, new ConfigDescription("Enable AutoPickaxe.", null, new ConfigurationManagerAttributes {Order = 24}));
        EnableAutoAxe = Config.Bind("02. Specific Tools", "Enable AutoAxe", true, new ConfigDescription("Enable AutoAxe.", null, new ConfigurationManagerAttributes {Order = 23}));
        EnableAutoScythe = Config.Bind("02. Specific Tools", "Enable AutoScythe", true, new ConfigDescription("Enable AutoScythe.", null, new ConfigurationManagerAttributes {Order = 22}));
        EnableAutoFishingRod = Config.Bind("02. Specific Tools", "Enable AutoFishingRod", true, new ConfigDescription("Enable AutoFishingRod.", null, new ConfigurationManagerAttributes {Order = 21}));
        FishingRodWaterDetectionDistance = Config.Bind("02. Specific Tools", "Fishing Rod Water Detection Distance", 5, new ConfigDescription("Control how far away from water you can be to use the fishing rod. Setting too high will cause swaps when you don't want it to...i.e combat near a water source", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes {Order = 20}));
        EnableAutoWateringCan = Config.Bind("02. Specific Tools", "Enable AutoWateringCan", true, new ConfigDescription("Enable AutoWateringCan.", null, new ConfigurationManagerAttributes {Order = 19}));
        EnableAutoHoe = Config.Bind("02. Specific Tools", "Enable AutoHoe", true, new ConfigDescription("Enable AutoHoe.", null, new ConfigurationManagerAttributes {Order = 17}));
        EnableDebug = Config.Bind("03. Debug", "Enable Debug", false, new ConfigDescription("Enable Debug.", null, new ConfigurationManagerAttributes {Order = 16}));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }
    
    internal static void DebugLog(string str)
    {
        if (!EnableDebug.Value) return;

        LOG.LogInfo(str);
    }

    private static void FindNextWateringCan()
    {
        Patches.FindBestTool(Patches.Tool.WateringCan);
    }

    private void OnDestroy()
    {
        WateringCan.onWateringCanEmpty -= FindNextWateringCan;
        LOG.LogError($"{PluginName} has been destroyed!");
    }

    private void OnDisable()
    {
        WateringCan.onWateringCanEmpty -= FindNextWateringCan;
        LOG.LogError($"{PluginName} has been disabled!");
    }
}