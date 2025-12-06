using Verse;

namespace PanaquaticZone;

public class PlantPreferenceModExtension : DefModExtension 
{ 
    public WaterPlantPreference plantPreference; 
}

public enum WaterPlantPreference
{
    Saltwater,
    Freshwater,
    Either
}