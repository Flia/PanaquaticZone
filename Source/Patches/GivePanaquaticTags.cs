using System.Collections.Generic;
using System.Linq;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public class GivePanaquaticTags
{
    static GivePanaquaticTags()
    {
        //adds a terrain tag corresponding to terrainDef's waterbody type, since plants don't use waterbody directly
        foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs.Where(def => def.IsWater))
        {
            if (terrainDef.waterBodyType == WaterBodyType.Freshwater)
            {
                terrainDef.tags.Add("Panaquatic_freshwater_terrain_tag");
            } else if (terrainDef.waterBodyType == WaterBodyType.Saltwater)
            {
                terrainDef.tags.Add("Panaquatic_saltwater_terrain_tag");
            }
        }
        
        //gives all plants with my modExtension the sowTag and terrainTags
        foreach (ThingDef plantDef in DefDatabase<ThingDef>.AllDefs.Where(def => def.HasModExtension<ModExtension_PlantSalinityPreference>()))
        {
            plantDef.plant.sowTags.Add("Panaquatic_Zone");

            if (plantDef.plant.wildTerrainTags != null) continue;
            
            HashSet<string> tagsToAdd = plantDef.GetModExtension<ModExtension_PlantSalinityPreference>().plantPreference switch
            {
                WaterPlantPreference.Freshwater =>
                    ["Panaquatic_freshwater_terrain_tag"],
                WaterPlantPreference.Saltwater =>
                    ["Panaquatic_saltwater_terrain_tag"],
                WaterPlantPreference.Either =>
                    ["Panaquatic_freshwater_terrain_tag", "Panaquatic_saltwater_terrain_tag"],
                _ =>
                    ["Panaquatic_freshwater_terrain_tag"]
            };
            plantDef.plant.WildTerrainTags.AddRange(tagsToAdd);
        }
    }
}