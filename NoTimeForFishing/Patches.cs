using System;
using System.Linq;
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
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.HasFish), typeof(Fish))]
    public static void FishingRod_HasFish(ref Fish fish, ref FishingRod __instance)
    {
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
        var caughtMuseum = dialogue.dialogueText.Any(line => line.ToLowerInvariant().Contains("museum"));

        if (caughtFish)
        {
            if (caughtMuseum)
            {
                if (Plugin.Debug.Value)
                {
                    Plugin.LOG.LogWarning("Caught a fish and a museum piece!");
                }

                if (Plugin.DisableCaughtFishMuseumWindow.Value)
                {
                    onComplete?.Invoke();
                    return false;
                }
            }
            else
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
                    // __instance._frameRate = 8f;
                    // __instance.ReadyForFish = false;
                    // __instance.Reeling = true;
                    // __instance._fishing = !__instance._fishing;
                    // __instance._swingAnimation = (__instance._fishing ? SwingAnimation.VerticalSlash : SwingAnimation.Pull);
                    var rod = __instance;
                    DOVirtual.DelayedCall(Plugin.InstantAutoReel.Value ? 0 : __instance.ActionDelay / __instance.AttackSpeed(), delegate
                    {
                        rod.Action(rod.pos);
                        rod.SendFishingState(3);
                        rod.CancelFishingAnimation();
                    }, false);

                    return false;
                }
            }
        }

        return true;
    }
}