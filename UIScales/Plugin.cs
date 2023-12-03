using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wish;

namespace UIScales
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
        private const string PluginName = "UI Scales";
        private const string PluginVersion = "0.1.8";

        public static ConfigEntry<float> MainMenuUiScale;
        public static ConfigEntry<float> InGameUiScale;
        public static ConfigEntry<float> ZoomLevel;
        public static ConfigEntry<float> CheatConsoleScale;
        private static ConfigEntry<bool> _enableNotifications;

        public static ConfigEntry<bool> Debug;
        public static ConfigEntry<KeyboardShortcut> UIKeyboardShortcutIncrease { get; set; }
        public static ConfigEntry<KeyboardShortcut> UIKeyboardShortcutDecrease { get; set; }
        public static ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutIncrease { get; set; }
        public static ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutDecrease { get; set; }
        internal static ManualLogSource LOG { get; private set; }

        [CanBeNull] internal static CanvasScaler UIOneCanvas { get; set; }
        [CanBeNull] internal static CanvasScaler UITwoCanvas { get; set; }
        [CanBeNull] public static CanvasScaler QuantumCanvas { get; set; }
        [CanBeNull] public static CanvasScaler MainMenuCanvas { get; set; }

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            LOG = new ManualLogSource(PluginName);
            BepInEx.Logging.Logger.Sources.Add(LOG);

            UIKeyboardShortcutIncrease = Config.Bind("Keyboard Shortcuts", "UI Scale Increase", new KeyboardShortcut(KeyCode.Keypad8, KeyCode.LeftControl));
            UIKeyboardShortcutDecrease = Config.Bind("Keyboard Shortcuts", "UI Scale Decrease", new KeyboardShortcut(KeyCode.Keypad2, KeyCode.LeftControl));

            ZoomKeyboardShortcutIncrease = Config.Bind("Keyboard Shortcuts", "Zoom Level Increase", new KeyboardShortcut(KeyCode.Keypad8));
            ZoomKeyboardShortcutDecrease = Config.Bind("Keyboard Shortcuts", "Zoom Level Decrease", new KeyboardShortcut(KeyCode.Keypad2));

            Debug = Config.Bind("Debug", "Debug", false, "Enable debug logging. Nothing really useful to the player, but useful for me to debug issues.");

            _enableNotifications = Config.Bind("Notifications", "Enable Notifications", true, "Enable notifications when changing UI scale or zoom.");

            CheatConsoleScale = Config.Bind<float>("Scale", "Cheat Console Scale", 3, new ConfigDescription("Cheat console UI scale while in game.", new AcceptableValueRange<float>(0.5f, 10f)));
            CheatConsoleScale.Value = Mathf.Max(CheatConsoleScale.Value, 0.5f);
            CheatConsoleScale.SettingChanged += (_, _) => { ConfigureCanvasScaler(QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, CheatConsoleScale.Value); };

            InGameUiScale = Config.Bind<float>("Scale", "Game UI Scale", 3, new ConfigDescription("UI scale while in game.", new AcceptableValueRange<float>(0.5f, 10f)));
            InGameUiScale.Value = Mathf.Max(InGameUiScale.Value, 0.5f);
            InGameUiScale.SettingChanged += (_, _) =>
            {
                ConfigureCanvasScaler(UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
                ConfigureCanvasScaler(UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
            };

            MainMenuUiScale = Config.Bind<float>("Scale", "Main Menu UI Scale", 2, new ConfigDescription("UI scale while at the main menu.", new AcceptableValueRange<float>(0.5f, 10f)));
            MainMenuUiScale.Value = Mathf.Max(MainMenuUiScale.Value, 0.5f);
            MainMenuUiScale.SettingChanged += (_, _) => { ConfigureCanvasScaler(MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, MainMenuUiScale.Value); };

            ZoomLevel = Config.Bind<float>("Scale", "Zoom Level", 2, new ConfigDescription("Zoom level while in game.", new AcceptableValueRange<float>(0.5f, 10f)));
            ZoomLevel.Value = Mathf.Max(ZoomLevel.Value, 0.5f);
            ZoomLevel.SettingChanged += (_, _) =>
            {
                if (Player.Instance is not null)
                {
                    Player.Instance.OverrideCameraZoomLevel = false;
                    Player.Instance.SetZoom(ZoomLevel.Value, true);
                }
            };

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
        }

        private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
           // LOG.LogWarning($"SceneManagerOnSceneLoaded: {arg0.name}");
           // var isMainMenu = arg0.name.Equals("MainMenu", StringComparison.InvariantCultureIgnoreCase);
            // UpdateUiScale(isMainMenu);
            // UpdateZoomLevel();
            UIOneCanvas = GameObject.Find("Manager/UI")?.GetComponent<CanvasScaler>();
            UITwoCanvas = GameObject.Find("Player(Clone)/UI")?.GetComponent<CanvasScaler>();
            QuantumCanvas = GameObject.Find("SharedManager/Quantum Console")?.GetComponent<CanvasScaler>();
            MainMenuCanvas = GameObject.Find("Canvas")?.GetComponent<CanvasScaler>();
            UpdateCanvasScaleFactors();
        }

        internal static void ConfigureCanvasScaler(CanvasScaler canvasScaler, CanvasScaler.ScaleMode scaleMode, float scaleFactor)
        {
            if (canvasScaler is null)
            {
                // Plugin.LOG.LogWarning($"ConfigureCanvasScaler: canvasScaler is null!");
                return;
            }

            canvasScaler.uiScaleMode = scaleMode;
            canvasScaler.scaleFactor = scaleFactor;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            LOG.LogError($"{PluginName} has been destroyed!");
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            LOG.LogError($"{PluginName} has been disabled!");
        }

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
    }
}