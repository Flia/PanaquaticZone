using UnityEngine;
using Verse;

namespace PanaquaticZone;

public class Panaquatic_Settings : ModSettings
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
        listingStandard.CheckboxLabeled("Panaquatic_MarineAgricultureLabel".Translate(), ref MarineAgriculture,
            "Panaquatic_MarineAgricultureTooltip".Translate());
        listingStandard.CheckboxLabeled("Panaquatic_IndustrialRunoffTooLabel".Translate(), ref IndustrialRunoffToo,
            "Panaquatic_IndustrialRunoffTooTooltip".Translate());
        listingStandard.End();
    }
}

public class Panaquatic : Mod
{
    private readonly Panaquatic_Settings settings;

    public Panaquatic(ModContentPack content) : base(content)
    {
        settings = GetSettings<Panaquatic_Settings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        Panaquatic_Settings.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Panaquatic_Settings".Translate();
    }
}