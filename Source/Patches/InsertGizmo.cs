/*using RimWorld;
using Verse;

namespace RT_Saltwater;

[StaticConstructorOnStartup]
public class InsertGizmo
{
    static InsertGizmo()
    {
        var i = DesignationCategoryDefOf.Zone.specialDesignatorClasses.IndexOf(typeof(Designator_ZoneAdd_Growing));
        DesignationCategoryDefOf.Zone.specialDesignatorClasses.Add(typeof(Designator_ZoneAdd_Saltwater));
        foreach (var designator in DesignationCategoryDefOf.Zone.specialDesignatorClasses)
        {
            Log.Message(designator.ToString());
        }
    }
}*/