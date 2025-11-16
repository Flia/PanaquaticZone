using HarmonyLib;
using Verse;

namespace RT_Saltwater;

[StaticConstructorOnStartup]
public class HarmonyBootstrap
{
    static HarmonyBootstrap()
    {
        var harmony = new Harmony("com.royallytipsy.saltwater");
        harmony.PatchAll();
    }
}
