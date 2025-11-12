using RimWorld;
using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class Designator_ZoneAdd_Saltwater : Designator_ZoneAdd
{
    protected override string NewZoneLabel => "RTMerrenPatches_SaltwaterZone".Translate();
    public Designator_ZoneAdd_Saltwater()
    {
        zoneTypeToPlace = typeof(Zone_Saltwater);
        defaultLabel = "RTMerrenPatches_SaltwaterZone".Translate();
        defaultDesc = "RTMerrenPatches_SaltwaterZone_description".Translate();
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
        return base.CanDesignateCell(c).Accepted && c.GetTerrain(Map).IsWater;
    }
}