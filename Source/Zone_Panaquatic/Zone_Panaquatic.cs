using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace PanaquaticZone;

public class Zone_Panaquatic : Zone_Growing
{
    protected override Color NextZoneColor => PanaquaticZoneColorUtility.NextPanaquaticZoneColor();
    
    //credit to Mashed's Ashlands for property/addcell block lmao, spent whole day on it before I looked at their code
    //I still don't understand *precisely* why it works.
    public new ThingDef PlantDefToGrow 
    { 
        get 
        { 
            if (base.PlantDefToGrow == ThingDefOf.Plant_Potato
                || ModsConfig.BiotechActive && base.PlantDefToGrow == ThingDefOf.Plant_Toxipotato) 
            {
                SetPlantDefToGrow(GetDefaultPlant());
            }
            return base.PlantDefToGrow; 
        }
    }
    public override void AddCell(IntVec3 c)
    {
        base.AddCell(c);
        //"done here because otherwise SettableEntirelyPolluted returns true due to Cells being empty"
        _ = PlantDefToGrow;
    }
    
    //okay it IS needed but how, hell if I know
    public Zone_Panaquatic()
    {
    }

    public Zone_Panaquatic(ZoneManager zoneManager) : base(zoneManager)
    {
        label = "Panaquatic_PanaquaticZone".Translate();
    }

    private ThingDef GetDefaultPlant()
    {
        return !this.SettableEntirelySaltwater()
            ? Setup.defaultFreshwaterPlant
            : Setup.defaultSaltwaterPlant;
    }
    
    public override IEnumerable<Gizmo> GetZoneAddGizmos()
    {
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Panaquatic_Expand>();
    }
}