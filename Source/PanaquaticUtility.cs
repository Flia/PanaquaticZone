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

    public static void WarnIfPreferenceMismatch(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        if (plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline) return;

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

    public static WaterPlantPreference getWaterPlantPreference(this ThingDef plantDef)
    {
        return plantDef.plant.getWaterPlantPreference();
    }

    public static WaterPlantPreference getWaterPlantPreference(this PlantProperties plant)
    {
        if (!plant.sowTags.Contains("Panaquatic_Zone"))
            return WaterPlantPreference.None;
        if (plant.WildTerrainTags.Contains("Panaquatic_freshwater_terrain_tag") &&
            plant.WildTerrainTags.Contains("Panaquatic_saltwater_terrain_tag"))
            return WaterPlantPreference.Euryhaline;
        if (plant.WildTerrainTags.Contains("Panaquatic_freshwater_terrain_tag"))
            return WaterPlantPreference.Freshwater;
        if (plant.WildTerrainTags.Contains("Panaquatic_saltwater_terrain_tag"))
            return WaterPlantPreference.Saltwater;
        if (plant.WildTerrainTags != null && plant.WildTerrainTags.Count > 0)
            return WaterPlantPreference.WildTagged;
        return WaterPlantPreference.None;
    }
    
    public static bool SettableEntirelySaltwater(this IPlantToGrowSettable s)
    {
        foreach (IntVec3 cell in s.Cells)
        {
            if (cell.GetWaterBodyType(s.Map) == WaterBodyType.Freshwater)
            {
                return false;
            }
        }
        return true;
    }
}

public enum WaterPlantPreference
{
    Freshwater = 0,
    Saltwater,
    Euryhaline,
    WildTagged,
    None
}