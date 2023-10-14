using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DG.Tweening;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using Wish;

namespace NoTimeForFishing;

[HarmonyPatch]
public static class Patches
{
    private static int MaxFishCount { get; set; }

    //fix
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Map), nameof(Map.UpdatePlayerImagePosition))]
    private static bool Map_UpdatePlayerImagePosition(ref Map __instance)
    {
        return __instance is not null && Player.Instance is not null;
    }

    private static int GetBubbleSpellCap()
    {
        var fishingSkill = GameSave.Fishing.GetNodeAmount("Fishing7a");
        var amount = fishingSkill switch
        {
            1 => Plugin.IncreaseBubbleSpellCap.Value ? 8 : 6,
            2 => Plugin.IncreaseBubbleSpellCap.Value ? 10 : 8,
            3 => Plugin.IncreaseBubbleSpellCap.Value ? 12 : 10,
            _ => Plugin.IncreaseBubbleSpellCap.Value ? 6 : 4
        };
        return amount;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishingBubble), nameof(FishingBubble.Awake))]
    public static void FishingBubble_Awake(ref FishingBubble __instance)
    {
        if (!Plugin.ModifyBubbleSpell.Value) return;
        var fishingSkill = GameSave.Fishing.GetNodeAmount("Fishing7a");
        MaxFishCount = GetBubbleSpellCap();
        Plugin.LOG.LogWarning(
            $"BubbleSkillLevel: {fishingSkill}/3, Max fish count: {MaxFishCount}.");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Tooltip), nameof(Tooltip.SetText))]
    public static void Tooltip_SetText(ref string name, ref string description, int lineHeight = 100)
    {
        if (!Plugin.ModifyBubbleSpell.Value) return;
        if (name.Contains("Bubble Net"))
        {
            var low = Plugin.IncreaseBubbleSpellCap.Value ? 8 : 6;
            var medium = Plugin.IncreaseBubbleSpellCap.Value ? 10 : 8;
            var high = Plugin.IncreaseBubbleSpellCap.Value ? 12 : 10;
            description +=
                $" \nThe bubble can hold a maximum of <color=#F8F377>{low}</color>/<color=#cdfb41>{medium}</color>/<color=#3FFB45>{high}</color> fish.";
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FishingBubble), nameof(FishingBubble.OnTriggerEnter2D))]
    private static bool OnTriggerEnter2D(ref FishingBubble __instance, ref Collider2D col)
    {
        if (!Plugin.ModifyBubbleSpell.Value) return true;
        return __instance.items.Count < MaxFishCount;
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
        if (Plugin.ModifyFishingRodCastSpeed.Value)
        {
            __instance.powerIncreaseSpeed = Plugin.FishingRodCastSpeed.Value;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Bobber), nameof(Bobber.GenerateWinArea))]
    private static void Bobber_GenerateWinArea(ref FishingMiniGame miniGame)
    {
        if (Plugin.ModifyMiniGameWinAreaMultiplier.Value)
        {
            miniGame.winAreaSize = Math.Min(1f, miniGame.winAreaSize * Plugin.MiniGameWinAreaMultiplier.Value);
            miniGame.sweetSpots[0].sweetSpotSize = Math.Min(1f,
                miniGame.sweetSpots[0].sweetSpotSize * Plugin.MiniGameWinAreaMultiplier.Value);
        }

        if (Plugin.ModifyMiniGameSpeed.Value)
        {
            miniGame.barMovementSpeed = Math.Min(Plugin.MiniGameMaxSpeed.Value, miniGame.barMovementSpeed);
        }
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

        if (Plugin.ModifyFishSpawnMultiplier.Value)
        {
            FishSpawnManager.fishSpawnGlobalMultiplier = Plugin.FishSpawnMultiplier.Value;
        }

        if (Plugin.ModifyFishSpawnLimit.Value)
        {
            FishSpawnManager.Instance.spawnLimit = Plugin.FishSpawnLimit.Value;
            ___spawnLimit = Plugin.FishSpawnLimit.Value;
        }
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
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.Action))]
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.CheckForWater))]
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.GetBobberHeight))]
    public static void FishingRod_CastDistance(ref FishingRod __instance)
    {
        if (!Plugin.EnhanceBaseCastLength.Value) return;
        __instance.throwDistance = 6;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Fish), nameof(Fish.TargetBobber))]
    public static void FishingRod_TargetBobber(ref Bobber bobber)
    {
        if (!Plugin.InstantAttraction.Value) return;
        bobber.FishingRod.fishAttractionRate = -100;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.PushDialogue))]
    public static bool PushDialogue(ref DialogueController __instance, ref DialogueNode dialogue,
        ref UnityAction onComplete, ref bool animateOnComplete, ref bool ignoreDialogueOnGoing)
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
                    __instance._swingAnimation =
                        (__instance._fishing ? SwingAnimation.VerticalSlash : SwingAnimation.Pull);
                    var rod = __instance;

                    DOVirtual.DelayedCall(
                        Plugin.InstantAutoReel.Value ? 0 : __instance.ActionDelay / __instance.AttackSpeed(), delegate
                        {
                            rod.Action(rod.pos);
                            rod.SendFishingState(3);
                            rod.CancelFishingAnimation();
                            rod._canUseFishingRod = true;
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

        var message = $"\nOriginal base path move speed: {baseMoveSpeed}";

        message += $"\nPassed in default path move speed: {defaultSpeed}";

        if (Plugin.DoubleBaseFishSwimSpeed.Value)
        {
            newSpeed = baseMoveSpeed * 2f;
            message += $"\nNew base path move speed: {newSpeed}";
        }

        if (SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[ProfessionType.Fishing]
            .GetNode("Fishing1b"))
        {
            newSpeed *= 1.3f;
            message += $"\nNew base path move speed (talented): {newSpeed}";
        }

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogWarning(message);
        }

        return newSpeed;
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Fish), nameof(Fish.TargetBobber))]
    public static IEnumerable<CodeInstruction> Fish_TargetBobber_Transpiler(IEnumerable<CodeInstruction> instructions,
        MethodBase originalMethod)
    {
        List<FieldInfo> colliders = new();
        colliders.Clear();
        var innerTypes = typeof(Fish).GetNestedTypes(AccessTools.all);
        var innerColliders = innerTypes.Where(type =>
            type.GetFields(AccessTools.all).Any(field => field.FieldType == typeof(Collider2D))).ToList();
        colliders.AddRange(innerColliders.SelectMany(type =>
            type.GetFields(AccessTools.all).Where(field => field.FieldType == typeof(Collider2D))));

        if (colliders.Count == 0)
        {
            Plugin.LOG.LogError(
                $"Failed to find any colliders in {originalMethod.Name}. Fish swim speed will not be modified.");
            return instructions.AsEnumerable();
        }

        var field = colliders[0];

        var codes = new List<CodeInstruction>(instructions);
        var foundMatchingSequence = false;

        for (var i = 0; i < codes.Count - 2; i++)
        {
            if (codes[i].opcode == OpCodes.Stfld && (FieldInfo) codes[i].operand ==
                AccessTools.Field(typeof(Fish), nameof(Fish._targetBobber)) &&
                codes[i + 1].opcode == OpCodes.Ldarg_2)
            {
                foundMatchingSequence = true;

                var insertInstructions = new List<CodeInstruction>
                {
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldfld, AccessTools.Field(typeof(Fish), nameof(Fish._pathMoveSpeed))),
                    new(OpCodes.Ldloc_0),
                    new(OpCodes.Ldfld, field),
                    new(OpCodes.Ldarg_2),
                    new(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(GetPathMoveSpeed))),
                    new(OpCodes.Stfld, AccessTools.Field(typeof(Fish), nameof(Fish._pathMoveSpeed))),
                };

                codes.InsertRange(i + 2, insertInstructions);
                break;
            }
        }

        if (foundMatchingSequence)
        {
            Plugin.LOG.LogWarning(
                $"Found the matching opcode sequence in {originalMethod.Name}. Fish swim speed will modified.");
        }
        else
        {
            Plugin.LOG.LogError(
                $"Failed to find the matching opcode sequence in {originalMethod.Name}. Fish swim speed will not be modified.");
        }


        return codes.AsEnumerable();
    }
}