using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AutoTool;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.autotool";
    private const string PluginName = "AutoTool";
    private const string PluginVersion = "0.0.1";
    internal static ManualLogSource LOG { get; set; }

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