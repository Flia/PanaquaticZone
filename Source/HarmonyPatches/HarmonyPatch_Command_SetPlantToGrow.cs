using HarmonyLib;
using Verse;
using RimWorld;

namespace RT_Saltwater;

//Uses Krafts.Publicizer to access WarnAsAppropriate method. It works, but is it the best solution?
[HarmonyPatch(typeof(Command_SetPlantToGrow), "WarnAsAppropriate")]
public static class HarmonyPatch_Command_SetPlantToGrow
{
    public static void Postfix(ThingDef plantDef, IPlantToGrowSettable ___settable)
    {
        SaltwaterUtility.WarnIfPreferenceMismatch(plantDef, ___settable);
    }
}

