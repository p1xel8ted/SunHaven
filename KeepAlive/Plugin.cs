using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace KeepAlive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.keepalive";
    private const string PluginName = "Keep Alive";
    private const string PluginVersion = "0.0.4";
    internal static ManualLogSource LOG { get; private set; }
    
    private void Awake()
    {
        LOG = new ManualLogSource("Keep Alive");
        BepInEx.Logging.Logger.Sources.Add(LOG);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
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