using RimWorld;
using Verse;
using HarmonyLib;

namespace RT_Saltwater;

[HarmonyPatch(typeof(PlantUtility), "CanSowOnGrower")]
public static class HarmonyPatch_SaltwaterZonePlants
{ 
    public static bool Postfix(bool result, ThingDef plantDef, object obj)
    {
        return obj is Zone_Saltwater ? plantDef.plant.sowTags.Contains("RT_Merren_Saltwater") : result;
    }
}