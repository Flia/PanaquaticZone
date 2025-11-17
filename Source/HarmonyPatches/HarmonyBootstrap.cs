using HarmonyLib;
using Verse;

namespace RT_Saltwater;

[StaticConstructorOnStartup]
public class HarmonyBootstrap
{
    static HarmonyBootstrap()
    {
        Harmony harmony = new("com.royallytipsy.saltwater");
        harmony.PatchAll();
    }
}
