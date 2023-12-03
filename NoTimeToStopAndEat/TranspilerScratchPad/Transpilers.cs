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
// public static class Transpilers
// {
//     [HarmonyTranspiler]
//     [HarmonyPatch(typeof(Food), nameof(Food.EatFoodRoutine), MethodType.Enumerator)]
//     private static IEnumerable<CodeInstruction> Food_EatFoodRoutine_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
//     {
//
//         FieldInfo itemGraphics;
//         try
//         {
//             itemGraphics = AccessTools.Field(typeof(Food), nameof(Food._itemGraphics));
//         }
//         catch (System.Exception)
//         {
//             return instructions;
//         }
//         
//         ConstructorInfo wfsConstructor;
//         try
//         {
//             wfsConstructor = AccessTools.Constructor(typeof(WaitForSeconds), new[] {typeof(float)});
//         }
//         catch (System.Exception)
//         {
//             return instructions;
//         }
//         
//         MethodInfo emoteMethod;
//         try
//         {
//             emoteMethod = AccessTools.Method(typeof(Player), nameof(Player.Emote));
//         }
//         catch (System.Exception)
//         {
//             return instructions;
//         }
//
//
//         var conFound = false;
//         var count = 0;
//         var emoteFound = false;
//         var codes = new List<CodeInstruction>(instructions);
//
//         for (var i = 0; i < codes.Count; i++)
//         {
//
//             if (codes[i].LoadsField(itemGraphics))
//             {
//                 if(codes[i+2].operand is 0.6f)
//                 {
//                     codes[i+2].operand = 0f;
//                     Plugin.LOG.LogInfo($"Found 0.6f duration and changed to 0. {codes[i+3].operand}");
//                 }
//                 if (codes[i+2].operand is 0.24000001f)
//                 {
//                     codes[i+2].operand = 0f;
//                     Plugin.LOG.LogInfo($"Found 0.24000001f duration and changed to 0. {codes[i + 1].operand}");
//                 }
//                 if (codes[i+5].operand is 0.27f)
//                 {
//                     codes[i+5].operand = 0f;
//                     Plugin.LOG.LogInfo($"Found 0.27f duration and changed to 0. {codes[i + 1].operand}");
//                 }
//                 
//                 if (codes[i+3].operand is 0.3f)
//                 {
//                     codes[i+3].operand = 0f;
//                     Plugin.LOG.LogInfo($"Found 0.3f duration and changed to 0. {codes[i + 1].operand}");
//                 }
//             }
//             
//
//             if (codes[i].opcode == OpCodes.Ldc_R4 &&
//                 i + 1 < codes.Count && codes[i + 1].opcode == OpCodes.Newobj &&
//                 codes[i + 1].operand as ConstructorInfo == wfsConstructor)
//             {
//                 codes[i].operand = 0f;
//                 conFound = true;
//             }
//             else if (codes[i].Calls(emoteMethod))
//             {
//                 if (codes[i - 1].operand is 99) continue;
//
//                 codes[i - 1].opcode = OpCodes.Ldc_I4;
//                 codes[i - 1].operand = 99;
//                 emoteFound = true;
//                 count++;
//             }
//         }
//
//         if (conFound)
//         {
//             Plugin.LOG.LogInfo($"Successfully patched WaitForSeconds constructor call in {originalMethod.DeclaringType}.{originalMethod.Name}");
//         }
//
//         if (emoteFound)
//         {
//             Plugin.LOG.LogInfo($"Successfully patched {count} Player.Emote call(s) in {originalMethod.DeclaringType}.{originalMethod.Name}");
//         }
//
//         return codes;
//     }
// }