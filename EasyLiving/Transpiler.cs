using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Wish;

namespace EasyLiving;

[Harmony]
public static class Transpiler
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Well), nameof(Well.OnMouseOver))]
    public static IEnumerable<CodeInstruction> Well_OnMouseOver_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        if (!Plugin.IncreaseWateringCanFillRange.Value) return instructions;
        
        var magnitudeGetter = AccessTools.PropertyGetter(typeof(Vector3), nameof(Vector3.magnitude));

        var codes = new List<CodeInstruction>(instructions);
        var foundMatchingSequence = false;

        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].Calls(magnitudeGetter) && codes[i + 1].operand.Equals(3f))
            {
                codes[i + 1].operand = 10f;
                foundMatchingSequence = true;
                break;
            }
        }

        if (foundMatchingSequence)
        {
            Plugin.LOG.LogInfo(
                $"Found the matching opcode sequence in {originalMethod.Name}. Watering Can fill range should be increased.");
        }
        else
        {
            Plugin.LOG.LogError(
                $"Failed to find the matching opcode sequence in {originalMethod.Name}.");
        }


        return codes.AsEnumerable();
    }
}