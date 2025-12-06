using HarmonyLib;
using Verse;

namespace PanaquaticZone;

[StaticConstructorOnStartup]
public class HarmonyBootstrap
{
    static HarmonyBootstrap()
    {
        Harmony harmony = new("com.royallytipsy.panaquaticzone");
        harmony.PatchAll();
    }
}
