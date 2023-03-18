using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace NoTimeForFishing
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.notimeforfishing";
        private const string PluginName = "No Time For Fishing!";
        private const string PluginVersion = "0.0.2";

        public static ConfigEntry<bool> DisableCaughtFishWindow;
        public static ConfigEntry<bool> SkipFishingMiniGame;
        public static ConfigEntry<bool> AutoReel;
        public static ConfigEntry<bool> InstantAutoReel;

        internal static ConfigEntry<bool> NoMoreNibbles;
        internal static ConfigEntry<int> FishingRodCastSpeed;
        internal static ConfigEntry<float> FishSpawnMultiplier;
        internal static ConfigEntry<int> FishSpawnLimit;
        internal static ConfigEntry<float> MiniGameMaxSpeed;
        internal static ConfigEntry<float> MiniGameWinAreaMultiplier;
        internal static ConfigEntry<bool> DoubleBaseFishSwimSpeed;

        internal static ConfigEntry<bool> DoubleBaseBobberAttractionRadius;
        
        public static ConfigEntry<bool> Debug;

        internal static ManualLogSource LOG { get; set; }


        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);

            //Bobber section
            DoubleBaseBobberAttractionRadius = Config.Bind("Bobber", "Double Base Bobber Attraction Radius", true, new ConfigDescription("Doubles the base radius of the bobber attraction. Your talent bonus' are applied afterwards.", null, new ConfigurationManagerAttributes {Order = 1}));
            
            //Fish section
            NoMoreNibbles = Config.Bind("Fish", "No More Nibbles", true, new ConfigDescription("Disable nibbling and fleeing behavior for fish.", null, new ConfigurationManagerAttributes {Order = 2}));
            FishSpawnLimit = Config.Bind("Fish", "Fish Spawn Limit", 100, new ConfigDescription("Adjust the maximum number of fish that can spawn.", new AcceptableValueRange<int>(1, 1500), new ConfigurationManagerAttributes {Order = 3}));
            FishSpawnMultiplier = Config.Bind("Fish", "Fish Spawn Multiplier", 100f, new ConfigDescription("Adjust the number of fish that spawn.", new AcceptableValueRange<float>(1, 1500), new ConfigurationManagerAttributes {Order = 4}));
            DoubleBaseFishSwimSpeed = Config.Bind("Fish", "Double Base Fish Swim Speed", true, new ConfigDescription("Double the base speed at which fish swim to the bobber. Your talent bonus' are applied afterwards.", null, new ConfigurationManagerAttributes {Order = 5}));
            
            //Fishing Rod section
            AutoReel = Config.Bind("Fishing Rod", "Auto Reel", true, new ConfigDescription("Automatically reel in fish when they bite.", null, new ConfigurationManagerAttributes {Order = 6}));
            InstantAutoReel = Config.Bind("Fishing Rod", "Instant Auto Reel", true, new ConfigDescription("Reel in fish instantly when they bite.", null, new ConfigurationManagerAttributes {Order = 7}));
            FishingRodCastSpeed = Config.Bind("Fishing Rod", "Fishing Rod Cast Speed", 5, new ConfigDescription("Adjust the base casting speed of the fishing rod. Your talent bonus' are applied afterwards.", new AcceptableValueRange<int>(1, 10), new ConfigurationManagerAttributes {Order = 8}));

            //Mini-Game section
            MiniGameMaxSpeed = Config.Bind("Mini-Game", "Mini Game Max Speed", 0.5f, new ConfigDescription("Adjust the maximum speed of the fishing mini-game.", new AcceptableValueRange<float>(0.1f, 1f), new ConfigurationManagerAttributes {Order = 10}));
            MiniGameWinAreaMultiplier = Config.Bind("Mini-Game", "Mini Game Win Area Multiplier", 2f, new ConfigDescription("Adjust the size of the winning area in the fishing mini-game.", new AcceptableValueRange<float>(1, 20), new ConfigurationManagerAttributes {Order = 11}));
            SkipFishingMiniGame = Config.Bind("Mini-Game", "Skip Fishing Mini Game", true, new ConfigDescription("Skip the fishing mini-game entirely.", null, new ConfigurationManagerAttributes {Order = 12}));

            //Miscellaneous section
            DisableCaughtFishWindow = Config.Bind("Miscellaneous", "Disable Caught Fish Window", true, new ConfigDescription("Disable the window that displays information about caught fish.", null, new ConfigurationManagerAttributes {Order = 13}));
 

            //Debug section
            Debug = Config.Bind("Debug", "Debug", false, new ConfigDescription("Enable debug for logging.", null, new ConfigurationManagerAttributes {Order = 14}));


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
}