using QFSW.QC;
using UnityEngine;
using Wish;

namespace CheatEnabler {
    [CommandPrefix("/")]
    public class CheatCs : MonoBehaviour
    {
        
        
        [Command()]
        private void savegame()
        {
            SingletonBehaviour<GameSave>.Instance.SaveGame(true);
            SingletonBehaviour<NotificationStack>.Instance.SendNotification("Game Saved!");
        }
        
        // //[FormerlySerializedAs("_quantumConsole")] [SerializeField] private QuantumConsole quantumConsole;
        //
        // [FormerlySerializedAs("god_mode")] public bool godMode;
        //
        // [FormerlySerializedAs("no_clip")] public bool noClip;
        //
        // [Command]
        // private void Adddevitems()
        // {
        //     Player.Instance.Inventory.AddItem(0x7533, 1, true);
        //     Player.Instance.Inventory.AddItem(0x7534, 1, true);
        //     Player.Instance.Inventory.AddItem(0x7535, 1, true);
        //     Player.Instance.Inventory.AddItem(0x7536, 1, true);
        //     Player.Instance.Inventory.AddItem(0x7537, 1, true);
        // }
        //
        // [Command]
        // private void Addexp(string profession, float amount)
        // {
        //     var professionType = (ProfessionType) Enum.Parse(typeof(ProfessionType), profession, true);
        //     Player.Instance.AddEXP(professionType, amount);
        // }
        //
        // [Command]
        // private void Additem(string item, int amount = 1)
        // {
        //     var d = ItemDatabase.GetID(item);
        //     Player.Instance.Inventory.AddItem((d != -1 ? d : int.Parse(item)), amount, true);
        // }
        //
        // [Command]
        // private void Addmoney(int amount)
        // {
        //     Player.Instance.AddMoney(amount);
        // }
        //
        // [Command]
        // public void Addpermenentstatbonus(string stat, float amount)
        // {
        //     GameSave.CurrentCharacter.AddStatBonus((StatType) Enum.Parse(typeof(StatType), stat.RemoveWhitespace().ToLower(), true), amount);
        // }
        //
        // [Command]
        // private void Addrangeofitems(string item, int amount, int range = 10)
        // {
        //     var d = ItemDatabase.GetID(item);
        //     for (var i = 0; i < range; i++)
        //     {
        //         Player.Instance.Inventory.AddItem((d != -1 ? d : int.Parse(item)) + i, amount, true);
        //     }
        // }
        //
        // [Command]
        // private void Addstat(string stat, float amount)
        // {
        //     Player.Instance.Stats.Add((StatType) Enum.Parse(typeof(StatType), stat.RemoveWhitespace().ToLower(), true), amount);
        // }
        //
        // [Command]
        // private void Addtime(float time)
        // {
        //     SingletonBehaviour<DayCycle>.Instance.AddTime(time);
        // }
        //
        // [Command]
        // private void Allitems()
        // {
        //     ItemDatabase.DebugItemList();
        // }
        //
        // private void Awake()
        // {
        //     Plugin.LOG.LogWarning("CheatEnabler QC Manager is loaded!");
        //     
        // }
        //
        // private IEnumerator DelayInitializeScene()
        // {
        //     yield return null;
        //     SingletonBehaviour<ScenePortalManager>.Instance.InitializeScene();
        //     PlaySettingsManager.PlaySettings.skipEndOfDayScreen = false;
        // }
        //
        // [Command]
        // private void Despawnpet()
        // {
        //     SingletonBehaviour<PetManager>.Instance.DespawnPet(Player.Instance);
        // }
        //
        // [Command]
        // private void Enabledaycycle(bool enable)
        // {
        //     PlaySettingsManager.PlaySettings.enableDayCycle = enable;
        // }
        //
        // [Command]
        // private void Getrelationships()
        // {
        //     foreach (var relationship in SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.relationships)
        //     {
        //         Debug.Log(string.Concat(relationship.Key, ": ", relationship.Value));
        //     }
        // }
        //
        // [Command]
        // private void Getstat(string stat)
        // {
        //     Debug.Log(Player.Instance.GetStat((StatType) Enum.Parse(typeof(StatType), stat.RemoveWhitespace().ToLower(), true)));
        // }
        //
        // [Command]
        // public void Godmode(bool active)
        // {
        //     Noclip(active);
        //     Setstat("movespeed", active ? 4 : 1);
        //     Setstat("jump", (active ? 1.75f : 1f));
        //     godMode = active;
        // }
        //
        // [Command]
        // private void MarryNpc(string npc)
        // {
        //     var progressStringCharacter = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
        //     if (!progressStringCharacter.IsNullOrWhitespace())
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", "");
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Married", false);
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("MarriedTo", progressStringCharacter), false);
        //         GameSave.CurrentCharacter.Relationships[progressStringCharacter] = 40f;
        //         SingletonBehaviour<NPCManager>.Instance.GetRealNPC(progressStringCharacter).GenerateCycle();
        //     }
        //
        //     if (SingletonBehaviour<NPCManager>.Instance._npcs.TryGetValue(npc, out var nPcai))
        //     {
        //         Setrelationship(npc, 100);
        //         nPcai.MarryPlayer();
        //     }
        // }
        //
        // [Command]
        // public void Noclip(bool active)
        // {
        //     Player.Instance.GetComponent<Collider2D>().isTrigger = active;
        //     noClip = active;
        // }
        //
        // // internal void QuantumConsoleOnOnActivate()
        // // {
        // //     PlayerInput.DisableInput("console");
        // //     PlayerInput.AllowArrowKeys = false;
        // // }
        // //
        // // internal void QuantumConsoleOnOnDeactivate()
        // // {
        // //     PlayerInput.EnableInput("console");
        // //     PlayerInput.AllowArrowKeys = true;
        // // }

