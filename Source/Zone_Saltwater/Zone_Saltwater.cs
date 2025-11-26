using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class Zone_Saltwater : Zone_Growing
{
    private static ThingDef defaultplant => SetDefaultPlant();
    protected override Color NextZoneColor => SaltwaterZoneColorUtility.NextSaltwaterZoneColor();
    
    //okay it IS needed but how, hell if I know
    public Zone_Saltwater()
    {
    }

    public Zone_Saltwater(ZoneManager zoneManager) : base(zoneManager)
    {
        label = "RT_Saltwater_SaltwaterZone".Translate();
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
        yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Saltwater_Expand>();
    }
}