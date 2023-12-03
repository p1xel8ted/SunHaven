// using System.Collections.Generic;
// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using UnityEngine;
// using Wish;
//
// namespace NoTimeToEat;
//
// [Harmony]
// public static class NewTranspiler
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
//     // [HarmonyPatch(typeof(Food), nameof(Food.EatFoodRoutine), MethodType.Enumerator)]
//     private static IEnumerable<CodeInstruction> Food_EatFoodRoutine_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
//     {
//    var codes = new List<CodeInstruction>(instructions);
//
//    Plugin.LOG.LogWarning($"Transpiling {originalMethod.Name}...");
//         for (var i = 0; i < codes.Count; i++)
//         {
//             //print IL
//             
//             Plugin.LOG.LogWarning($"[{i}]: {codes[i].opcode} {codes[i].operand}");
//         //     if (codes[i].operand is ConstructorInfo constructorInfo && constructorInfo == wfsConstructor)
//         //     {
//         //         codes[i - 2].opcode = OpCodes.Nop;
//         //         codes[i - 1].opcode = OpCodes.Nop;
//         //         codes[i].opcode = OpCodes.Nop;
//         //         codes[i + 1].opcode = OpCodes.Nop;
//         //     }
//         //
//         //     if (codes[i].Calls(emoteMethod))
//         //     {
//         //         codes[i - 3].opcode = OpCodes.Nop;
//         //         codes[i - 2].opcode = OpCodes.Nop;
//         //         codes[i - 1].opcode = OpCodes.Nop;
//         //         codes[i].opcode = OpCodes.Nop;
//         //         
//         //         codes[i - 3].operand = null;
//         //         codes[i - 2].operand = null;
//         //         codes[i - 1].operand = null;
//         //         codes[i].operand = null;
//         //     }
//         //
//         //     if (codes[i].Calls(cancelEmote))
//         //     {
//         //         for (var j = 0; j < 26; j++)
//         //         {
//         //             codes[i + j + 1].opcode = OpCodes.Nop;
//         //             codes[i + j + 1].operand = null;
//         //         }
//         //     }
//         //
//         //     if (codes[i].StoresField(eating) && codes[i+3].StoresField(canEat))
//         //     {
//         //        
//         //         for (var j = 1; j < 80; j++)
//         //         {
//         //             Plugin.LOG.LogWarning($"[{i + j + 3}]: {codes[i + j + 3].opcode} {codes[i + j + 3].operand}");
//         //             codes[i + j + 3].opcode = OpCodes.Nop;
//         //             codes[i + j + 3].operand = null;
//         //         }
//         //     }
//         }
//
//         return codes;
//     }
//
//
//     private static void EatParticles()
//     {
//         var vector = Player.Instance.transform.position + new Vector3(0f, -0.15f, -1f);
//         ParticleManager.Instance.InstantiateParticle(SingletonBehaviour<Prefabs>.Instance.eatParticles, vector);
//     }
// }