using RimWorld;
using Verse;

namespace RT_Saltwater;

[StaticConstructorOnStartup]
public class PlantPatches
{
    static PlantPatches()
    {
        //not sure why but it's just never sown
        //DefDatabase<ThingDef>.GetNamed("Plant_TreeCypress").plant.sowTags.Add("RT_Saltwater");
    }
    
}