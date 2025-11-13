using RimWorld;
using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class Designator_ZoneAdd_Saltwater : Designator_ZoneAdd
{
    protected override string NewZoneLabel => "RT_Saltwater_SaltwaterZone".Translate();
    public Designator_ZoneAdd_Saltwater()
    {
        zoneTypeToPlace = typeof(Zone_Saltwater);
        defaultLabel = "RT_Saltwater_SaltwaterZone".Translate();
        defaultDesc = "RT_Saltwater_SaltwaterZone_description".Translate();
        icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing");
        tutorTag = "ZoneAdd_Growing";
    }
    protected override Zone MakeNewZone()
    {
        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
        return new Zone_Saltwater(Find.CurrentMap.zoneManager);
    }

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        if (RT_Saltwater_Settings.NoSalt)
            return base.CanDesignateCell(c).Accepted && c.GetTerrain(Map).IsWater && !c.GetTerrain(Map).IsOcean;
        return base.CanDesignateCell(c).Accepted && c.GetTerrain(Map).IsWater;
    }
}