using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RT_Saltwater;

public class Zone_Saltwater : Zone_Growing
{
    public Zone_Saltwater() //I really wish I knew why this empty constructor is needed
    {
    }

    public Zone_Saltwater(ZoneManager zoneManager)
        : base(zoneManager)
    {
        label = "RT_Saltwater_SaltwaterZone".Translate();
        if (ModsConfig.IsActive("Arquebus.StagzMerfolk") && DefDatabase<ThingDef>.GetNamed("Stagz_DarkAlgae") != null)
        { 
            SetPlantDefToGrow(DefDatabase<ThingDef>.GetNamed("Stagz_DarkAlgae"));
        }
    }

    public override IEnumerable<Gizmo> GetZoneAddGizmos()
    {
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Saltwater_Expand>();
    }
    
    //Want to implement colors. From base:
    //protected override Color NextZoneColor => ZoneColorUtility.NextGrowingZoneColor();
}