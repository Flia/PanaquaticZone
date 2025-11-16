using HarmonyLib;
using Verse;
using RimWorld;

namespace RT_Saltwater;

[HarmonyPatch(typeof(Command_SetPlantToGrow), "ProcessInput")]
public class HarmonyPatch_Command_SetPlantToGrow
{
    public static void Postfix()
    {
        //Messages.Message((string) "ProcessInput", MessageTypeDefOf.CautionInput, false);
    }
}

[HarmonyPatch(typeof(Command_SetPlantToGrow), "WarnAsAppropriate")]
public class HarmonyPatch_WarnAsAppropriate
{
    public static void Postfix()
    {
        Messages.Message((string) "WarnAsAppropriate", MessageTypeDefOf.CautionInput, false);
    }
}

