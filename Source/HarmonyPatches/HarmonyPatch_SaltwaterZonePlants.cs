using RimWorld;
using Verse;
using HarmonyLib;

namespace RT_Saltwater;

[HarmonyPatch(typeof(PlantUtility), "CanSowOnGrower")]
public static class HarmonyPatch_SaltwaterZonePlants
{ 
    public static bool Postfix(bool result, ThingDef plantDef, object obj)
    {
        if (obj is Zone_Saltwater)
            return plantDef.plant.sowTags.Contains("RT_Saltwater") || plantDef.plant.sowTags.Contains("VCE_Aquatic") ||
                   plantDef.plant.sowTags.Contains("Water");
        return result;
    }
}