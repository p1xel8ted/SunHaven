using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.ResourceManagement.AsyncOperations;
using Wish;

namespace CustomTexturesRedux;

[Harmony]
public static class Transpilers
{
    private const string LoadClothingSprites = "LoadClothingSprites";
    private static readonly MethodInfo AsyncOpHandleResultGetter = AccessTools.PropertyGetter(typeof(AsyncOperationHandle<ClothingLayerSprites>), nameof(AsyncOperationHandle<ClothingLayerSprites>.Result));
    private static readonly MethodInfo HandleResultMethod = AccessTools.Method(typeof(TextureUtils), nameof(TextureUtils.HandleResult));

    [HarmonyTargetMethods]
    public static IEnumerable<MethodBase> TargetMethods()
    {
        // Target methods named "LoadClothingSprites" within nested types of ClothingLayerData
        var targetMethods = typeof(ClothingLayerData)
            .GetTypeInfo()
            .GetNestedTypes(AccessTools.all)
            .SelectMany(t => t.GetMethods(AccessTools.all))
            .Where(m => m.Name.Contains(LoadClothingSprites));

        foreach (var method in targetMethods)
            yield return method;
    }
    
    [HarmonyPriority(0)]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        Plugin.Log.LogInfo("Patching ClothingLayerData.LoadClothingSprites");
        var found = false;
        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++)
        {
            if (!codes[i].Calls(AsyncOpHandleResultGetter)) continue;
            found = true;
            codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, HandleResultMethod));
            i++;
        }

        if (found)
        {
            Plugin.Log.LogInfo($"Successfully patched ClothingLayerData.LoadClothingSprites!");
        }
        else
        {
            Plugin.Log.LogError($"Failed to patch ClothingLayerData.LoadClothingSprites!");
        }
        
        return codes.AsEnumerable();
    }
}