using System.Collections.Generic;
using System.Linq;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public class GivePlantTags
{
    static GivePlantTags()
    {
        //adds a terrain tag corresponding to terrainDef's waterbody type, since plants can't use waterbody directly
        foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs.Where(def => def.tags != null && def.tags.Contains("Water")))
        {
            if (terrainDef.waterBodyType == WaterBodyType.Freshwater)
            {
                terrainDef.tags.Add("Panaquatic_freshwater_terrain_tag");
            } else if (terrainDef.waterBodyType == WaterBodyType.Saltwater)
            {
                terrainDef.tags.Add("Panaquatic_saltwater_terrain_tag");
            }
        }
        
        //now for plants...
        foreach (ThingDef plantDef in DefDatabase<ThingDef>.AllDefs.Where(def => def.category == ThingCategory.Plant))
        {
            //skips over all plants which never had any aquatic agriculture tags in the first place
            if (!plantDef.plant.sowTags.Contains("Panaquatic_Zone") &&
                !plantDef.plant.sowTags.Contains("Water") &&
                !plantDef.plant.sowTags.Contains("VCE_Aquatic")) continue;
            
            //adds my sowTag because it simplifies checks
            if (!plantDef.plant.sowTags.Contains("Panaquatic_Zone")) plantDef.plant.sowTags.Add("Panaquatic_Zone");
            
            //if plant has wild tags defined I don't mess with them;
            //plants with no wild tags are cultivars so there should be no danger
            if (plantDef.plant.wildTerrainTags == null || plantDef.plant.wildTerrainTags.Count == 0)
            {
                if (!plantDef.HasModExtension<PlantPreferenceModExtension>())
                {
                    plantDef.plant.WildTerrainTags.Add("Panaquatic_freshwater_terrain_tag");
                    continue;
                }

                HashSet<string> tagsToAdd = plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference switch
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
}