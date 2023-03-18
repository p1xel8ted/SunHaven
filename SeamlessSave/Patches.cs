using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wish;

namespace SeamlessSave;

[HarmonyPatch]
public partial class Plugin
{
    //the game unloads objects when quitting to menu, and BepInEx gets caught up in it, which kills any mods that use BepInEx. This fixes that.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scene), nameof(Scene.GetRootGameObjects), new Type[] { })]
    public static void Scene_GetRootGameObjects(ref GameObject[] __result)
    {
        var newList = __result.ToList();
        newList.RemoveAll(x => x.name.Contains("BepInEx"));
        __result = newList.ToArray();
    }


	


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.SaveGame))]
    public static void GameSave_SaveGame()
    {
        if (Player.Instance == null) return;
        var saveLoc = new SaveLocation
        {
            location = Player.Instance.ExactPosition,
            scene = SceneManager.GetActiveScene().name
        };
        var savePath = Path.Combine(Application.persistentDataPath, GameSave.characterFolder, GameSave.Instance.CurrentSave.characterData.characterName + ".sloc");
        var json = JsonUtility.ToJson(saveLoc, true);
        File.WriteAllText(savePath, json);
        _log.LogWarning($"Saved player location ({saveLoc.location}) for character {GameSave.Instance.CurrentSave.characterData.characterName}, at {savePath}.");
    }


    private static SaveLocation _saveLocation;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.UpdateQuestItems))]
    public static void QuestManager_UpdateQuestItems()
    {
        var savePath = Path.Combine(Application.persistentDataPath, GameSave.characterFolder, GameSave.Instance.CurrentSave.characterData.characterName + ".sloc");

        _saveLocation = JsonUtility.FromJson<SaveLocation>(File.ReadAllText(savePath));

        _log.LogWarning($"Loaded player location ({_saveLocation.location}) for character {GameSave.Instance.CurrentSave.characterData.characterName}, at {savePath}.");
        
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadScene(_saveLocation.scene);
       
    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name != _saveLocation.scene)
        {
            Player.Instance.SetPosition(_saveLocation.location); 
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }
    }
}