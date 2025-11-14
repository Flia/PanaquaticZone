using UnityEngine;
using Verse;

namespace RT_Saltwater;

public class RT_Saltwater_Settings : ModSettings
{
    public static bool NoSalt = false;
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref NoSalt, "NoSaltSetting");
        base.ExposeData();
    }

    public static void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("RT_Saltwater_NoSaltLabel".Translate(), ref NoSalt, "RT_Saltwater_NoSaltTooltip".Translate());
        listingStandard.End();
    }
}