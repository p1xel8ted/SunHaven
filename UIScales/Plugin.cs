using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace UIScales
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
        private const string PluginName = "UIScales";
        private const string PluginVersion = "0.1.5";

        public static ConfigEntry<float> MainMenuUiScale;
        public static ConfigEntry<float> InGameUiScale;
        public static ConfigEntry<float> ZoomLevel;
        public static ConfigEntry<float> CheatConsoleScale;
        public static ConfigEntry<bool> EnableNotifications;
        
        public static ConfigEntry<bool> Debug;
        public ConfigEntry<KeyboardShortcut> UIKeyboardShortcutIncrease { get; set; }
        public ConfigEntry<KeyboardShortcut> UIKeyboardShortcutDecrease { get; set; }
        public ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutIncrease { get; set; }
        public ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutDecrease { get; set; }
        internal static ManualLogSource LOG { get; private set; }

        internal static bool ZoomNeedsUpdating = true;

        internal static CanvasScaler UICanvas { get; set; }
        internal static CanvasScaler SecondUICanvas { get; set; }
        public static CanvasScaler QuantumCanvas { get; set; }
        public static CanvasScaler MainMenuCanvas { get; set; }

        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);

            UIKeyboardShortcutIncrease = Config.Bind("Keyboard Shortcuts", "UI Scale Increase", new KeyboardShortcut(KeyCode.Keypad8, KeyCode.LeftControl));
            UIKeyboardShortcutDecrease = Config.Bind("Keyboard Shortcuts", "UI Scale Decrease", new KeyboardShortcut(KeyCode.Keypad2, KeyCode.LeftControl));

            ZoomKeyboardShortcutIncrease = Config.Bind("Keyboard Shortcuts", "Zoom Level Increase", new KeyboardShortcut(KeyCode.Keypad8));
            ZoomKeyboardShortcutDecrease = Config.Bind("Keyboard Shortcuts", "Zoom Level Decrease", new KeyboardShortcut(KeyCode.Keypad2));

            Debug = Config.Bind("Debug", "Debug", false, "Enable debug logging. Nothing really useful to the player, but useful for me to debug issues.");

            EnableNotifications = Config.Bind("Notifications", "Enable Notifications", true, "Enable notifications when changing UI scale or zoom.");
            
            CheatConsoleScale = Config.Bind<float>("Scale", "Cheat Console Scale", 3, new ConfigDescription("Cheat console UI scale while in game.", new AcceptableValueRange<float>(0.5f, 10f)));
            if (CheatConsoleScale.Value < 0.5)
            {
                CheatConsoleScale.Value = 0.5f;
            }

            InGameUiScale = Config.Bind<float>("Scale", "Game UI Scale", 3, new ConfigDescription("UI scale while in game.", new AcceptableValueRange<float>(0.5f, 10f)));
            if (InGameUiScale.Value < 0.5)
            {
                InGameUiScale.Value = 0.5f;
            }


            MainMenuUiScale = Config.Bind<float>("Scale", "Menu UI Scale", 2, new ConfigDescription("UI scale while at the main menu.", new AcceptableValueRange<float>(0.5f, 10f)));
            if (MainMenuUiScale.Value < 0.5)
            {
                MainMenuUiScale.Value = 0.5f;
            }

            ZoomLevel = Config.Bind<float>("Scale", "Zoom Level", 2, new ConfigDescription("Zoom level while in game.", new AcceptableValueRange<float>(0.5f, 10f)));
            if (ZoomLevel.Value < 0.5)
            {
                ZoomLevel.Value = 0.5f;
                ZoomNeedsUpdating = true;
            }

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
}