using RimWorld;
using Verse;
using HarmonyLib;

namespace PanaquaticZone;

[HarmonyPatch(typeof(PlantUtility), nameof(PlantUtility.CanSowOnGrower))]
public static class HarmonyPatch_PanaquaticPlantList
{
    public static void Postfix(ThingDef plantDef, object obj, ref bool __result)
    {
        if (obj is Zone_Panaquatic panaquatic)
        {
            __result = PollutionUtility.CanPlantAt(plantDef, panaquatic) && 
                       PanaquaticUtility.CanPlantAt(plantDef, panaquatic) &&
                       plantDef.plant.sowTags.Contains("Panaquatic_Zone");
        }
    }
}