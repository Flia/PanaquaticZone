using System.Reflection;
using HarmonyLib;
using Verse;
using RimWorld;

namespace RT_Saltwater;

[HarmonyPatch]
public static class HarmonyPatch_AddWaterMismatchWarnTo_WarnAsAppropriate
{
    //target method is private so I need this setup. I think. This stuff is deep magic to me
    [HarmonyTargetMethod]
    public static MethodInfo TargetMethod()
    {
        return AccessTools.Method("Verse.Command_SetPlantToGrow:WarnAsAppropriate");
    }

    [HarmonyPostfix]
    public static void Postfix(ThingDef plantDef, IPlantToGrowSettable ___settable)
    {
        if (___settable is Zone_Saltwater)
        {
            SaltwaterUtility.WarnIfPreferenceMismatch(plantDef, ___settable);
        }
    }
}
