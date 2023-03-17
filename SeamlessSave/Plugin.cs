using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SeamlessSave
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.seamlesssave";
        private const string PluginName = "Seamless Save";
        private const string PluginVersion = "0.0.1";
        private static ManualLogSource _log;

        private void Awake()
        {
            _log = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(_log);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            _log.LogWarning($"Plugin {PluginName} is loaded!");
        }

        private void OnDestroy()
        {
            _log.LogError("I've been destroyed!");
        }

        private void OnDisable()
        {
            _log.LogError("I've been disabled!");
        }
    }
}