using RimWorld;
using Verse;
using HarmonyLib;

namespace RT_Saltwater;

[HarmonyPatch(typeof(PlantUtility), "CanSowOnGrower")]
public static class HarmonyPatch_SaltwaterZonePlants
{
    public static void Postfix(ThingDef plantDef, object obj, ref bool __result)
    {
        if (obj is Zone_Saltwater saltwater)
        {
            __result = PollutionUtility.CanPlantAt(plantDef, saltwater) && 
                       SaltwaterUtility.CanPlantAt(plantDef, saltwater) &&
                       (plantDef.plant.sowTags.Contains("RT_Saltwater") ||
                        plantDef.plant.sowTags.Contains("VCE_Aquatic") ||
                        plantDef.plant.sowTags.Contains("Water"));
        }
    }
}