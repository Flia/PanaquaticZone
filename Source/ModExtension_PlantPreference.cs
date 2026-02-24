using Verse;

namespace PanaquaticZone;

public class PlantSalinityPreference : DefModExtension
{ 
    public WaterPlantPreference plantPreference;

    public PlantSalinityPreference() {}

    public PlantSalinityPreference(WaterPlantPreference plantPreference)
    {
        this.plantPreference = plantPreference;
    }
}

public enum WaterPlantPreference
{
    Freshwater = 0,
    Saltwater,
    Either,
    WildTagged
}