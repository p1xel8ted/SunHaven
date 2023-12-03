// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using Wish;
//
// namespace NoTimeToEat;
//
// [Harmony]
// public static class RemoveTweenEffectsTranspiler
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
//     public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
//     {
//         var codes = new List<CodeInstruction>(instructions);
//
//         for (int i = 0; i < codes.Count; i++)
//         {
//             // Identify the tween-related calls by their opcode and operand
//             if ((codes[i].opcode == OpCodes.Call || codes[i].opcode == OpCodes.Callvirt) &&
//                 codes[i].operand is MethodInfo method &&
//                 (method.DeclaringType.FullName.Contains("DG.Tweening") || method.Name.StartsWith("DO")))
//             {
//                 // Remove the tween call
//                 codes.RemoveAt(i);
//
//                 Plugin.LOG.LogWarning($"Removed tween call: {method.Name}");
//                 // Depending on the method's return type, you might need to pop the return value off the stack
//                 // if the method returns a value (e.g., a Tweener). This is a simplistic approach and might need adjustments.
//                 if (method.ReturnType != typeof(void))
//                 {
//                     Plugin.LOG.LogWarning($"Popping return value off the stack for {method.Name}");
//                     codes.Insert(i, new CodeInstruction(OpCodes.Pop));
//                 }
//
//                 // Adjust the index to account for the removed instruction
//                 i--;
//             }
//         }
//
//         return codes.AsEnumerable();
//     }
// }