﻿using HarmonyLib;
using QFSW.QC;
using Wish;

namespace CheatEnabler;

[HarmonyPatch]
public partial class Plugin
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuestManager), nameof(QuestManager.UpdateQuestItems))]
    public static void Enable_Cheat_Menu()
    {
        Settings.EnableCheats = true;
        QuantumConsole.Instance.GenerateCommands = true;
        QuantumConsole.Instance.Initialize();
        LOG.LogWarning("Cheat Menu Enabled...");
    }
}