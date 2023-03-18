using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DG.Tweening;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Wish;

namespace NoTimeForFishing;

[HarmonyPatch]
public static class Patches
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


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Bobber), nameof(Bobber.OnEnable))]
    private static void Bobber_Patches(ref Bobber __instance)
    {
        const float originalRadius = 0.7f;
        float radius;
        var message = $"\nOriginal base radius: {originalRadius}\n";
        var newBaseRadius = originalRadius;
        if (Plugin.DoubleBaseBobberAttractionRadius.Value)
        {
            newBaseRadius *= 2;
        }

        message += $"New base radius: {Plugin.DoubleBaseBobberAttractionRadius.Value}\n";

        if (GameSave.Fishing.GetNode("Fishing1b"))
        {
            radius = newBaseRadius * (1f + 0.1f * GameSave.Fishing.GetNodeAmount("Fishing1b"));
            message += $"Final radius due to talent increase: {radius}\n";
        }
        else
        {
            radius = newBaseRadius;
            message += $"Final radius: {radius}\n";
        }

        __instance.bobberRadius = radius;
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning($"{message}");
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.Use1))]
    private static void FishingRod_Use1(ref FishingRod __instance)
    {
        __instance.powerIncreaseSpeed = Plugin.FishingRodCastSpeed.Value;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Bobber), nameof(Bobber.GenerateWinArea))]
    private static void Bobber_GenerateWinArea(ref FishingMiniGame miniGame)
    {
        miniGame.winAreaSize = Math.Min(1f, miniGame.winAreaSize * Plugin.MiniGameWinAreaMultiplier.Value);
        miniGame.barMovementSpeed = Math.Min(Plugin.MiniGameMaxSpeed.Value, miniGame.barMovementSpeed);
        miniGame.sweetSpots[0].sweetSpotSize = Math.Min(1f, miniGame.sweetSpots[0].sweetSpotSize * Plugin.MiniGameWinAreaMultiplier.Value);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Utilities), nameof(Utilities.Chance))]
    public static void Utilities_Chance(ref bool __result)
    {
        if (Player.Instance.IsFishing && Plugin.NoMoreNibbles.Value)
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning("Player is fishing and no more nibbles true!");
            }

            __result = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishSpawnManager), nameof(FishSpawnManager.Start))]
    private static void FishSpawnManager_Start(ref int ___spawnLimit)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning("FishSpawnManager Start: Adjusting fish spawn multiplier and spawn limit...");
        }

        FishSpawnManager.fishSpawnGlobalMultiplier = Plugin.FishSpawnMultiplier.Value;
        FishSpawnManager.Instance.spawnLimit = Plugin.FishSpawnLimit.Value;
        ___spawnLimit = Plugin.FishSpawnLimit.Value;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.HasFish), typeof(Fish))]
    public static void FishingRod_HasFish(ref Fish fish, ref FishingRod __instance)
    {
        if (!Plugin.SkipFishingMiniGame.Value) return;
        if (Plugin.AutoReel.Value)
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning($"Attempting to auto-loot {fish.name}...");
            }

            __instance.UseDown1();
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.PushDialogue))]
    public static bool PushDialogue(ref DialogueController __instance, ref DialogueNode dialogue, ref UnityAction onComplete, ref bool animateOnComplete, ref bool ignoreDialogueOnGoing)
    {
        if (!Player.Instance.IsFishing)
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning("Player isn't fishing! Let dialogue run like normal...");
            }

            return true;
        }

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning("Player is fishing! Modify dialogue if their settings allow...");
        }

        var caughtFish = dialogue.dialogueText.Any(line => line.ToLowerInvariant().Contains("caught"));

        if (caughtFish)
        {
            if (Plugin.Debug.Value)
            {
                Plugin.LOG.LogWarning("Caught just a fish!");
            }

            if (Plugin.DisableCaughtFishWindow.Value)
            {
                onComplete?.Invoke();
                return false;
            }
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.UseDown1))]
    public static bool FishingRod_UseDown1(ref FishingRod __instance)
    {
        if (!Plugin.SkipFishingMiniGame.Value) return true;
        if (!__instance.player.IsOwner || !__instance._canUseFishingRod || __instance.Reeling)
        {
            return true;
        }

        if (__instance._fishing)
        {
            if (__instance.ReadyForFish)
            {
                if (!__instance._bobber.MiniGameInProgress)
                {
                    __instance.wonMiniGame = true;
                    __instance._frameRate = 8f;
                    __instance.ReadyForFish = false;
                    __instance.Reeling = true;
                    __instance._fishing = !__instance._fishing;
                    __instance._swingAnimation = (__instance._fishing ? SwingAnimation.VerticalSlash : SwingAnimation.Pull);
                    var rod = __instance;
                    DOVirtual.DelayedCall(Plugin.InstantAutoReel.Value ? 0 : __instance.ActionDelay / __instance.AttackSpeed(), delegate
                    {
                        rod.Action(rod.pos);
                        rod.SendFishingState(3);
                        rod.CancelFishingAnimation();
                        rod.StartCoroutine(rod.ResetFishingRodRoutine());
                    }, false);

                    return false;
                }
            }
        }

        return true;
    }


    public static float GetPathMoveSpeed(float defaultSpeed, Collider2D collider, Bobber bobber)
    {
        const float baseMoveSpeed = 1.25f;
        var newSpeed = baseMoveSpeed;

        Plugin.LOG.LogWarning("\nOriginal base path move speed: " + baseMoveSpeed);
        Plugin.LOG.LogWarning("\nPassed in default path move speed: " + defaultSpeed);

        if (Plugin.DoubleBaseFishSwimSpeed.Value)
        {
            newSpeed = baseMoveSpeed * 2f;
            Plugin.LOG.LogWarning("\nNew base path move speed: " + newSpeed);
        }

        if (SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[ProfessionType.Fishing].GetNode("Fishing1b"))
        {
            newSpeed *= 1.3f;
            Plugin.LOG.LogWarning("\nNew base path move speed (talented): " + newSpeed);
        }

        return newSpeed;
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Fish), nameof(Fish.TargetBobber))]
    public static IEnumerable<CodeInstruction> Fish_TargetBobber_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        var inner = typeof(Fish).GetNestedType("<>c__DisplayClass0_0__1", AccessTools.all)
                    ?? throw new Exception("Inner Not Found");

        var field = AccessTools.Field(inner, "collider");

        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(Fish), nameof(Fish._targetBobber))),
                new CodeMatch(OpCodes.Ldarg_2))
            .Advance(1)
            .InsertAndAdvance(new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Fish), nameof(Fish._pathMoveSpeed))),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldfld, field),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(Patches.GetPathMoveSpeed))),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(Fish), nameof(Fish._pathMoveSpeed))),
            })
            .InstructionEnumeration();
    }
}