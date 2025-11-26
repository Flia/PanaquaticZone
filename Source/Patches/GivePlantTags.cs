using System.Linq;
using Verse;

namespace RT_Saltwater;

[StaticConstructorOnStartup]
public class GivePlantTags
{
    static GivePlantTags()
    {
        foreach (ThingDef plantDef in DefDatabase<ThingDef>.AllDefs.Where(def => def.category == ThingCategory.Plant))
        {
            if (!plantDef.plant.sowTags.Contains("RT_Saltwater") &&
                !plantDef.plant.sowTags.Contains("Water") &&
                !plantDef.plant.sowTags.Contains("VCE_Aquatic")) continue;
            
            if (!plantDef.plant.sowTags.Contains("RT_Saltwater")) plantDef.plant.sowTags.Add("RT_Saltwater");
            
            if (plantDef.plant.wildTerrainTags == null || plantDef.plant.wildTerrainTags.Count == 0)
            {
                if (!plantDef.HasModExtension<PlantPreferenceModExtension>())
                {
                    plantDef.plant.WildTerrainTags.AddRange(SaltwaterUtility.freshwaterTags);
                    continue;
                }
                var tagsToAdd = plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference switch
                {
                    WaterPlantPreference.Freshwater =>
                        SaltwaterUtility.freshwaterTags,
                    WaterPlantPreference.Saltwater =>
                        SaltwaterUtility.saltwaterTags,
                    WaterPlantPreference.Either =>
                        SaltwaterUtility.eitherTags,
                    _ =>
                        SaltwaterUtility.freshwaterTags
                };
                plantDef.plant.WildTerrainTags.AddRange(tagsToAdd);
            }
        }
    }
}