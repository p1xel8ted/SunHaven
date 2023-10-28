using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Wish;

namespace MoreScythesRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.morescythesredux";
    private const string PluginName = "More Scythes Redux";
    private const string PluginVersion = "0.1.0";
    public static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"{PluginName} plugin has loaded successfully.");
    }


}