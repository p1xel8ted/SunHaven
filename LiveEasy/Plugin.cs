using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace LiveEasy;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.liveeasy";
    private const string PluginName = "LiveEasy";
    private const string PluginVersion = "0.0.1";
    public ConfigEntry<KeyboardShortcut> SaveShortcut { get; set; }
    internal static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        SaveShortcut = Config.Bind("Keyboard Shortcuts", "Quick Save", new KeyboardShortcut(KeyCode.F5));
        
        LOG = new ManualLogSource("Log");
        BepInEx.Logging.Logger.Sources.Add(LOG);

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