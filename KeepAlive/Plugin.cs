using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace KeepAlive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.keepalive";
    private const string PluginName = "KeepAlive";
    private const string PluginVersion = "0.0.2";
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
      
        LOG = new ManualLogSource("Log");
        BepInEx.Logging.Logger.Sources.Add(LOG);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogWarning($"Plugin {PluginName} is loaded!");
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