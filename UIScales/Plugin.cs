using System;
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
        private const string PluginVersion = "0.1.3";

        internal static ConfigEntry<float> MainMenuUiScale;
        internal static ConfigEntry<float> InGameUiScale;
        internal static ConfigEntry<float> ZoomLevel;
        internal static ConfigEntry<float> CheatConsoleScale;
        internal static ManualLogSource LOG { get; private set; }
        
        internal static bool ZoomNeedsUpdating = true;

        internal static CanvasScaler UICanvas { get; set; }
        internal static CanvasScaler SecondUICanvas { get; set; }
        public static CanvasScaler QuantumCanvas { get; set; }
        public static CanvasScaler MainMenuCanvas { get; set; }
        public static GameObject CinematicBlackBars { get; set; }

        private void Awake()
        {
            LOG = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(LOG);

            CheatConsoleScale = Config.Bind<float>("Scale", "CheatConsoleScale", 3, "Cheat console UI scale while in game.");
            if (CheatConsoleScale.Value < 0.5)
            {
                CheatConsoleScale.Value = 0.5f;
          
            }

            InGameUiScale = Config.Bind<float>("Scale", "GameUIScale", 3, "UI scale while in game.");
            if (InGameUiScale.Value < 0.5)
            {
                InGameUiScale.Value = 0.5f;
          
            }
            
            MainMenuUiScale = Config.Bind<float>("Scale", "MenuUIScale", 2, "UI scale while at the main menu.");
            if (MainMenuUiScale.Value < 0.5)
            {
                MainMenuUiScale.Value = 0.5f;  
              
            }
            
            ZoomLevel = Config.Bind<float>("Scale", "ZoomLevel", 2, "Zoom level while in game.");
            if (ZoomLevel.Value < 0.5)
            {
                ZoomLevel.Value = 0.5f;  
                ZoomNeedsUpdating = true;
            }
            
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(),PluginGuid);
            LOG.LogWarning($"Plugin {PluginName} is loaded!");
        }
        
        private void OnDestroy()
        {
            LOG.LogError($"I've been destroyed!");
        }

        private void OnDisable()
        {
            LOG.LogError($"I've been disabled!");
        }
    }
}