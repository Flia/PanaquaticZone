using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace PanaquaticZone;

public class Zone_Panaquatic : Zone_Growing
{
    private static ThingDef defaultplant => SetDefaultPlant();
    protected override Color NextZoneColor => PanaquaticZoneColorUtility.NextPanaquaticZoneColor();
    
    //okay it IS needed but how, hell if I know
    public Zone_Panaquatic()
    {
    }

    public Zone_Panaquatic(ZoneManager zoneManager) : base(zoneManager)
    {
        label = "Panaquatic_PanaquaticZone".Translate();
        SetPlantDefToGrow(defaultplant);
    }

    private static ThingDef SetDefaultPlant()
    {
        if (ModsConfig.IsActive("Arquebus.StagzMerfolk"))
        {
            return DefDatabase<ThingDef>.GetNamed("Stagz_DarkAlgae");
        }
        if (ModsConfig.IsActive("VanillaExpanded.VPlantsEMore"))
        {
            return DefDatabase<ThingDef>.GetNamed("VCE_Taro");
        }
        if (ModsConfig.IsActive("LimeTreeSnake.Biosphere"))
        {
            return DefDatabase<ThingDef>.GetNamed("LTS_Plant_RedRice");
        }
        return null; //TODO: maybe should throw an error or something
    }
    
    public override IEnumerable<Gizmo> GetZoneAddGizmos()
    {
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Panaquatic_Expand>();
    }
}