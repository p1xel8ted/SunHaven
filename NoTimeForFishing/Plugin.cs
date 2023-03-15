using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace NoTimeForFishing
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public  class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.notimeforfishing";
        private const string PluginName = "No Time For Fishing!";
        private const string PluginVersion = "0.0.1";

        public static ConfigEntry<bool> DisableCaughtFishWindow;
        public static ConfigEntry<bool> SkipFishingMiniGame;

        private static ManualLogSource LOG { get; set; }

        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);

            SkipFishingMiniGame = Config.Bind("Fishing", "Skip Fishing Mini Game", true, "Skip the fishing mini game.");
            DisableCaughtFishWindow = Config.Bind("Caught Fish Window", "Disable Caught Fish Window", true, "Disable the caught fish window.");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogWarning($"Plugin {PluginName} is loaded!");
        }

        private void OnDestroy()
        {
            LOG.LogError($"I've been destroyed!");
        }

        private void OnDisable()
        {
            LOG.LogError($"I've been disabled!");
        }
    }
}