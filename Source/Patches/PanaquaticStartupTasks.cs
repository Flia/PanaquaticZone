using System.Collections.Generic;
using System.Linq;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public static class PanaquaticStartupTasks
{
    static PanaquaticStartupTasks()
    {
        TagTerrain();
        TagPlants();
    }

    //Adds a terrain tag corresponding to terrainDef's waterbody type, since plants don't use waterbody directly;
    //Also caches data used for SpecialDisplayStats description
    private static void TagTerrain()
    {
        HashSet<string> freshwaterTiles = [];
        HashSet<string> saltwaterTiles = [];
        foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs.Where(def => def.IsWater))
        {
            if (terrainDef.passability != Traversability.Impassable)
                if (terrainDef.waterBodyType == WaterBodyType.Freshwater) 
                {
                    terrainDef.tags.Add("Panaquatic_freshwater_terrain_tag");
                    freshwaterTiles.Add(terrainDef.label);
                    PanaquaticUtility.allAcceptableWaterTilesTracker.Add(terrainDef);
                }
                else if (terrainDef.waterBodyType == WaterBodyType.Saltwater)
                {
                    terrainDef.tags.Add("Panaquatic_saltwater_terrain_tag");
                    saltwaterTiles.Add(terrainDef.label);
                    PanaquaticUtility.allAcceptableWaterTilesTracker.Add(terrainDef);
                }
        }
        PanaquaticUtility.freshwaterTilesStatDisplayCache = freshwaterTiles.ToCommaList(true);
        PanaquaticUtility.saltwaterTilesStatDisplayCache = saltwaterTiles.ToCommaList(true);
    }
    
    //Gives all plants with my modExtension the sowTag and terrainTags
    //Also caches SpecialDisplayStats strings for WildTagged plants
    private static void TagPlants() {
        foreach (ThingDef plantDef in DefDatabase<ThingDef>.AllDefs.Where(def => def.HasModExtension<ModExtension_PlantSalinityPreference>()))
        {
            plantDef.plant.sowTags.Add("Panaquatic_Zone");

            if (plantDef.plant.wildTerrainTags != null && plantDef.plant.wildTerrainTags.Count > 0)
            {
                Log.Warning("Found mod extension on a plant with wild terrain tags: " + plantDef.defName);
                HashSet<string> terrainDefsForPlant = [];
                foreach (TerrainDef terrain in PanaquaticUtility.allAcceptableWaterTilesTracker)
                {
                    if (plantDef.plant.WildTerrainTags.Overlaps(terrain.tags.OrElseEmptyEnumerable()))
                        terrainDefsForPlant.Add(terrain.label);
                } 
                PanaquaticUtility.wildTaggedTilesCacheDictionary.Add(plantDef.plant, terrainDefsForPlant.ToCommaList(true));
                continue;
            }
            
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