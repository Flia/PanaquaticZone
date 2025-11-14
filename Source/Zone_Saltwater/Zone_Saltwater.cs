using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class Zone_Saltwater : Zone_Growing
{
    private ThingDef plantDefToGrow;
    protected override Color NextZoneColor => SaltwaterZoneColorUtility.NextSaltwaterZoneColor();
    
    public Zone_Saltwater() //I really wish I knew why this empty constructor is needed
    {
    }

    public Zone_Saltwater(ZoneManager zoneManager)
        : base(zoneManager)
    {
        label = "RT_Saltwater_SaltwaterZone".Translate();
        SetPlantDefToGrow(DefDatabase<ThingDef>.GetNamed("RT_Swiftcoral"));
    }
    
    public override IEnumerable<Gizmo> GetZoneAddGizmos()
    {
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Saltwater_Expand>();
    }
    
    public new ThingDef PlantDefToGrow
    {
        get
        {
            if (plantDefToGrow != null)
                return plantDefToGrow;
            plantDefToGrow = !PollutionUtility.SettableEntirelyPolluted((IPlantToGrowSettable) this) ? ThingDefOf.Plant_Potato : ThingDefOf.Plant_Toxipotato;
            return plantDefToGrow;
        }
    }
}