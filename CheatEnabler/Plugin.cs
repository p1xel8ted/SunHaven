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
        private const string PluginVersion = "0.1.6";
        private static ManualLogSource LOG { get; set; }
        private static CheatCs CheatCsInstance { get; set; }

        private void Awake()
        {
            LOG = new ManualLogSource("Cheat Enabler");
            BepInEx.Logging.Logger.Sources.Add(LOG);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
            CheatCsInstance = gameObject.AddComponent<CheatCs>();
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(CheatCsInstance);
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