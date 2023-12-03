// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using Wish;
//
// namespace NoTimeToEat;
//
// [HarmonyPatch]
// internal static class EatFoodRoutinePatch
// {
//     private const string MoveNext = "MoveNext";
//     private const string EatFoodRoutine = "<EatFoodRoutine>";
//
//     [HarmonyTargetMethods]
//     private static IEnumerable<MethodBase> TargetMethods()
//     {
//         var parentType = typeof(Food);
//         foreach (var method in parentType.GetMethods(AccessTools.all))
//         {
//             if (!method.Name.Contains(EatFoodRoutine) && !method.Name.Contains(MoveNext)) continue;
//             yield return method;
//         }
//
//         foreach (var nestedType in parentType.GetNestedTypes(AccessTools.all))
//         {
//             foreach (var method in nestedType.GetMethods(AccessTools.all))
//             {
//                 if (!method.Name.Contains(EatFoodRoutine) && !method.Name.Contains(MoveNext)) continue;
//                 yield return method;
//             }
//         }
//     }
//
//     [HarmonyTranspiler]
//     public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
//     {
//         MethodInfo methodCalled;
//         try
//         {
//             methodCalled = AccessTools.Method(typeof(Player), nameof(Player.Emote));
//         }
//         catch (Exception)
//         {
//             return instructions;
//         }
//
//         var count = 0;
//         var found = false;
//         var codes = new List<CodeInstruction>(instructions);
//         for (var i = 0; i < codes.Count; i++)
//         {
//             if (!codes[i].Calls(methodCalled)) continue;
//             if (codes[i - 1].operand is 99) continue;
//             codes[i - 1].opcode = OpCodes.Ldc_I4;
//             codes[i - 1].operand = 99;
//             found = true;
//             count++;
//         }
//
//         if (found)
//         {
//             Plugin.LOG.LogInfo($"Successfully patched {count} Player.Emote call(s) in {originalMethod.DeclaringType}.{originalMethod.Name}");
//         }
//
//         return codes.AsEnumerable();
//     }
// }