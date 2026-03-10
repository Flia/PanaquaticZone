using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[HarmonyPatch(typeof(PlantProperties),"SpecialDisplayStats")]
public class HarmonyPatch_AddSalinityStat
{
    private static string Panaquatic_SalinityStat_Desc, Panaquatic_SalinityStat;
    private static Dictionary<string, string> PlantPreferenceStrings = new();
    private static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, PlantProperties __instance)
    {
        if (!__instance.sowTags.Contains("Panaquatic_Zone")) 
            return __result;

        WaterPlantPreference plantPreferenceRaw = __instance.getWaterPlantPreference();

        Panaquatic_SalinityStat_Desc ??= "Panaquatic_SalinityStat_Desc".Translate([
            PanaquaticStartupTasks.freshwaterTilesStatDisplayCache,
            PanaquaticStartupTasks.saltwaterTilesStatDisplayCache
        ]);

        Panaquatic_SalinityStat ??= "Panaquatic_SalinityStat".Translate();

        if (!PlantPreferenceStrings.TryGetValue($"Panaquatic_{plantPreferenceRaw}Preference", out string plantReference))
        {
            plantReference = $"Panaquatic_{plantPreferenceRaw}Preference".Translate().ToString();
            PlantPreferenceStrings.Add($"Panaquatic_{plantPreferenceRaw}Preference", plantReference);
        }

        StatDrawEntry statEntry = new(
            StatCategoryDefOf.Basics,
            Panaquatic_SalinityStat,
            plantReference,
            Panaquatic_SalinityStat_Desc,
            4157);
        return __result.Concat(statEntry);
    }
}