using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace KeepAlive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.keepalive";
    private const string PluginName = "KeepAlive";
    private const string PluginVersion = "0.0.3";
    internal static ManualLogSource LOG { get; set; }

    internal static string GetGameObjectPath(GameObject obj)
    {
        var path = obj.name;
        var parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
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