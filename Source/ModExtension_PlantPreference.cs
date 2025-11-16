using Verse;

namespace RT_Saltwater;

public class PlantPreferenceModExtension : DefModExtension 
{ 
    public WaterPlantPreference plantPreference; 
}

public enum WaterPlantPreference
{
    Saltwater,
    Freshwater,
    Either,
    Neither,
}