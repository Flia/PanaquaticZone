using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public static class SaltwaterUtility
{
    //Those tags are from basegame. I just might return to having static constant at some point...
    public static readonly HashSet<string> saltwaterTags =
        ["Ocean"];
    public static readonly HashSet<string> freshwaterTags =
        ["River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    public static readonly HashSet<string> eitherTags =
        ["Ocean", "River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];

    static SaltwaterUtility()
    {
        List<TerrainDef> terrainDefs = DefDatabase<TerrainDef>.AllDefs.Where(def => def.waterBodyType == WaterBodyType.Saltwater).ToList();
        string message = "";
        foreach (TerrainDef terrainDef in terrainDefs)
        {
            message = message + ", " + terrainDef.label;
            saltwaterTags.AddRange(terrainDef.tags);
        }
        saltwaterTags.RemoveWhere(tag => tag == "Water");
        // Log.Message("Saltwater tags count: " + saltwaterTags.Count);
        // Log.Message("Terrains: " + message);
        // message = saltwaterTags.Aggregate("", (current, tag) => current + ", " + tag);
        // Log.Message("Tags: " + message);

        terrainDefs = DefDatabase<TerrainDef>.AllDefs.Where(def => def.waterBodyType == WaterBodyType.Freshwater).ToList();
        message = "";
        foreach (TerrainDef terrainDef in terrainDefs)
        {
            message = message + ", " + terrainDef.label;
            freshwaterTags.AddRange(terrainDef.tags);
        }
        freshwaterTags.RemoveWhere(tag => tag == "Water");
        // Log.Message("Freshwater tags count: " + freshwaterTags.Count);
        // Log.Message("Terrains: " + message);
        // message = freshwaterTags.Aggregate("", (current, tag) => current + ", " + tag);
        // Log.Message("Tags: " + message);
        
        eitherTags.AddRange(saltwaterTags);
        eitherTags.AddRange(freshwaterTags);
        // Log.Message("Either tags count: " + eitherTags.Count);
        // message = eitherTags.Aggregate("", (current, tag) => current + ", " + tag);
        // Log.Message("Tags: " + message);
    }

    //A heavily edited variant of vanilla PollutionUtility func
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
        if (plantDef.HasModExtension<PlantPreferenceModExtension>() &&
            plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference ==
            WaterPlantPreference.Either) return;

        var plantTags = plantDef.plant.WildTerrainTags;

        foreach (IntVec3 cell in settable.Cells)
        {
            if (!plantTags.Overlaps(cell.GetTerrain(settable.Map).tags.OrElseEmptyEnumerable()))
            {
                Messages.Message("RT_Saltwater_WarnPreferenceMismatch".Translate(plantDef.label),
                    MessageTypeDefOf.RejectInput, false);
                return;
            }
        }
    }
}