using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace AutoTools;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.autotools";
    private const string PluginName = "AutoTools";
    private const string PluginVersion = "0.0.1";
    internal static ManualLogSource LOG { get; set; }
    
    internal static ConfigEntry<bool> EnableAutoTool { get; private set; }
    internal static ConfigEntry<bool> EnableAutoToolOnFarmTiles { get; private set; }
    internal static ConfigEntry<bool> EnableAutoPickaxe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoAxe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoScythe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoFishingRod { get; private set; }
   // internal static ConfigEntry<int> AutoFishingRodDistance { get; private set; }
    internal static ConfigEntry<bool> EnableAutoWateringCan { get; private set; }
    internal static ConfigEntry<bool> EnableAutoSword { get; private set; }
    internal static ConfigEntry<bool> EnableAutoHoe { get; private set; }
    

    private void Awake()
    {
      SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        LOG = new ManualLogSource("AutoTools");
        BepInEx.Logging.Logger.Sources.Add(LOG);

        EnableAutoTool = Config.Bind("01. General", "Enable AutoTools", true, new ConfigDescription("Enable AutoTools.", null, new ConfigurationManagerAttributes {Order = 26}));
        EnableAutoToolOnFarmTiles = Config.Bind("01. General", "Enable AutoTools On Farm Tiles", true, new ConfigDescription("Enable AutoTools On Farm Tiles.", null, new ConfigurationManagerAttributes {Order = 25}));
        EnableAutoPickaxe = Config.Bind("02. Specific Tools", "Enable AutoPickaxe", true, new ConfigDescription("Enable AutoPickaxe.", null, new ConfigurationManagerAttributes {Order = 24}));
        EnableAutoAxe = Config.Bind("02. Specific Tools", "Enable AutoAxe", true, new ConfigDescription("Enable AutoAxe.", null, new ConfigurationManagerAttributes {Order = 23}));
        EnableAutoScythe = Config.Bind("02. Specific Tools", "Enable AutoScythe", true, new ConfigDescription("Enable AutoScythe.", null, new ConfigurationManagerAttributes {Order = 22}));
        EnableAutoFishingRod = Config.Bind("02. Specific Tools", "Enable AutoFishingRod", true, new ConfigDescription("Enable AutoFishingRod.", null, new ConfigurationManagerAttributes {Order = 21}));
        //AutoFishingRodDistance = Config.Bind("01. General", "AutoFishingRodDistance", 20, new ConfigDescription("Distance to water to switch.", new AcceptableValueRange<int>(1,20), new ConfigurationManagerAttributes {Order = 20}));
        EnableAutoWateringCan = Config.Bind("02. Specific Tools", "Enable AutoWateringCan", true, new ConfigDescription("Enable AutoWateringCan.", null, new ConfigurationManagerAttributes {Order = 19}));
        EnableAutoSword = Config.Bind("02. Specific Tools", "Enable AutoSword", true, new ConfigDescription("Enable AutoSword.", null, new ConfigurationManagerAttributes {Order = 18}));
        EnableAutoHoe = Config.Bind("02. Specific Tools", "Enable AutoHoe", true, new ConfigDescription("Enable AutoHoe.", null, new ConfigurationManagerAttributes {Order = 17}));
        
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogWarning($"Plugin {PluginName} is loaded!");
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Plugin.LOG.LogWarning($"Scene loaded: {arg0.name}");
        Patches.UpdateToolIndexes();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        LOG.LogError($"{PluginName} has been destroyed!");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        LOG.LogError($"{PluginName} has been disabled!");
    }
}