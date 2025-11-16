using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class Zone_Saltwater : Zone_Growing
{
    protected override Color NextZoneColor => SaltwaterZoneColorUtility.NextSaltwaterZoneColor();
    
    public Zone_Saltwater() //okay it IS needed but how, hell if I know
    {
    }

    public Zone_Saltwater(ZoneManager zoneManager) : base(zoneManager)
    {
        label = "RT_Saltwater_SaltwaterZone".Translate();
        SetPlantDefToGrow(DefDatabase<ThingDef>.GetNamed("RT_Swiftcoral"));
    }
    
    public override IEnumerable<Gizmo> GetZoneAddGizmos()
    {
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Saltwater_Expand>();
    }
}