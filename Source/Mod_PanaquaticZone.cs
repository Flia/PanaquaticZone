using HarmonyLib;
using Verse;

namespace PanaquaticZone;

public class Mod_PanaquaticZone : Mod
{
    public Mod_PanaquaticZone(ModContentPack content) : base(content)
    {
        Harmony harmony = new("com.royallytipsy.panaquaticzone");
        harmony.PatchAll();
    }
}