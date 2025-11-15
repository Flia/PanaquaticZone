using System.Linq;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public static class SaltwaterUtility
{
    //A variant of vanilla PollutionUtility func
    public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
    {
        //if plant has no wild tags it's probably defined as cultivar only
        if (plantDef.plant.wildTerrainTags == null || plantDef.plant.wildTerrainTags.Count == 0)
        {
            return true;
        }
        //but in case it DOES have tags:
        //compare each cell's tags against each of plant's tags, return true if any cell has matching tag
        return (from cell in settable.Cells
            from terrainTag in cell.GetTerrain(settable.Map).tags
            from wildTag in plantDef.plant.wildTerrainTags
            where terrainTag == wildTag
            select terrainTag).Any();
    }
}