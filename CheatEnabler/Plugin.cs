using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace CheatEnabler
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private Harmony _hi;
        private static ConfigEntry<bool> _modEnabled;
        private const string PluginGuid = "p1xel8ted.sunhaven.cheatenabler";
        private const string PluginName = "CheatEnabler";
        private const string PluginVersion = "0.1.0";
        private static ManualLogSource _log;
        
        private void Awake()
        {
            _log = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(_log);
            _modEnabled = Config.Bind("General", "ModEnabled", true, "Enable/disable this mod.");
            _hi = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            _log.LogWarning($"Plugin {PluginName} is loaded!");
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