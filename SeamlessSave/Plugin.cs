using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Wish;

namespace SeamlessSave
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.seamlesssave";
        private const string PluginName = "Seamless Save";
        private const string PluginVersion = "0.0.1";
        private static ManualLogSource _log;


        public ConfigEntry<KeyboardShortcut> KeyboardShortcutSaveGame { get; set; }

        private void Awake()
        {
            KeyboardShortcutSaveGame = Config.Bind("Keyboard Shortcuts", "Save Game", new KeyboardShortcut(KeyCode.F5));
            _log = new ManualLogSource("Log");
            BepInEx.Logging.Logger.Sources.Add(_log);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            _log.LogWarning($"Plugin {PluginName} is loaded!");
        }

        private void Update()
        {
            if (KeyboardShortcutSaveGame.Value.IsUp())
            {
                if (Player.Instance == null) return;
                var saveLoc = new SaveLocation
                {
                    location = Player.Instance.ExactPosition,
                    scene = ScenePortalManager.ActiveSceneName.Trim().ToLower()
                };
                
                var savePath = Path.Combine(Application.persistentDataPath, GameSave.characterFolder, GameSave.Instance.CurrentSave.characterData.characterName + ".sloc");
                var json = JsonUtility.ToJson(saveLoc, true);
                File.WriteAllText(savePath, json);
                _log.LogWarning($"Saved player location ({saveLoc.location}) for character {GameSave.Instance.CurrentSave.characterData.characterName}, at {savePath}.");
            }
        }

        private void OnDestroy()
        {
            _log.LogError("I've been destroyed!");
        }

        private void OnDisable()
        {
            _log.LogError("I've been disabled!");
        }
    }
}