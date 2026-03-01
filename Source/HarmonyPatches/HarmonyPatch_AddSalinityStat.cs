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

            string SalinityDesc = "Panaquatic_SalinityStat_Desc".Translate([PanaquaticStartupTasks.freshwaterTilesStatDisplayCache, PanaquaticStartupTasks.saltwaterTilesStatDisplayCache]);
            if (plantPreferenceRaw == WaterPlantPreference.WildTagged)
            {
                SalinityDesc += "\n\n"
                                + "Panaquatic_SalinityStat_DescWildTagged".Translate().Colorize(ColoredText.TipSectionTitleColor)
                                + PanaquaticStartupTasks.wildTaggedTilesCacheDictionary[__instance];
            }
                
            var statEntry = new StatDrawEntry(
                StatCategoryDefOf.Basics,
                "Panaquatic_SalinityStat".Translate(), 
                $"Panaquatic_{plantPreferenceRaw}Preference".Translate(),
                SalinityDesc,
                4157);
            return __result.Concat(statEntry);
        }
        return __result;
    }
}