using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace PanaquaticZone;

[UsedImplicitly]
public class Mod_PanaquaticZone : Mod
{
    public Mod_PanaquaticZone(ModContentPack content) : base(content)
    {
        new Harmony("royallytipsy.panaquaticzone").PatchAll(Assembly.GetExecutingAssembly());
        LongEventHandler.QueueLongEvent(() => 
            System.Runtime.CompilerServices.RuntimeHelpers
                .RunClassConstructor(typeof(Setup).TypeHandle), "Panaquatic Zone", true, null);
    }
}