using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class RT_Saltwater_Settings : ModSettings
{
    public static bool MarineAgriculture = true;
    public static bool IndustrialRunoffToo = false;
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref MarineAgriculture, "MarineAgricultureSetting");
        Scribe_Values.Look(ref IndustrialRunoffToo, "IndustrialRunoffToo");
        base.ExposeData();
    }

    public static void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("RT_Saltwater_MarineAgricultureLabel".Translate(), ref MarineAgriculture, "RT_Saltwater_MarineAgricultureTooltip".Translate());
        listingStandard.CheckboxLabeled("RT_Saltwater_IndustrialRunoffTooLabel".Translate(), ref IndustrialRunoffToo, "RT_Saltwater_IndustrialRunoffTooTooltip".Translate());
        listingStandard.End();
    }
}

public class RT_Saltwater : Mod
{
    private readonly RT_Saltwater_Settings settings;

    public RT_Saltwater(ModContentPack content) : base(content)
    {
        settings = GetSettings<RT_Saltwater_Settings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        RT_Saltwater_Settings.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "RT_Saltwater_Settings".Translate();
    }
}