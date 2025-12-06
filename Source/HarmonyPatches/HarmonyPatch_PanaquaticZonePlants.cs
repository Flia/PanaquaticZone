using RimWorld;
using Verse;
using HarmonyLib;

namespace PanaquaticZone;

[HarmonyPatch(typeof(PlantUtility), "CanSowOnGrower")]
public static class HarmonyPatch_PanaquaticZonePlants
{
    public static void Postfix(ThingDef plantDef, object obj, ref bool __result)
    {
        if (obj is Zone_Panaquatic saltwater)
        {
            __result = PollutionUtility.CanPlantAt(plantDef, saltwater) && 
                       PanaquaticUtility.CanPlantAt(plantDef, saltwater) &&
                       plantDef.plant.sowTags.Contains("Panaquatic_Zone");
        }
    }
}