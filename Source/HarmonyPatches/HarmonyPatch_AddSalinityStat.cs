using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[HarmonyPatch(typeof(PlantProperties),"SpecialDisplayStats")]
public class HarmonyPatch_AddSalinityStat
{
    private static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, PlantProperties __instance)
    {
        if (__instance.sowTags.Contains("Panaquatic_Zone"))
        {
            WaterPlantPreference plantPreferenceRaw = PanaquaticUtility.getWaterPlantPreference(__instance);
            
            string plantPreferenceString = plantPreferenceRaw == WaterPlantPreference.WildTagged
                ? $"Panaquatic_{plantPreferenceRaw}Preference".Translate() + " (" + string.Join(", ", __instance.WildTerrainTags) + ")"
                : $"Panaquatic_{plantPreferenceRaw}Preference".Translate();
            
            var statEntry = new StatDrawEntry(
                StatCategoryDefOf.Basics,
                "Panaquatic_SalinityStat".Translate(), 
                plantPreferenceString,
                "Panaquatic_SalinityStat_Desc".Translate(),
                4157);
            return __result.Concat(statEntry);
        }
        return __result;
    }
}