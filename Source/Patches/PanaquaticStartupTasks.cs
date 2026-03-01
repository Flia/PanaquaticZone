using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public static class PanaquaticStartupTasks
{
    public static readonly ThingDef defaultFreshwaterPlant;
    public static readonly ThingDef defaultSaltwaterPlant;
    private static readonly bool allowSaltwaterForZone;
    private static readonly bool allowFreshwaterForZone;
    public static string freshwaterTilesStatDisplayCache;
    public static string saltwaterTilesStatDisplayCache;
    public static readonly HashSet<TerrainDef> allAcceptableWaterTilesTracker = [];
    public static readonly Dictionary<PlantProperties, string> wildTaggedTilesCacheDictionary = [];

    public static bool AllowSaltwaterForZone => allowSaltwaterForZone;
    public static bool AllowFreshwaterForZone => allowFreshwaterForZone;

    static PanaquaticStartupTasks()
    {
        List<TerrainDef> allWaterTiles = DefDatabase<TerrainDef>.AllDefs.Where(def => def.IsWater && def.passability != Traversability.Impassable).ToList();
        List<ThingDef> allPlantDefsWithExtension = DefDatabase<ThingDef>.AllDefs.Where(def => def.HasModExtension<ModExtension_PlantSalinityPreference>()).ToList();
        TagTerrain(allWaterTiles);
        CacheTerrainForStatDisplay(allWaterTiles);
        TagPlants(allPlantDefsWithExtension, allWaterTiles);
        allowSaltwaterForZone = SetAllowSaltwaterForZone(allPlantDefsWithExtension);
        allowFreshwaterForZone = SetAllowFreshwaterForZone(allPlantDefsWithExtension);
        if (AllowSaltwaterForZone)
            defaultSaltwaterPlant = allPlantDefsWithExtension.Find(plantDef
                => plantDef.getWaterPlantPreference() == WaterPlantPreference.Saltwater
                   || plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline);
        if (AllowFreshwaterForZone)
            defaultFreshwaterPlant = allPlantDefsWithExtension.Find(plantDef
                => plantDef.getWaterPlantPreference() == WaterPlantPreference.Freshwater
                   || plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline);
    }

    private static void TagTerrain(IEnumerable<TerrainDef> allWaterTiles)
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

    private static void CacheTerrainForStatDisplay(IEnumerable<TerrainDef> allWaterTiles)
    {
        HashSet<string> freshwaterTiles = [];
        HashSet<string> saltwaterTiles = [];
        foreach (TerrainDef terrainDef in allWaterTiles)
        { 
            if (terrainDef.waterBodyType == WaterBodyType.Freshwater)
            { 
                freshwaterTiles.Add(terrainDef.label);
            }
            else if (terrainDef.waterBodyType == WaterBodyType.Saltwater)
            {
                saltwaterTiles.Add(terrainDef.label);
            }
        }
        freshwaterTilesStatDisplayCache = freshwaterTiles.ToLineList("- ");
        saltwaterTilesStatDisplayCache = saltwaterTiles.ToLineList("- ");
    }
    
    //Gives all plants with my modExtension the sowTag and terrainTags
    private static void TagPlants(IEnumerable<ThingDef> allPlantDefsWithExtension, IEnumerable<TerrainDef> allWaterTiles) {
        foreach (ThingDef plantDef in allPlantDefsWithExtension)
        {
            plantDef.plant.sowTags.Add("Panaquatic_Zone");

            //Although I handle wild plants too, WildTagged is fallback logic
            foreach (TerrainDef terrainDef in allWaterTiles)
            {
                if (terrainDef.waterBodyType is WaterBodyType.Freshwater or WaterBodyType.Saltwater)
                {
                    allAcceptableWaterTilesTracker.Add(terrainDef);
                }
            }
            if (plantDef.plant.wildTerrainTags != null && plantDef.plant.wildTerrainTags.Count > 0)
            {
                Log.Warning("Found mod extension on a plant with wild terrain tags: " + plantDef.defName);
                HashSet<string> terrainDefsForPlant = [];
                foreach (TerrainDef terrain in allAcceptableWaterTilesTracker)
                {
                    if (plantDef.plant.WildTerrainTags.Overlaps(terrain.tags.OrElseEmptyEnumerable()))
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
    
    private static bool SetAllowSaltwaterForZone(IEnumerable<ThingDef> allPlantDefsWithExtension)
    {
        bool flag = false;
        foreach (ThingDef plantDef in allPlantDefsWithExtension)
        {
            if (plantDef.getWaterPlantPreference() == WaterPlantPreference.Saltwater || plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }
    
    private static bool SetAllowFreshwaterForZone(IEnumerable<ThingDef> allPlantDefsWithExtension)
    {
        bool flag = false;
        foreach (ThingDef plantDef in allPlantDefsWithExtension)
        {
            if (plantDef.getWaterPlantPreference() == WaterPlantPreference.Freshwater || plantDef.getWaterPlantPreference() == WaterPlantPreference.Euryhaline)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }
}