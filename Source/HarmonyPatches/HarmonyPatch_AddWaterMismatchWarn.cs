using HarmonyLib;
using Verse;
using RimWorld;

namespace RT_Saltwater;

//Uses Krafts.Publicizer to access WarnAsAppropriate method. It works, but is it the best solution?
//check if AccessTools can't do the job
[HarmonyPatch(typeof(Command_SetPlantToGrow), "WarnAsAppropriate")]
public static class HarmonyPatch_AddWaterMismatchWarnTo_WarnAsAppropriate
{
    public static void Postfix(ThingDef plantDef, IPlantToGrowSettable ___settable)
    {
        if (___settable is Zone_Saltwater)
        {
            SaltwaterUtility.WarnIfPreferenceMismatch(plantDef, ___settable);
        }
    }
}
