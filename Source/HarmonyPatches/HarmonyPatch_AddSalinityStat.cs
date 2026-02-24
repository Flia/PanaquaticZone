/*using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[HarmonyPatch(typeof(ThingDef),"SpecialDisplayStats")]
public class HarmonyPatch_AddSalinityStat
{
    public static void Postfix(ref StatRequest req, ref IEnumerable<StatDrawEntry> __result, ThingDef ___instance)
    {
        if (___instance.plant != null && ___instance.plant.sowTags.Contains("Panaquatic_Zone"))
        {
            WaterPlantPreference plantPreferenceRaw = ___instance.GetModExtension<PlantSalinityPreference>().plantPreference;
            
            string plantPreferenceString =
                ___instance.GetModExtension<PlantSalinityPreference>().plantPreference == WaterPlantPreference.WildTagged
                    ? $"Panaquatic_{plantPreferenceRaw}Preference".Translate() + " (" +
                      string.Join(", ", ___instance.plant.WildTerrainTags) + ")"
                    : $"Panaquatic_{plantPreferenceRaw}Preference".Translate();
            
            __result = __result.Append(new StatDrawEntry(
                StatCategoryDefOf.Basics,
                "Panaquatic_SalinityStat".Translate(), 
                plantPreferenceString,
                "Panaquatic_SalinityStat_Desc".Translate(), //has placeholder language key
                4157));
        }
    }
}*/