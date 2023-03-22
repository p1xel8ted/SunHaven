using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CheatEnabler
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.cheatenabler";
        private const string PluginName = "CheatEnabler";
        private const string PluginVersion = "0.1.3";
        internal static ManualLogSource LOG;

        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogWarning($"Plugin {PluginName} is loaded!");
            gameObject.AddComponent<QuantumConsoleManager>();
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