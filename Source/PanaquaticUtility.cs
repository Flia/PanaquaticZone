using System.Linq;
using RimWorld;
using Verse;

namespace PanaquaticZone;

public static class PanaquaticUtility
{
    public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        var plantTags = plantDef.plant.WildTerrainTags;

        if (plantTags == null) return false;

        return settable.Cells.Any(cell =>
            plantTags.Overlaps(cell.GetTerrain(settable.Map).tags.OrElseEmptyEnumerable()));
    }

    //should I also make it check for polluted water?
    public static void WarnIfPreferenceMismatch(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        if (plantDef.HasModExtension<PlantSalinityPreference>() &&
            plantDef.GetModExtension<PlantSalinityPreference>().plantPreference ==
            WaterPlantPreference.Either) return;

        var plantTags = plantDef.plant.WildTerrainTags;

        foreach (IntVec3 cell in settable.Cells)
        {
            if (!plantTags.Overlaps(cell.GetTerrain(settable.Map).tags.OrElseEmptyEnumerable()))
            {
                Messages.Message("Panaquatic_WarnPreferenceMismatch".Translate(plantDef.label),
                    MessageTypeDefOf.RejectInput, false);
                return;
            }
        }
    }
}