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
        private const string PluginVersion = "0.0.4";

        public static ConfigEntry<bool> DisableCaughtFishWindow;
        public static ConfigEntry<bool> SkipFishingMiniGame;
        public static ConfigEntry<bool> AutoReel;
        public static ConfigEntry<bool> InstantAutoReel;

        internal static ConfigEntry<bool> NoMoreNibbles;

        internal static ConfigEntry<bool> ModifyFishingRodCastSpeed;
        internal static ConfigEntry<int> FishingRodCastSpeed;

        internal static ConfigEntry<bool> ModifyFishSpawnMultiplier;
        internal static ConfigEntry<int> FishSpawnMultiplier;

        internal static ConfigEntry<bool> ModifyFishSpawnLimit;
        internal static ConfigEntry<int> FishSpawnLimit;

        internal static ConfigEntry<bool> ModifyMiniGameSpeed;
        internal static ConfigEntry<float> MiniGameMaxSpeed;

        internal static ConfigEntry<bool> ModifyMiniGameWinAreaMultiplier;
        internal static ConfigEntry<float> MiniGameWinAreaMultiplier;
        internal static ConfigEntry<bool> DoubleBaseFishSwimSpeed;

        internal static ConfigEntry<bool> DoubleBaseBobberAttractionRadius;
        internal static ConfigEntry<bool> EnhanceBaseCastLength;

        internal static ConfigEntry<bool> InstantAttraction;

        public static ConfigEntry<bool> Debug;

        internal static ManualLogSource LOG { get; set; }


        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);

            //Bobber section
            DoubleBaseBobberAttractionRadius = Config.Bind("Bobber", "Double Base Bobber Attraction Radius", true, new ConfigDescription("Doubles the base radius of the bobber attraction. Your talent bonus' are applied afterwards.", null, new ConfigurationManagerAttributes {Order = 2}));
            InstantAttraction = Config.Bind("Bobber", "Enable Instant Attraction", true, new ConfigDescription("Fish are immediately drawn to the bobber.", null, new ConfigurationManagerAttributes {Order = 1}));

            //Fish section
            NoMoreNibbles = Config.Bind("Fish", "No More Nibbles", true, new ConfigDescription("Disable nibbling and fleeing behavior for fish.", null, new ConfigurationManagerAttributes {Order = 6}));
            DoubleBaseFishSwimSpeed = Config.Bind("Fish", "Double Base Fish Swim Speed", true, new ConfigDescription("Double the base speed at which fish swim to the bobber. Your talent bonus' are applied afterwards.", null, new ConfigurationManagerAttributes {Order = 5}));
            ModifyFishSpawnLimit = Config.Bind("Fish", "Modify Fish Spawn Limit", true, new ConfigDescription("Modify the maximum number of fish that can spawn.", null, new ConfigurationManagerAttributes {Order = 4}));
            FishSpawnLimit = Config.Bind("Fish", "Fish Spawn Limit", 1500, new ConfigDescription("Adjust the maximum number of fish that can spawn.", new AcceptableValueRange<int>(1, 1500), new ConfigurationManagerAttributes {Order = 3}));
            ModifyFishSpawnMultiplier = Config.Bind("Fish", "Modify Fish Spawn Multiplier", true, new ConfigDescription("Modify the number of fish that spawn.", null, new ConfigurationManagerAttributes {Order = 2}));
            FishSpawnMultiplier = Config.Bind("Fish", "Fish Spawn Multiplier", 1500, new ConfigDescription("Adjust the number of fish that spawn.", new AcceptableValueRange<int>(1, 1500), new ConfigurationManagerAttributes {Order = 1}));


            //Fishing Rod section
            AutoReel = Config.Bind("Fishing Rod", "Auto Reel", true, new ConfigDescription("Automatically reel in fish when they bite.", null, new ConfigurationManagerAttributes {Order = 5}));
            InstantAutoReel = Config.Bind("Fishing Rod", "Instant Auto Reel", true, new ConfigDescription("Reel in fish instantly when they bite.", null, new ConfigurationManagerAttributes {Order = 4}));
            EnhanceBaseCastLength = Config.Bind("Fishing Rod", "Enhance Base Cast Length", true, new ConfigDescription("Enhances the base casting length of the fishing rod. Your talent bonus' are applied afterwards.", null, new ConfigurationManagerAttributes {Order = 3}));
            ModifyFishingRodCastSpeed = Config.Bind("Fishing Rod", "Modify Fishing Rod Cast Speed", true, new ConfigDescription("Modify the base casting speed of the fishing rod. Your talent bonus' are applied afterwards.", null, new ConfigurationManagerAttributes {Order = 2}));
            FishingRodCastSpeed = Config.Bind("Fishing Rod", "Fishing Rod Cast Speed", 5, new ConfigDescription("Adjust the base casting speed of the fishing rod. Your talent bonus' are applied afterwards.", new AcceptableValueRange<int>(1, 10), new ConfigurationManagerAttributes {Order = 1}));


            //Mini-Game section
            SkipFishingMiniGame = Config.Bind("Mini-Game", "Skip Fishing Mini Game", true, new ConfigDescription("Skip the fishing mini-game entirely.", null, new ConfigurationManagerAttributes {Order = 5}));
            ModifyMiniGameSpeed = Config.Bind("Mini-Game", "Modify Mini Game Speed", true, new ConfigDescription("Modify the speed of the fishing mini-game.", null, new ConfigurationManagerAttributes {Order = 4}));
            MiniGameMaxSpeed = Config.Bind("Mini-Game", "Mini Game Max Speed", 0.1f, new ConfigDescription("Adjust the maximum speed of the fishing mini-game.", new AcceptableValueRange<float>(0.1f, 1f), new ConfigurationManagerAttributes {Order = 3}));
            ModifyMiniGameWinAreaMultiplier = Config.Bind("Mini-Game", "Modify Mini Game Win Area Multiplier", true, new ConfigDescription("Modify the size of the winning area in the fishing mini-game.", null, new ConfigurationManagerAttributes {Order = 2}));
            MiniGameWinAreaMultiplier = Config.Bind("Mini-Game", "Mini Game Win Area Multiplier", 20f, new ConfigDescription("Adjust the size of the winning area in the fishing mini-game.", new AcceptableValueRange<float>(1, 20), new ConfigurationManagerAttributes {Order = 1}));


            //Miscellaneous section
            DisableCaughtFishWindow = Config.Bind("Miscellaneous", "Disable Caught Fish Window", true, new ConfigDescription("Disable the window that displays information about caught fish.", null, new ConfigurationManagerAttributes {Order = 2}));
            Debug = Config.Bind("Miscellaneous", "Debug", false, new ConfigDescription("Enable debug for logging.", null, new ConfigurationManagerAttributes {Order = 1}));

            Config.Bind("Miscellaneous", "Reset to Recommended", true, new ConfigDescription("Set the mod to p1xel8ted's recommended settings.", null, new ConfigurationManagerAttributes {CustomDrawer = RecommendedButtonDrawer}));

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogWarning($"Plugin {PluginName} is loaded!");
        }

        private static bool _showConfirmationDialog = false;

        private static void DisplayConfirmationDialog()
        {
            GUILayout.Label("Are you sure you want to reset to default settings?");

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
                {
                    RecommendedSettingsAction();
                }

                if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
                {
                    _showConfirmationDialog = false;
                }
            }
            GUILayout.EndHorizontal();
        }

        private static void RecommendedButtonDrawer(ConfigEntryBase entry)
        {
            if (_showConfirmationDialog)
            {
                DisplayConfirmationDialog();
            }
            else
            {
                var button = GUILayout.Button("Recommended Settings", GUILayout.ExpandWidth(true));
                if (!button) return;
                
                RecommendedSettingsAction();
            }
        }

        private static void RecommendedSettingsAction()
        {
            _showConfirmationDialog = true;
            //Bobber section
            DoubleBaseBobberAttractionRadius.Value = true;
            InstantAttraction.Value = true;

            //Fish section
            NoMoreNibbles.Value = true;
            DoubleBaseFishSwimSpeed.Value = true;
            ModifyFishSpawnLimit.Value = true;
            FishSpawnLimit.Value = 1500;
            ModifyFishSpawnMultiplier.Value = true;
            FishSpawnMultiplier.Value = 1500;

            //Fishing Rod section
            AutoReel.Value = true;
            InstantAutoReel.Value = true;
            EnhanceBaseCastLength.Value = true;
            ModifyFishingRodCastSpeed.Value = true;
            FishingRodCastSpeed.Value = 5;

            //Mini-Game section
            SkipFishingMiniGame.Value = true;
            ModifyMiniGameSpeed.Value = true;
            MiniGameMaxSpeed.Value = 0.1f;
            ModifyMiniGameWinAreaMultiplier.Value = true;
            MiniGameWinAreaMultiplier.Value = 20f;

            //Miscellaneous section
            DisableCaughtFishWindow.Value = true;
            Debug.Value = false;
        }

        private void OnDestroy()
        {
            LOG.LogError($"{PluginName} has been destroyed!");
        }

        private void OnDisable()
        {
            LOG.LogError($"{PluginName} has been disabled!");
        }
    }
}