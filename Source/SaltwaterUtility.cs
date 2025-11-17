using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public static class SaltwaterUtility
{
    public static readonly HashSet<string> saltwaterTags =
        ["Ocean"];
    public static readonly HashSet<string> freshwaterTags =
        ["River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    public static readonly HashSet<string> eitherTags =
        ["Ocean", "River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    
    //A heavily edited variant of vanilla PollutionUtility func
    public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        var plantTags = plantDef.plant.WildTerrainTags;

        if (plantTags == null) return false;

        return settable.Cells.Any(cell =>
            plantTags.Overlaps(cell.GetTerrain(settable.Map).tags.OrElseEmptyEnumerable()));
    }

    //should I also make a check for polluted water?
    public static void WarnIfPreferenceMismatch(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        if (plantDef.HasModExtension<PlantPreferenceModExtension>() &&
            plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference ==
            WaterPlantPreference.Either) return;

        var plantTags = plantDef.plant.WildTerrainTags;

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