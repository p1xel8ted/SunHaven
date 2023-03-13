using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;


namespace UIScales
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
        private const string PluginName = "UIScales";
        private const string PluginVersion = "0.1.3";

        private static ConfigEntry<bool> _uiModEnabled;
        private static ConfigEntry<bool> _zoomModEnabled;
        private static ConfigEntry<float> _mainMenuUiScale;
        private static ConfigEntry<float> _inGameUiScale;
        private static ConfigEntry<float> _zoomLevel;
        private static ManualLogSource _log;
        
        private void Awake()
        {
            _log = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(_log);
            
            _uiModEnabled = Config.Bind("General", "UiModEnabled", true, "Enable/disable UI adjustment.");
            _zoomModEnabled = Config.Bind("General", "ZoomModEnabled", true, "Enable/disable zoom adjustment.");
            _inGameUiScale = Config.Bind<float>("Scale", "GameUIScale", 3, "UI scale while in game.");
            _mainMenuUiScale = Config.Bind<float>("Scale", "MenuUIScale", 2, "UI scale while at the main menu.");
            _zoomLevel = Config.Bind<float>("Scale", "ZoomLevel", 2, "Zoom level while in game.");

            _log.LogWarning($"Plugin {PluginName} is loaded!");
        }
    }
}