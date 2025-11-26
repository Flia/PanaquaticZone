using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RT_Saltwater;

//Designator_ZoneAdd_Fishing has some pretty convoluted code for cell acceptance. I wonder why?
public class Designator_ZoneAdd_Saltwater : Designator_ZoneAdd
{
    protected override string NewZoneLabel => "RT_Saltwater_SaltwaterZone".Translate();
    public Designator_ZoneAdd_Saltwater()
    {
        zoneTypeToPlace = typeof(Zone_Saltwater);
        defaultLabel = "RT_Saltwater_SaltwaterZone".Translate();
        defaultDesc = "RT_Saltwater_SaltwaterZone_description".Translate();
        icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_RT_Saltwater");
        tutorTag = "ZoneAdd_Growing";
    }
    protected override Zone MakeNewZone()
    {
        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
        return new Zone_Saltwater(Find.CurrentMap.zoneManager);
    }

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        if (!base.CanDesignateCell(c).Accepted || c.GetTerrain(Map).passability == Traversability.Impassable) return false;
        if (!RT_Saltwater_Settings.IndustrialRunoffToo && c.IsPolluted(Map)) return false;
        if (c.GetWaterBodyType(Map) == WaterBodyType.Freshwater) return true;
        return c.GetWaterBodyType(Map) == WaterBodyType.Saltwater && RT_Saltwater_Settings.MarineAgriculture;
    }

    public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
    {
        base.DesignateMultiCell(cells);
        if (Find.Selector.SelectedZone is Zone_Saltwater zone)
        {
            SaltwaterUtility.WarnIfPreferenceMismatch(zone.GetPlantDefToGrow(), zone);
        }
    }
}