        //
        // [Command]
        // private void Resetalldecoration()
        // {
        //     SingletonBehaviour<GameSave>.Instance.ResetDecorations();
        // }
        //
        // [Command]
        // private void Resetanimals()
        // {
        //     SingletonBehaviour<NPCManager>.Instance.DeleteAnimals();
        // }
        //
        // [Command]
        // private void Resetcharacterprogress(string progress)
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(progress, false);
        // }
        //
        // [Command]
        // private void Resetfarminginfo()
        // {
        //     SingletonBehaviour<GameSave>.Instance.ResetFarmingInfo();
        // }
        //
        // [Command]
        // private void Resetfoodstats()
        // {
        //     Player.Instance.SetBaseStats();
        //     SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.foodStats = new Dictionary<int, int>();
        // }
        //
        // [Command]
        // private void Resethelpnotifications(string mailName)
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("UsedWateringCan", false);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("UsedHoe", false);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("FilledUpWateringCan", false);
        //     var playerFarmQuestManager = FindObjectOfType<PlayerFarmQuestManager>();
        //     if (playerFarmQuestManager == null)
        //     {
        //         return;
        //     }
        //
        //     playerFarmQuestManager.SendStartingNotifications();
        // }
        //
        // [Command]
        // private void Resetinventory()
        // {
        //     Player.Instance.Inventory.ClearInventory();
        // }
        //
        // [Command]
        // private void Resetmail()
        // {
        //     var allMail = MailManager.AllMail;
        //     foreach (var mailAsset in allMail)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(mailAsset.name, false);
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Mail.Clear();
        // }
        //
        // [Command]
        // public void Resetmoney()
        // {
        //     SingletonBehaviour<GameSave>.Instance.CurrentWorld.coins = 0;
        // }
        //
        // [Command]
        // public void Resetpermanentstatbonuses()
        // {
        //     GameSave.CurrentCharacter.StatBonuses.Clear();
        // }
        //
        // [Command]
        // private void Resetprogress()
        // {
        //     SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.progress = new Dictionary<int, byte[]>();
        //     SingletonBehaviour<GameSave>.Instance.CurrentSave.worldData.progress = new Dictionary<int, byte[]>();
        //     SingletonBehaviour<ScenePortalManager>.Instance.InitializeScene();
        // }
        //
        // [Command]
        // private void Resetquests()
        // {
        //     try
        //     {
        //         GameSave.CurrentCharacter.questData = new Dictionary<string, Dictionary<string, byte[]>>();
        //         var allQuests = QuestManager.AllQuests;
        //         int i;
        //         for (i = 0; i < allQuests.Length; i++)
        //         {
        //             var questAsset = allQuests[i];
        //             if (SingletonBehaviour<GameSave>.Instance.TryGetProgressBoolCharacter(questAsset.name, out _))
        //             {
        //                 SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(questAsset.name, false);
        //             }
        //         }
        //
        //         foreach (var list in Player.Instance.QuestList.questLog.Keys.ToList())
        //         {
        //             Player.Instance.QuestList.AbandonQuest(list);
        //         }
        //
        //         SingletonBehaviour<QuestManager>.Instance.Initialize();
        //         var nPcaiArray = FindObjectsOfType<NPCAI>();
        //         for (i = 0; i < nPcaiArray.Length; i++)
        //         {
        //             var nPcai = nPcaiArray[i];
        //             nPcai.GenerateQuestAndCycle();
        //             nPcai.GetDialogueTree();
        //         }
        //     }
        //     catch (Exception exception)
        //     {
        //         Debug.Log(exception);
        //         throw;
        //     }
        // }
        //
        // [Command]
        // private void Resetrelationships()
        // {
        //     SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.relationships = new Dictionary<string, float>();
        // }
        //
        // [Command]
        // private void Resetskills()
        // {
        //     var values = (ProfessionType[]) Enum.GetValues(typeof(ProfessionType));
        //     foreach (var nums in values)
        //     {
        //         Player.Instance.SetEXP(nums, 0f);
        //         GameSave.CurrentCharacter.professions[nums].nodes = new Dictionary<int, int>();
        //         SingletonBehaviour<GameSave>.Instance.SetProgressIntCharacter(string.Concat(nums, "SkillPoints"), 0);
        //     }
        // }
        //
        // [Command]
        // private void Resetworldprogress(string progress)
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(progress, false);
        // }
        //
        // [Command]
        // private void Sendmail(string mailName)
        // {
        //     Player.Instance.SendMail(SingletonBehaviour<MailManager>.Instance.GetMail(mailName), true);
        // }
        //
        // [Command]
        // private void Setcharacterprogress(string progress)
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(progress, true);
        // }
        //
        // [Command]
        // private void Setday(int day)
        // {
        //     SingletonBehaviour<DayCycle>.Instance.SetDay(day);
        //     StartCoroutine(DelayInitializeScene());
        // }
        //
        // [Command]
        // private void Setdaya()
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("DayA", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("DayB", false);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("Raining", false);
        //     SingletonBehaviour<NPCManager>.Instance.GenerateNPCPaths(false);
        // }
        //
        // [Command]
        // private void Setdayb()
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("DayA", false);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("DayB", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("Raining", false);
        //     SingletonBehaviour<NPCManager>.Instance.GenerateNPCPaths(false);
        // }
        //
        // [Command]
        // private void Setdayrain()
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("DayA", false);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("DayB", false);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("Raining", true);
        //     SingletonBehaviour<NPCManager>.Instance.GenerateNPCPaths(false);
        // }
        //
        // [Command]
        // private void Setdayspeed(int speed)
        // {
        //     var speed1 = Speed.x1;
        //     if (speed <= 1)
        //     {
        //         if (speed == 0)
        //         {
        //             speed1 = Speed.x0;
        //         }
        //         else if (speed == 1)
        //         {
        //             speed1 = Speed.x1;
        //         }
        //     }
        //     else if (speed == 10)
        //     {
        //         speed1 = Speed.x10;
        //     }
        //     else if (speed == 100)
        //     {
        //         speed1 = Speed.x100;
        //     }
        //     else if (speed == 0x3e8)
        //     {
        //         speed1 = Speed.x1000;
        //     }
        //
        //     PlaySettingsManager.PlaySettings.DayCycleSpeed = speed1;
        // }
        //
        // [Command]
        // private void Setexp(string profession, float amount)
        // {
        //     var professionType = (ProfessionType) Enum.Parse(typeof(ProfessionType), profession, true);
        //     Player.Instance.SetEXP(professionType, amount);
        // }
        //
        // [Command]
        // private void Sethealth(int health)
        // {
        //     if (health > Player.Instance.GetStat(StatType.Mana))
        //     {
        //         Setstat("health", health);
        //     }
        //
        //     Player.Instance.Heal(health - Player.Instance.Health);
        // }
        //
        // [Command]
        // private void Setmana(int mana)
        // {
        //     if (mana > Player.Instance.GetStat(StatType.Mana))
        //     {
        //         Setstat("mana", mana);
        //     }
        //
        //     Player.Instance.AddMana(mana - Player.Instance.Mana);
        // }
        //
        // [Command]
        // private void Setmaxfoodstats(int amount)
        // {
        //     try
        //     {
        //         Player.Instance.SetBaseStats();
        //         SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.foodStats = new Dictionary<int, int>();
        //         var itemDataArray = ItemDatabase.items;
        //         foreach (var itemDatum in itemDataArray)
        //         {
        //             if (itemDatum != null)
        //             {
        //                 var foodDatum = itemDatum as FoodData;
        //                 if (foodDatum != null)
        //                 {
        //                     GameSave.CurrentCharacter.foodStats[foodDatum.id] = amount;
        //                 }
        //             }
        //         }
        //
        //         Player.Instance.GetStatByNumberEaten();
        //     }
        //     catch (Exception exception)
        //     {
        //         Debug.Log(exception);
        //         throw;
        //     }
        // }
        //
        // [Command]
        // private void Setnpcquest(string npcName)
        // {
        //     SingletonBehaviour<NPCManager>.Instance.GetNPC(npcName).GenerateRandomQuest(true);
        // }
        //
        // [Command]
        // private void Setpreplaceddecorations()
        // {
        //     foreach (var list in SingletonBehaviour<GameManager>.Instance.objects.ToList())
        //     {
        //         SingletonBehaviour<GameManager>.Instance.DeleteObjectSubTile(list.Key);
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressIntWorld("MailboxesPlaced", 0);
        //     WorldController.SetPrePlacedDecorations();
        // }
        //
        // [Command]
        // private void Setrelationship(string npc, int amount)
        // {
        //     SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npc] = amount;
        // }
        //
        // [Command]
        // private void Setseason(string season)
        // {
        //     var season1 = (Season) Enum.Parse(typeof(Season), season, true);
        //     SingletonBehaviour<DayCycle>.Instance.SetSeason(season1);
        //     SingletonBehaviour<NPCManager>.Instance.GenerateNPCSeasonSprite();
        //     StartCoroutine(DelayInitializeScene());
        // }
        //
        // [Command]
        // public void Setstat(string stat, float amount)
        // {
        //     Player.Instance.Stats.Set((StatType) Enum.Parse(typeof(StatType), stat.RemoveWhitespace().ToLower(), true), amount);
        // }
        //
        // [Command]
        // private void Settime(float hour)
        // {
        //     var instance = SingletonBehaviour<DayCycle>.Instance;
        //     var time = SingletonBehaviour<DayCycle>.Instance.Time;
        //     var year = time.Year;
        //     time = SingletonBehaviour<DayCycle>.Instance.Time;
        //     var month = time.Month;
        //     time = SingletonBehaviour<DayCycle>.Instance.Time;
        //     instance.Time = new DateTime(year, month, time.Day, Mathf.Clamp((int) hour, 6, 23), (int) ((hour - (int) hour) * 60f), 0, DateTimeKind.Utc);
        //     var nPCManager = SingletonBehaviour<NPCManager>.Instance;
        //     time = SingletonBehaviour<DayCycle>.Instance.Time;
        //     var single = (float) time.Hour;
        //     time = SingletonBehaviour<DayCycle>.Instance.Time;
        //     nPCManager.CheckForStartingNPCPaths(single + time.Minute / 60f);
        // }
        //
        // [Command]
        // private void Setuiactive(bool active)
        // {
        //     var o = Wish.Utilities.FindObject(GameObject.Find("Player"), "ActionBar");
        //     if (o != null)
        //     {
        //         o.SetActive(active);
        //     }
        //
        //     var gameObject1 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "ActionBar");
        //     if (gameObject1 != null)
        //     {
        //         gameObject1.SetActive(active);
        //     }
        //
        //     var gameObject2 = Wish.Utilities.FindObject(GameObject.Find("Player"), "ExpBars");
        //     if (gameObject2 != null)
        //     {
        //         gameObject2.SetActive(active);
        //     }
        //
        //     var gameObject3 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "ExpBars");
        //     if (gameObject3 != null)
        //     {
        //         gameObject3.SetActive(active);
        //     }
        //
        //     var gameObject4 = Wish.Utilities.FindObject(GameObject.Find("Player"), "QuestTracking");
        //     if (gameObject4 != null)
        //     {
        //         gameObject4.SetActive(active);
        //     }
        //
        //     var gameObject5 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracking");
        //     if (gameObject5 != null)
        //     {
        //         gameObject5.SetActive(active);
        //     }
        //
        //     var gameObject6 = Wish.Utilities.FindObject(GameObject.Find("Player"), "QuestTracker");
        //     if (gameObject6 != null)
        //     {
        //         gameObject6.SetActive(active);
        //     }
        //
        //     var gameObject7 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracker");
        //     if (gameObject7 != null)
        //     {
        //         gameObject7.SetActive(active);
        //     }
        //
        //     var gameObject8 = Wish.Utilities.FindObject(GameObject.Find("Player"), "HelpNotifications");
        //     if (gameObject8 != null)
        //     {
        //         gameObject8.SetActive(active);
        //     }
        //
        //     var gameObject9 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "HelpNotifications");
        //     if (gameObject9 != null)
        //     {
        //         gameObject9.SetActive(active);
        //     }
        //
        //     var gameObject10 = Wish.Utilities.FindObject(GameObject.Find("Player"), "NotificationStack");
        //     if (gameObject10 != null)
        //     {
        //         gameObject10.SetActive(active);
        //     }
        //
        //     var gameObject11 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "NotificationStack");
        //     if (gameObject11 != null)
        //     {
        //         gameObject11.SetActive(active);
        //     }
        //
        //     var gameObject12 = Wish.Utilities.FindObject(GameObject.Find("Manager"), "UI");
        //     if (gameObject12 != null)
        //     {
        //         gameObject12.SetActive(active);
        //     }
        //
        //     var gameObject13 = GameObject.Find("QuestTrackerVisibilityToggle");
        //     if (gameObject13 == null)
        //     {
        //         return;
        //     }
        //
        //     gameObject13.SetActive(active);
        // }
        //
        // [Command]
        // private void Setuiactivebutactionbar(bool active)
        // {
        //     var o = Wish.Utilities.FindObject(GameObject.Find("Player"), "ExpBars");
        //     if (o != null)
        //     {
        //         o.SetActive(active);
        //     }
        //
        //     var gameObject1 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "ExpBars");
        //     if (gameObject1 != null)
        //     {
        //         gameObject1.SetActive(active);
        //     }
        //
        //     var gameObject2 = Wish.Utilities.FindObject(GameObject.Find("Player"), "QuestTracking");
        //     if (gameObject2 != null)
        //     {
        //         gameObject2.SetActive(active);
        //     }
        //
        //     var gameObject3 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracking");
        //     if (gameObject3 != null)
        //     {
        //         gameObject3.SetActive(active);
        //     }
        //
        //     var gameObject4 = Wish.Utilities.FindObject(GameObject.Find("Player"), "HelpNotifications");
        //     if (gameObject4 != null)
        //     {
        //         gameObject4.SetActive(active);
        //     }
        //
        //     var gameObject5 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "HelpNotifications");
        //     if (gameObject5 != null)
        //     {
        //         gameObject5.SetActive(active);
        //     }
        //
        //     var gameObject6 = Wish.Utilities.FindObject(GameObject.Find("Player"), "NotificationStack");
        //     if (gameObject6 != null)
        //     {
        //         gameObject6.SetActive(active);
        //     }
        //
        //     var gameObject7 = Wish.Utilities.FindObject(GameObject.Find("Player(Clone)"), "NotificationStack");
        //     if (gameObject7 != null)
        //     {
        //         gameObject7.SetActive(active);
        //     }
        //
        //     var gameObject8 = Wish.Utilities.FindObject(GameObject.Find("Manager"), "UI");
        //     if (gameObject8 != null)
        //     {
        //         gameObject8.SetActive(active);
        //     }
        //
        //     var gameObject9 = GameObject.Find("QuestTrackerVisibilityToggle");
        //     if (gameObject9 == null)
        //     {
        //         return;
        //     }
        //
        //     gameObject9.SetActive(active);
        // }
        //
        // [Command]
        // private void Setworldprogress(string progress)
        // {
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(progress, true);
        // }
        //
        // [Command]
        // private void Setzoom(int zoomLevel)
        // {
        //     var single = zoomLevel switch
        //     {
        //         1 => 22.5f,
        //         2 => 11.25f,
        //         3 => 7.5f,
        //         4 => 5.625f,
        //         _ => 7.5f
        //     };
        //
        //     Player.Instance.CameraZoomLevel = single;
        // }
        //
        // [Command]
        // private void Skipday()
        // {
        //     PlaySettingsManager.PlaySettings.skipEndOfDayScreen = true;
        //     SingletonBehaviour<DayCycle>.Instance.AddTime(1f);
        //     StartCoroutine(DelayInitializeScene());
        // }
        //
        // [Command]
        // private void Skipday(int days)
        // {
        //     PlaySettingsManager.PlaySettings.skipEndOfDayScreen = true;
        //     StartCoroutine(SkipDayRoutine(days));
        // }
        //
        // private IEnumerator SkipDayRoutine(int days)
        // {
        //     for (var i = 0; i < days; i++)
        //     {
        //         SingletonBehaviour<DayCycle>.Instance.AddTime(1f);
        //         yield return null;
        //         yield return null;
        //     }
        //
        //     SingletonBehaviour<ScenePortalManager>.Instance.InitializeScene();
        //     PlaySettingsManager.PlaySettings.skipEndOfDayScreen = false;
        // }
        //
        // [Command]
        // private void Skipintro()
        // {
        //     var trainIntroCutscene = FindObjectOfType<TrainIntroCutscene>();
        //     if (trainIntroCutscene)
        //     {
        //         trainIntroCutscene.SkipCutScene();
        //     }
        //
        //     var luciaCutscene = FindObjectOfType<LuciaCutscene>();
        //     if (luciaCutscene)
        //     {
        //         luciaCutscene.SkipCutScene();
        //     }
        //
        //     var newTrainIntroCutscene = FindObjectOfType<NewTrainIntroCutscene>();
        //     if (newTrainIntroCutscene)
        //     {
        //         newTrainIntroCutscene.SkipCutScene();
        //     }
        //
        //     var lynnsGoodbyeCutscene = FindObjectOfType<LynnsGoodbyeCutscene>();
        //     if (lynnsGoodbyeCutscene)
        //     {
        //         lynnsGoodbyeCutscene.SkipCutScene();
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Intro", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("NewTrainIntroCutscene", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("LuciaCutscene1", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("LuciaCutscene2", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Slept", true);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("PlacedHouse", true);
        //     Player.Instance.Inventory.AddItem(0x7533, 1, 0, true);
        //     Player.Instance.Inventory.AddItem(0x7534, 1, 0, true);
        //     Player.Instance.Inventory.AddItem(0x7537, 1, 0, true);
        //     Player.Instance.Inventory.AddItem(0x7535, 1, 0, true);
        //     Player.Instance.Inventory.AddItem(0x7536, 1, 0, true);
        //     Player.Instance.Inventory.AddItem(0x2ee0, 15, 0, true);
        //     Player.Instance.Inventory.AddItem(0x2906, 1, 0, true);
        //     Player.Instance.AddMoney(-200);
        //     Cutscene.WithinMultipartCutscene = false;
        // }
        //
        // [Command]
        // private void Skiptonpccycle(string npc, int cycle)
        // {
        //     if (SingletonBehaviour<NPCManager>.Instance._npcs.TryGetValue(npc, out var nPcai))
        //     {
        //         for (var i = 0; i < 16; i++)
        //         {
        //             SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat(npc, " Cycle ", i), i < cycle);
        //         }
        //
        //         nPcai.GenerateCycle(true);
        //     }
        // }
        //
        // [Command]
        // private void Skiptoworldquest(int breakpoint)
        // {
        //     Resetquests();
        //     if (breakpoint == 1)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection1Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection1Quest", breakpoint >= 2);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToDragonsMeetCutscene1", breakpoint >= 2);
        //     if (breakpoint == 2)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection2Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection2Quest", breakpoint >= 3);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToDragonsMeetCutscene2", breakpoint >= 3);
        //     if (breakpoint == 3)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection3Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection3Quest", breakpoint >= 4);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene1", breakpoint >= 4);
        //     if (breakpoint == 4)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection4Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection4Quest", breakpoint >= 5);
        //     if (breakpoint == 5)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection5Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection5Quest", breakpoint >= 6);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene2", breakpoint >= 6);
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection6Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection6Quest", breakpoint >= 7);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene3", breakpoint >= 7);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene4", breakpoint >= 7);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("CollectedGloriteCrystal", breakpoint >= 7);
        //     if (breakpoint == 7)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection7Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection7Quest", breakpoint >= 8);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene5", breakpoint >= 8);
        //     if (breakpoint == 8)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection8Quest");
        //     }
        // }
        //
        // [Command]
        // private void Skiptoworldquestnelvari(int breakpoint)
        // {
        //     Resetquests();
        //     if (breakpoint == 0)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection1Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection1Quest", breakpoint >= 1);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToDragonsMeetCutscene1", breakpoint >= 1);
        //     if (breakpoint == 1)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection2Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection2Quest", breakpoint >= 2);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToDragonsMeetCutscene2", breakpoint >= 2);
        //     if (breakpoint == 2)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection3Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection3Quest", breakpoint >= 3);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene1", breakpoint >= 3);
        //     if (breakpoint == 3)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection4Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection4Quest", breakpoint >= 4);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene2", breakpoint >= 4);
        //     if (breakpoint == 4)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection4Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection3Quest", breakpoint >= 5);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene3", breakpoint >= 5);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene4", breakpoint >= 5);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtectionCutscene5", breakpoint >= 5);
        //     if (breakpoint == 5)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection8Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection8Quest", breakpoint >= 6);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TimeOfNeedCutscene1", breakpoint >= 6);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TimeOfNeedCutscene2", breakpoint >= 6);
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("ClearingTheRoad1Quest");
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheMysteryOfNelvari1Quest");
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("SunDragonsApprentice1Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheMysteryOfNelvari1Quest", breakpoint >= 7);
        //     if (breakpoint == 7)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheMysteryOfNelvari2Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheMysteryOfNelvari2Quest", breakpoint >= 8);
        //     if (breakpoint == 8)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheMysteryOfNelvari3Quest");
        //     }
        //
        //     if (breakpoint <= 8)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressIntCharacter("NelvariTree", 0);
        //     }
        //
        //     if (breakpoint <= 8)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 0), false);
        //     }
        //
        //     if (breakpoint <= 8)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 1), false);
        //     }
        //
        //     if (breakpoint <= 8)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 2), false);
        //     }
        //
        //     if (breakpoint <= 8)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 3), false);
        //     }
        //
        //     StartCoroutine(DelayInitializeScene());
        // }
        //
        // [Command]
        // private void Skiptoworldquestwithergate(int breakpoint)
        // {
        //     Skiptoworldquest(9);
        //     if (breakpoint == 1)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheSunDragonsProtection8Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TheSunDragonsProtection8Quest", breakpoint >= 6);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TimeOfNeedCutscene1", breakpoint >= 6);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("TimeOfNeedCutscene2", breakpoint >= 6);
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("ClearingTheRoad1Quest");
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("TheMysteryOfNelvari1Quest");
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         Player.Instance.QuestList.StartQuest("SunDragonsApprentice1Quest");
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressIntCharacter("NelvariTree", 0);
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 0), false);
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 1), false);
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 2), false);
        //     }
        //
        //     if (breakpoint == 6)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(string.Concat("CompleteNelvariTree", 3), false);
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld("NorthTownMonster", breakpoint >= 7);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ClearingTheRoad1Quest", breakpoint >= 7);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ClearingTheRoadCutscene1", breakpoint >= 7);
        //     if (breakpoint == 7)
        //     {
        //         Player.Instance.QuestList.StartQuest("ClearingTheRoad2Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ClearingTheRoad2Quest", breakpoint >= 8);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ClearingTheRoadCutscene2", breakpoint >= 8);
        //     if (breakpoint == 8)
        //     {
        //         Player.Instance.QuestList.StartQuest("JourneyToWithergate1Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergate1Quest", breakpoint >= 9);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergateCutscene1", breakpoint >= 9);
        //     if (breakpoint == 9)
        //     {
        //         Player.Instance.QuestList.StartQuest("JourneyToWithergate2Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergate2Quest", breakpoint >= 10);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergateCutscene2", breakpoint >= 10);
        //     if (breakpoint == 10)
        //     {
        //         Player.Instance.QuestList.StartQuest("JourneyToWithergate3Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergate3Quest", breakpoint >= 11);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergateCutscene3", breakpoint >= 11);
        //     if (breakpoint == 11)
        //     {
        //         Player.Instance.QuestList.StartQuest("JourneyToWithergate4Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergate4Quest", breakpoint >= 12);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergateCutscene4", breakpoint >= 12);
        //     if (breakpoint == 12)
        //     {
        //         Player.Instance.QuestList.StartQuest("JourneyToWithergate5Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Apartment", breakpoint >= 13);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergate5Quest", breakpoint >= 13);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergateCutscene5", breakpoint >= 13);
        //     if (breakpoint == 13)
        //     {
        //         Player.Instance.QuestList.StartQuest("JourneyToWithergate6Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("JourneyToWithergate6Quest", breakpoint >= 14);
        //     if (breakpoint == 14)
        //     {
        //         Player.Instance.QuestList.StartQuest("ConfrontingDynus1Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynus1Quest", breakpoint >= 15);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynusCutscene1", breakpoint >= 15);
        //     if (breakpoint == 15)
        //     {
        //         Player.Instance.QuestList.StartQuest("ConfrontingDynus2Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynus2Quest", breakpoint >= 16);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynusCutscene2", breakpoint >= 16);
        //     if (breakpoint == 16)
        //     {
        //         Player.Instance.QuestList.StartQuest("ConfrontingDynus3Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynus3Quest", breakpoint >= 17);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynusCutscene3", breakpoint >= 17);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynusCutscene4", breakpoint >= 17);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynusCutscene5", breakpoint >= 17);
        //     if (breakpoint == 17)
        //     {
        //         Player.Instance.QuestList.StartQuest("ConfrontingDynus4Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynus4Quest", breakpoint >= 18);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("DynusAltarCutscene", breakpoint >= 18);
        //     if (breakpoint == 18)
        //     {
        //         Player.Instance.QuestList.StartQuest("ConfrontingDynus5Quest");
        //     }
        //
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("ConfrontingDynus5Quest", breakpoint >= 19);
        //     SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("DynusIntroCutscene", breakpoint >= 19);
        //     if (breakpoint == 19)
        //     {
        //         Player.Instance.QuestList.StartQuest("ConfrontingDynus6Quest");
        //     }
        // }
        //
        // [Command]
        // private void Spawnpet(string petname)
        // {
        //     SingletonBehaviour<PetManager>.Instance.SpawnPet(petname, Player.Instance, null);
        // }
        //
        // [Command]
        // private void Startquest(string questname)
        // {
        //     Player.Instance.QuestList.StartQuest(questname);
        // }
        //
        // [Command]
        // private void Teleport(string sceneName)
        // {
        //     var lower = sceneName.Trim().ToLower();
        //     var vector2 = lower switch
        //     {
        //         "playerhouse1" => new Vector2(48f, 68f),
        //         "Tier1House0" => new Vector2(48f, 68f),
        //         "throneroom" => new Vector2(21.36f, -2f),
        //         "forest" => new Vector2(21.67f, 7.5f),
        //         "portalroom" => new Vector2(23.5f, 14.14214f),
        //         "shoppingcenter" => new Vector2(61.81f, 25f),
        //         "wishingwell" => new Vector2(55f, 63f),
        //         "town" => new Vector2(67.41667f, 306.7076f),
        //         "town1" => new Vector2(67.41667f, 306.7076f),
        //         "town2" => new Vector2(67.41667f, 306.7076f),
        //         "town5" => new Vector2(67.41667f, 306.7076f),
        //         "withergaterooftopfarm" => new Vector2(126.125f, 83.6743f),
        //         "foresta" => new Vector2(284.7917f, 355.3212f),
        //         _ => (lower == "nelvari6" ? new Vector2(320.3333f, 98.76098f) : Vector2.zero)
        //     };
        //
        //     SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(vector2, sceneName, null, null, () =>
        //     {
        //         if (vector2.Equals(Vector2.zero))
        //         {
        //             var scenePortalSpotArray = FindObjectsOfType<ScenePortalSpot>();
        //             if (scenePortalSpotArray.Length == 0)
        //             {
        //                 return;
        //             }
        //
        //             var single = 0f;
        //             var scenePortalSpot = scenePortalSpotArray[0];
        //             foreach (var scenePortalSpot1 in scenePortalSpotArray)
        //             {
        //                 if (scenePortalSpot1.transform.position.x > single)
        //                 {
        //                     single = scenePortalSpot1.transform.position.x;
        //                     scenePortalSpot = scenePortalSpot1;
        //                 }
        //             }
        //
        //             var component = scenePortalSpot.GetComponent<BoxCollider2D>();
        //             if (component)
        //             {
        //                 var vector3 = scenePortalSpot.transform.position;
        //                 vector3.z = 0f;
        //                 var size = component.size;
        //                 Player.Instance.SetPosition(vector3 + (size.x > size.y ? Vector3.up * 2f : Vector3.left * 2f));
        //                 return;
        //             }
        //
        //             var vector31 = scenePortalSpot.transform.position;
        //             vector31.z = 0f;
        //             Player.Instance.SetPosition(vector31 + (Vector3.up * 2f));
        //         }
        //     });
        // }
        //
        // [Command]
        // private void Unlockmines()
        // {
        //     for (var i = 0; i < 55; i++)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(string.Concat("minesUnlock", i), true);
        //     }
        //
        //     for (var j = 1; j < 5; j++)
        //     {
        //         SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(string.Concat("minesUnlock", j * 10, "A"), true);
        //     }
        // }
    }
}