using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace PanaquaticZone;

//Designator_ZoneAdd_Fishing has some pretty convoluted code for cell acceptance. I wonder why?
public class Designator_ZoneAdd_Panaquatic : Designator_ZoneAdd
{
    protected override string NewZoneLabel => "Panaquatic_PanaquaticZone".Translate();
    public Designator_ZoneAdd_Panaquatic()
    {
        zoneTypeToPlace = typeof(Zone_Panaquatic);
        defaultLabel = "Panaquatic_PanaquaticZone".Translate();
        defaultDesc = "Panaquatic_PanaquaticZone_description".Translate();
        icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Panaquatic");
        tutorTag = "ZoneAdd_Growing";
    }
    protected override Zone MakeNewZone()
    {
        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
        return new Zone_Panaquatic(Find.CurrentMap.zoneManager);
    }

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        if (!base.CanDesignateCell(c).Accepted) return false;
        if (c.GetTerrain(Map).passability == Traversability.Impassable) return false;
        if (c.IsPolluted(Map)) return false;
        if (c.GetWaterBodyType(Map) == WaterBodyType.Freshwater && PanaquaticStartupTasks.AllowFreshwaterForZone) return true;
        return c.GetWaterBodyType(Map) == WaterBodyType.Saltwater && PanaquaticStartupTasks.AllowSaltwaterForZone;
    }

    public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
    {
        base.DesignateMultiCell(cells);
        if (Find.Selector.SelectedZone is Zone_Panaquatic zone)
        {
            PanaquaticUtility.WarnIfPreferenceMismatch(zone.GetPlantDefToGrow(), zone);
        }
    }
}