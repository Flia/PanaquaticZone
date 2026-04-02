using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PanaquaticZone;

[HarmonyPatch]
public class HarmonyPatchCompat_NicePlantsMenu
{
    [HarmonyPrepare]
    private static bool shouldPatchBadHygiene()
    {
        return ModsConfig.IsActive("Andromeda.NicePlantsMenu");
    }

    [HarmonyTargetMethod]
    public static MethodInfo TargetMethod()
    {
        return AccessTools.Method("NicePlantsMenu.Dialog_PlantBrowser:SewAvailable");
    }

    [HarmonyPostfix]
    private static void Postfix(ThingDef plantDef, IPlantToGrowSettable s, ref bool __result)
    {
        if (s is Zone_Panaquatic panaquatic)
        {
            __result = PollutionUtility.CanPlantAt(plantDef, panaquatic) && 
                       PanaquaticUtility.CanPlantAt(plantDef, panaquatic) &&
                       plantDef.plant.sowTags.Contains("Panaquatic_Zone");
        }
    }
}