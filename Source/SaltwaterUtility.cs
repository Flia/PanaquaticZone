using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public static class SaltwaterUtility
{
    private static readonly List<string> saltwaterTags =
        ["Ocean"];
    private static readonly List<string> freshwaterTags =
        ["River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    private static readonly List<string> eitherTags =
        ["Ocean", "River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];
    
    //A heavily edited variant of vanilla PollutionUtility func
    public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        //if plant has wildTags, use them, otherwise get from ModExtension, otherwise use Freshwater set
        //NB: plants without those tags are *usually* cultivars-only
        List<string> plantTags;
        if (plantDef.plant.wildTerrainTags != null && plantDef.plant.wildTerrainTags.Count != 0)
            plantTags = plantDef.plant.wildTerrainTags;
        else plantTags = CreateTagsFromPlantPreferenceModExtension(plantDef);

        if (plantTags == null) return false;
        
        //compare each cell's tags against each of plant's tags, return true if any cell has matching tag
        return (from cell in settable.Cells
            from terrainTag in cell.GetTerrain(settable.Map).tags
            from plantTag in plantTags
            where terrainTag == plantTag
            select terrainTag).Any();
    }
    
    public static List<string> CreateTagsFromPlantPreferenceModExtension(ThingDef plantDef)
    {
        var plantPreference = WaterPlantPreference.Freshwater;
        if (plantDef.HasModExtension<PlantPreferenceModExtension>())
            plantPreference = plantDef.GetModExtension<PlantPreferenceModExtension>().plantPreference;

        return plantPreference switch
        {
            WaterPlantPreference.Freshwater => freshwaterTags,
            WaterPlantPreference.Saltwater => saltwaterTags,
            WaterPlantPreference.Either => eitherTags,
            // ReSharper disable once RedundantSwitchExpressionArms -
            // reminder that this value can theoretically mean something different from default
            WaterPlantPreference.Neither => null,
            _ => null
        };
    }
}