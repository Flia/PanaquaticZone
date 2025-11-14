using Verse;
using UnityEngine;

namespace RT_Saltwater;

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