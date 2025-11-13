using System.Collections.Generic;
using UnityEngine;

namespace RT_Saltwater;

public static class SaltwaterZoneColorUtility //straight up copied from ZoneColorUtility
{
    private static List<Color> saltwaterZoneColors = new List<Color>();
    private static int nextSaltwaterZoneColorIndex = 0;
    private const float ZoneOpacity = 0.09f;

    static SaltwaterZoneColorUtility()
    {
        foreach (Color saltwaterZoneColor in SaltwaterZoneColors())
        {
            Color color = new Color(saltwaterZoneColor.r, saltwaterZoneColor.g, saltwaterZoneColor.b, ZoneOpacity);
            saltwaterZoneColors.Add(color);
        }
    }

    public static Color NextSaltwaterZoneColor()
    {
        Color saltwaterZoneColor = saltwaterZoneColors[nextSaltwaterZoneColorIndex];
        ++nextSaltwaterZoneColorIndex;
        if (nextSaltwaterZoneColorIndex < saltwaterZoneColors.Count) return saltwaterZoneColor;
        nextSaltwaterZoneColorIndex = 0;
        return saltwaterZoneColor;
    }

    public static IEnumerable<Color> SaltwaterZoneColors()
    {
        yield return Color.Lerp(new Color(0f, 1f, 1f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0f, 0.6f, 1f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0.2f, 1f, 0.6f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0.2f, 0.8f, 1f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0.2f, 1f, 0.8f), Color.gray, 0.25f);
    }
}
