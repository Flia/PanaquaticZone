using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PanaquaticZone;

public static class PanaquaticUtility
{
    public static string freshwaterTilesStatDisplayCache;
    public static string saltwaterTilesStatDisplayCache;
    public static readonly HashSet<TerrainDef> allAcceptableWaterTilesTracker = [];
    public static readonly Dictionary<PlantProperties, string> wildTaggedTilesCacheDictionary = [];

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
        if (getWaterPlantPreference(plantDef) == WaterPlantPreference.Euryhaline) return;

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

    public static WaterPlantPreference getWaterPlantPreference(ThingDef plantDef)
    {
        return getWaterPlantPreference(plantDef.plant);
    }

    public static WaterPlantPreference getWaterPlantPreference(PlantProperties plant)
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
}

public enum WaterPlantPreference
{
    Freshwater = 0,
    Saltwater,
    Euryhaline,
    WildTagged,
    None
}