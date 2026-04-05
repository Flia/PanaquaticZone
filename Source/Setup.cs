using System.Collections.Generic;
using System.Linq;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public static class Setup
{
    public static readonly bool allowFreshwaterForZone;
    public static readonly bool allowSaltwaterForZone;
    public static readonly ThingDef defaultFreshwaterPlant;
    public static readonly ThingDef defaultSaltwaterPlant;
    public static readonly string freshwaterTilesStatDisplayCache;
    public static readonly string saltwaterTilesStatDisplayCache;

    static Setup()
    {
        List<TerrainDef> allWaterTiles = DefDatabase<TerrainDef>.AllDefs.Where(def => def.IsWater && def.passability != Traversability.Impassable).ToList();
        List<ThingDef> allPlantDefsWithExtension = DefDatabase<ThingDef>.AllDefs.Where(def => def.HasModExtension<ModExtension_PlantSalinityPreference>()).ToList();
        TagTerrain(DefDatabase<TerrainDef>.AllDefsListForReading);
        TagPlants(allPlantDefsWithExtension);
        freshwaterTilesStatDisplayCache = CacheWaterTerrainForStatDisplay(allWaterTiles, WaterBodyType.Freshwater);
        saltwaterTilesStatDisplayCache = CacheWaterTerrainForStatDisplay(allWaterTiles, WaterBodyType.Saltwater);

        WaterPlantPreference[] freshWaterPreference = [WaterPlantPreference.Freshwater, WaterPlantPreference.Euryhaline];
        WaterPlantPreference[] saltWaterPreference = [WaterPlantPreference.Saltwater, WaterPlantPreference.Euryhaline];

        defaultFreshwaterPlant = allPlantDefsWithExtension.FirstOrDefault(plantDef =>
            freshWaterPreference.Contains(plantDef.getWaterPlantPreference()));
        defaultSaltwaterPlant = allPlantDefsWithExtension.FirstOrDefault(plantDef =>
            saltWaterPreference.Contains(plantDef.getWaterPlantPreference()));

        allowFreshwaterForZone = defaultFreshwaterPlant != null;
        allowSaltwaterForZone = defaultSaltwaterPlant != null;
    }

    private static void TagTerrain(List<TerrainDef> tiles)
    {
        foreach (TerrainDef terrainDef in tiles)
        {
            switch (terrainDef.waterBodyType)
            {
                case WaterBodyType.Freshwater:
                    terrainDef.tags.Add("Panaquatic_freshwater_terrain_tag");
                    continue;
                case WaterBodyType.Saltwater:
                    terrainDef.tags.Add("Panaquatic_saltwater_terrain_tag");
                    continue;
            }
            
            terrainDef.tags ??= [];
            terrainDef.tags.Add("Panaquatic_outside-zone_terrain_tag");
        }
    }

    private static string CacheWaterTerrainForStatDisplay(List<TerrainDef> allWaterTiles, WaterBodyType waterBodyType)
    {
        return allWaterTiles
            .Where(terrainDef => terrainDef.waterBodyType == waterBodyType)
            .Select(x => x.label)
            .Distinct()
            .ToLineList("- ");
    }
    
    //Gives plants with my modExtension the sowTag and terrainTags
    private static void TagPlants(List<ThingDef> allPlantDefsWithExtension) {
        foreach (ThingDef plantDef in allPlantDefsWithExtension)
        {
            //ignore plants with wildTerrainTags, they don't work correctly with growing zones
            if (plantDef.plant.wildTerrainTags is { Count: > 0 })
            {
                Log.Warning("Found mod extension on a plant with wild terrain tags: " + plantDef.defName);
                continue;
            }
            
            plantDef.plant.sowTags.Add("Panaquatic_Zone");
            
            //I'm guessing it's a new 1.6 stat, since older mods are missing it.
            //Removes Fertility Requirement & Fertility Sensitivity from stat display, which for all of those plants is 0% anyway
            plantDef.plant.completelyIgnoreFertility = true; 
            
            HashSet<string> tagsToAdd = plantDef.GetModExtension<ModExtension_PlantSalinityPreference>().plantPreference switch
            {
                WaterPlantPreference.Freshwater =>
                    ["Panaquatic_freshwater_terrain_tag", "Panaquatic_outside-zone_terrain_tag"],
                WaterPlantPreference.Saltwater =>
                    ["Panaquatic_saltwater_terrain_tag", "Panaquatic_outside-zone_terrain_tag"],
                WaterPlantPreference.Euryhaline =>
                    ["Panaquatic_freshwater_terrain_tag", "Panaquatic_saltwater_terrain_tag", "Panaquatic_outside-zone_terrain_tag"],
                _ =>
                    []
            };
            plantDef.plant.WildTerrainTags.AddRange(tagsToAdd);
        }
    }
}