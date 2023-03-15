using System.Linq;
using DG.Tweening;
using HarmonyLib;
using UnityEngine.Events;
using Wish;

namespace NoTimeForFishing;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.PushDialogue))]
    public static bool PushDialogue(ref DialogueController __instance, ref DialogueNode dialogue, ref UnityAction onComplete, ref bool animateOnComplete, ref bool ignoreDialogueOnGoing)
    {
        if (!Plugin.DisableCaughtFishWindow.Value) return true;

        if (dialogue.dialogueText.Any(line => line.ToLowerInvariant().Contains("caught")))
        {
            onComplete.Invoke();
            return false;
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
                    DOVirtual.DelayedCall(__instance.ActionDelay / __instance.AttackSpeed(), delegate
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