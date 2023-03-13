using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace UIScales
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private Harmony _hi;
        private static ConfigEntry<bool> _modEnabled;
        private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
        private const string PluginName = "UIScales";
        private const string PluginVersion = "0.1.3";
        

        internal static ConfigEntry<bool> UIModEnabled;
        internal static ConfigEntry<bool> ZoomModEnabled;
        internal static ConfigEntry<float> MainMenuUiScale;
        internal static ConfigEntry<float> InGameUiScale;
        internal static ConfigEntry<float> ZoomLevel;
        internal static ManualLogSource LOG;
        
        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);
            
            _modEnabled = Config.Bind("General", "ModEnabled", true, "Enable/disable this mod.");
            UIModEnabled = Config.Bind("General", "UiModEnabled", true, "Enable/disable UI adjustment.");
            ZoomModEnabled = Config.Bind("General", "ZoomModEnabled", true, "Enable/disable zoom adjustment.");
            InGameUiScale = Config.Bind<float>("Scale", "GameUIScale", 3, "UI scale while in game.");
            MainMenuUiScale = Config.Bind<float>("Scale", "MenuUIScale", 2, "UI scale while at the main menu.");
            ZoomLevel = Config.Bind<float>("Scale", "ZoomLevel", 2, "Zoom level while in game.");

            LOG.LogWarning($"Plugin {PluginName} is loaded!");
        }
        
        public void OnEnable()
        {
            if (_modEnabled.Value)
            {
                _hi = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            }
        }
        
        public void OnDisable()
        {
            _hi?.UnpatchSelf();
        }
        
        public void OnDestroy()
        {
            _hi?.UnpatchSelf();
        }
    }
}