using System.IO;
using HarmonyLib;
using UnityEngine;
using Wish;

namespace SeamlessSave;

[HarmonyPatch]
public partial class Plugin
{
    
    private static SaveLocation _saveLocation;
    private static bool _loaded;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsOwner))]
    public static void Player_InitializeAsOwner()
    {
        var savePath = Path.Combine(Application.persistentDataPath, GameSave.characterFolder, GameSave.Instance.CurrentSave.characterData.characterName + ".sloc");
        if (File.Exists(savePath))
        {
            _saveLocation = JsonUtility.FromJson<SaveLocation>(File.ReadAllText(savePath));
            _log.LogWarning($"Loaded player location ({_saveLocation.location}) for character {GameSave.Instance.CurrentSave.characterData.characterName}, at {savePath}.");
            // if (!_loaded)
            // {
                //SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(_saveLocation.location, _saveLocation.scene);
                SingletonBehaviour<ScenePortalManager>.Instance.SceneChangeRoutine(_saveLocation.location, _saveLocation.scene);
                // }
        }
    }
}