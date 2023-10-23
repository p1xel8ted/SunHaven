using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace MuseumSellPrice;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "mimi.sunhaven.museum_sell_price";
    private const string PluginName = "Museum Sell Price";
    private const string PluginVersion = "0.0.4";

    internal static ConfigEntry<bool> Enabled { get; private set; }
    internal static ConfigEntry<float> Multiplier { get; private set; }
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        LOG = new ManualLogSource("Museum Sell Price");
        BepInEx.Logging.Logger.Sources.Add(LOG);
        Enabled = Config.Bind("General", "Enabled", true, "Set to false to disable this mod.");
        Multiplier = Config.Bind("General", "Multiplier", 100f, "A bass value that will be used to determine how much to multiply the value of USELESS museum only items (default 100 to me seems like the minimum to make someone care to not throw away something like an 'Ancient Sun Haven Sword')");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }
}