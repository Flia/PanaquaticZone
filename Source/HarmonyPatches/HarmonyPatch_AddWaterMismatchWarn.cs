using HarmonyLib;
using Verse;
using RimWorld;

namespace PanaquaticZone;

[HarmonyPatch(typeof(Command_SetPlantToGrow), "WarnAsAppropriate")]
public static class HarmonyPatch_AddWaterMismatchWarnTo_WarnAsAppropriate
{
    public static void Postfix(ThingDef plantDef, IPlantToGrowSettable ___settable)
    {
        if (___settable is Zone_Panaquatic)
        {
            PanaquaticUtility.WarnIfPreferenceMismatch(plantDef, ___settable);
        }
    }
}
