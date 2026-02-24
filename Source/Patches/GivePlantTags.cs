using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public class GivePlantTags
{
    static GivePlantTags()
    {
        //adds a terrain tag corresponding to terrainDef's waterbody type, since plants don't use waterbody directly
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
            
            //adds my sowTag if absent because it simplifies checks
            if (!plantDef.plant.sowTags.Contains("Panaquatic_Zone")) plantDef.plant.sowTags.Add("Panaquatic_Zone");
            
            //if plant has wild tags defined it gets WildTagged but otherwise isn't messed with
            /*Log.Message("Found water plant: " + plantDef.defName);
            if (plantDef.plant.wildTerrainTags != null)
            {
                if (plantDef.HasModExtension<PlantSalinityPreference>())
                {
                    plantDef.GetModExtension<PlantSalinityPreference>().plantPreference = WaterPlantPreference.WildTagged;
                }
                else
                {
                    plantDef.modExtensions.Add(new PlantSalinityPreference(WaterPlantPreference.WildTagged));
                }
                Log.Message("given wild tag");
                continue;
            }*/
            
            //plants with no wild tags (cultivars) get appropriate terrain tags
            if (!plantDef.HasModExtension<PlantSalinityPreference>())
            {
                plantDef.modExtensions.Add(new PlantSalinityPreference(WaterPlantPreference.Freshwater));
                Log.Message("given automatic freshwater tag");
            }

            HashSet<string> tagsToAdd = plantDef.GetModExtension<PlantSalinityPreference>().plantPreference switch
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
            Log.Message(plantDef.GetModExtension<PlantSalinityPreference>().plantPreference.ToString());
            plantDef.plant.WildTerrainTags.AddRange(tagsToAdd);
        }
    }
}