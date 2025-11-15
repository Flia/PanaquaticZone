using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public static class SaltwaterUtility //A variant of vanilla PollutionUtility func
{
    private static readonly List<string> waterTags =
        ["Ocean", "River", "WaterFreshShallow", "WaterFreshShallowStill", "WaterFreshShallowMoving", "WaterMarshy"];

    public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        //if plant has no tags it's probably defined as cultivar only
        if (plantDef.plant.wildTerrainTags == null || plantDef.plant.wildTerrainTags.Count == 0)
        {
            return true;
        }
        //but in case it DOES have tags...
        foreach (var cell in settable.Cells)
        {
            foreach (var tag in waterTags)
            {
                if (cell.GetTerrain(settable.Map).HasTag(tag))
                    return true;
            } 
        }
        return false;
    }
}