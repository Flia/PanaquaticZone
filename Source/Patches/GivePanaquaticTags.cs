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
    }
}