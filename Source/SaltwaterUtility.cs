using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public static class SaltwaterUtility
{
    private static readonly HashSet<string> saltwaterTags =
        ["Ocean"];
    private static readonly HashSet<string> freshwaterTags =
        ["River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    private static readonly HashSet<string> eitherTags =
        ["Ocean", "River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    
    //A heavily edited variant of vanilla PollutionUtility func
    public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        var plantTags = CreateTagsForPlant(plantDef);

        if (plantTags == null) return false;
        
        foreach (IntVec3 cell in settable.Cells)
        {
            if (plantTags.Overlaps(cell.GetTerrain(settable.Map).tags.OrElseEmptyEnumerable()))
                return true;
        }
        return false;
    }
    
    
    //if plant has wildTags, return them, otherwise get from ModExtension, otherwise return Freshwater set
    //NB: plants without wildTags are *usually* cultivars-only
    public static HashSet<string> CreateTagsForPlant(ThingDef plantDef)
    {
        if (plantDef.plant.wildTerrainTags != null && plantDef.plant.wildTerrainTags.Count != 0)
        {
            return plantDef.plant.WildTerrainTags;
        }

        var plantPreference = WaterPlantPreference.Freshwater;
        if (plantDef.HasModExtension<PlantPreferenceModExtension>())
            plantPreference = plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference;

        return plantPreference switch
        {
            WaterPlantPreference.Freshwater => freshwaterTags,
            WaterPlantPreference.Saltwater => saltwaterTags,
            WaterPlantPreference.Either => eitherTags,
            // ReSharper disable once RedundantSwitchExpressionArms -
            // reminder that this value can theoretically mean something different from default
            WaterPlantPreference.Neither => null,
            _ => null
        };
    }

    //should I also make a check for polluted water?
    public static void WarnIfPreferenceMismatch(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        if (plantDef.HasModExtension<PlantPreferenceModExtension>() &&
            plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference ==
            WaterPlantPreference.Either) return;

        var plantTags = CreateTagsForPlant(plantDef);

        foreach (IntVec3 cell in settable.Cells)
        {
            if (!plantTags.Overlaps(cell.GetTerrain(settable.Map).tags.OrElseEmptyEnumerable()))
            {
                Messages.Message("RT_Saltwater_WarnPreferenceMismatch".Translate(plantDef.label),
                    MessageTypeDefOf.CautionInput, false);
                return;
            }
        }
    }
}