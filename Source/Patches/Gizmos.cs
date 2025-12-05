/*using RimWorld;
using Verse;

namespace RT_Saltwater;

[StaticConstructorOnStartup]
public class Gizmos
{
    static Gizmos()
    {
        DefDatabase<DesignationCategoryDef>.GetNamed("Zone").specialDesignatorClasses
            .Add(typeof(Designator_ZoneAdd_Saltwater));
        

        DefDatabase<DesignationCategoryDef>.GetNamed("Zone").specialDesignatorClasses
            .Remove(typeof(Designator_ZoneAdd_Growing));
        //DefDatabase<DesignationCategoryDef>.GetNamed("Zone").specialDesignatorClasses
        //    .Remove(typeof(LTSBiosphere.Zones.Designator_ZoneAdd_Aquatic));
        //DefDatabase<DesignationCategoryDef>.GetNamed("Zone").specialDesignatorClasses
        //    .Remove(typeof(VanillaPlantsExpandedMorePlants.Designator_AquaticGrowingZone)();
        
        foreach (var def in DefDatabase<DesignationCategoryDef>.GetNamed("Zone").specialDesignatorClasses)
        {
            Log.Message(def.FullName);
        }
    }
}*/