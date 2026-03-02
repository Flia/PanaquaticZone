using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public static class PanaquaticStartupTasks
{
    public static readonly bool allowFreshwaterForZone;
    public static readonly bool allowSaltwaterForZone;
    public static readonly ThingDef defaultFreshwaterPlant;
    public static readonly ThingDef defaultSaltwaterPlant;
    public static readonly string freshwaterTilesStatDisplayCache;
    public static readonly string saltwaterTilesStatDisplayCache;
    public static readonly Dictionary<PlantProperties, string> wildTaggedTilesCacheDictionary = [];

    static PanaquaticStartupTasks()
    {
        List<TerrainDef> allWaterTiles = DefDatabase<TerrainDef>.AllDefs.Where(def => def.IsWater && def.passability != Traversability.Impassable).ToList();
        List<ThingDef> allPlantDefsWithExtension = DefDatabase<ThingDef>.AllDefs.Where(def => def.HasModExtension<ModExtension_PlantSalinityPreference>()).ToList();
        TagTerrain(allWaterTiles);
        TagPlants(allPlantDefsWithExtension, allWaterTiles);
        freshwaterTilesStatDisplayCache = CacheWaterTerrainForStatDisplay(allWaterTiles, WaterBodyType.Freshwater);
        saltwaterTilesStatDisplayCache = CacheWaterTerrainForStatDisplay(allWaterTiles, WaterBodyType.Freshwater);
        allowFreshwaterForZone = Enumerable.Any(allPlantDefsWithExtension,
            plantDef => plantDef.getWaterPlantPreference() == WaterPlantPreference.Freshwater ||
                        plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline);
        allowSaltwaterForZone = Enumerable.Any(allPlantDefsWithExtension,
                    plantDef => plantDef.getWaterPlantPreference() == WaterPlantPreference.Saltwater ||
                                plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline);
        if (allowFreshwaterForZone)
            defaultFreshwaterPlant = allPlantDefsWithExtension.Find(plantDef
                => plantDef.getWaterPlantPreference() == WaterPlantPreference.Freshwater
                   || plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline);
        if (allowSaltwaterForZone)
                    defaultSaltwaterPlant = allPlantDefsWithExtension.Find(plantDef
                        => plantDef.getWaterPlantPreference() == WaterPlantPreference.Saltwater
                           || plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline);
    }

    private static void TagTerrain(List<TerrainDef> allWaterTiles)
    {
        foreach (TerrainDef terrainDef in allWaterTiles)
        {
            if (terrainDef.waterBodyType == WaterBodyType.Freshwater)
            {
                terrainDef.tags.Add("Panaquatic_freshwater_terrain_tag");
            }
            else if (terrainDef.waterBodyType == WaterBodyType.Saltwater)
            {
                terrainDef.tags.Add("Panaquatic_saltwater_terrain_tag");
            }
        }
    }

    private static string CacheWaterTerrainForStatDisplay(List<TerrainDef> allWaterTiles, WaterBodyType waterBodyType)
    {
        HashSet<string> waterTiles = [];
        foreach (TerrainDef terrainDef in allWaterTiles.Where(terrainDef => terrainDef.waterBodyType == waterBodyType))
        {
            waterTiles.Add(terrainDef.label);
        }
        return waterTiles.ToLineList("- ");
    }
    
    //Gives all plants with my modExtension the sowTag and terrainTags
    private static void TagPlants(List<ThingDef> allPlantDefsWithExtension, List<TerrainDef> allWaterTiles) {
        
        //Prep for wildtagged block
        HashSet<TerrainDef> allAcceptableWaterTilesTracker = [];
        foreach (TerrainDef terrainDef in allWaterTiles.Where(terrainDef => terrainDef.waterBodyType is WaterBodyType.Freshwater or WaterBodyType.Saltwater))
        {
            allAcceptableWaterTilesTracker.Add(terrainDef);
        }
        
        foreach (ThingDef plantDef in allPlantDefsWithExtension)
        {
            plantDef.plant.sowTags.Add("Panaquatic_Zone");

            //Although I handle wild plants too, WildTagged is fallback logic
            if (plantDef.plant.wildTerrainTags is { Count: > 0 })
            {
                Log.Warning("Found mod extension on a plant with wild terrain tags: " + plantDef.defName);
                HashSet<string> terrainDefsForPlant = [];
                foreach (TerrainDef terrain in allAcceptableWaterTilesTracker.Where(terrain =>
                             plantDef.plant.WildTerrainTags.Overlaps(terrain.tags.OrElseEmptyEnumerable())))
                {
                    terrainDefsForPlant.Add(terrain.label.CapitalizeFirst());
                } 
                wildTaggedTilesCacheDictionary.Add(plantDef.plant, terrainDefsForPlant.ToLineList("- "));
                continue;
            }
            //end of WildTagged block
            
            HashSet<string> tagsToAdd = plantDef.GetModExtension<ModExtension_PlantSalinityPreference>().plantPreference switch
            {
                WaterPlantPreference.Freshwater =>
                    ["Panaquatic_freshwater_terrain_tag"],
                WaterPlantPreference.Saltwater =>
                    ["Panaquatic_saltwater_terrain_tag"],
                WaterPlantPreference.Euryhaline =>
                    ["Panaquatic_freshwater_terrain_tag", "Panaquatic_saltwater_terrain_tag"],
                _ =>
                    ["Panaquatic_freshwater_terrain_tag"]
            };
            plantDef.plant.WildTerrainTags.AddRange(tagsToAdd);
        }
    }
}