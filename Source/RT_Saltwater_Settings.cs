using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class RT_Saltwater_Settings : ModSettings
{
    public static bool ExampleSetting = true;
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref ExampleSetting, "ExampleSetting");
        base.ExposeData();
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
        string ExampleSetting_Label = "Fish_ExampleSettingLabel".Translate();
        string ExampleSetting_Tooltip = "Fish_CosmeticGenesTooltip".Translate();
        Listing_Standard listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled(ExampleSetting_Label, ref ExampleSetting, ExampleSetting_Tooltip);
        listingStandard.End();
    }
}