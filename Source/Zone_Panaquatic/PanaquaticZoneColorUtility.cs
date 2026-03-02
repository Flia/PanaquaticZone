using System.Collections.Generic;
using UnityEngine;

namespace PanaquaticZone;

//straight up copied from ZoneColorUtility
public static class PanaquaticZoneColorUtility 
{
    private static readonly List<Color> panaquaticZoneColors = [];
    private static int nextPanaquaticZoneColorIndex;
    private const float ZoneOpacity = 0.09f;

    static PanaquaticZoneColorUtility()
    {
        foreach (Color panaquaticZoneColor in PanaquaticZoneColors())
        {
            Color color = new Color(panaquaticZoneColor.r, panaquaticZoneColor.g, panaquaticZoneColor.b, ZoneOpacity);
            panaquaticZoneColors.Add(color);
        }
    }

    public static Color NextPanaquaticZoneColor()
    {
        Color panaquaticZoneColor = panaquaticZoneColors[nextPanaquaticZoneColorIndex];
        ++nextPanaquaticZoneColorIndex;
        if (nextPanaquaticZoneColorIndex < panaquaticZoneColors.Count) return panaquaticZoneColor;
        nextPanaquaticZoneColorIndex = 0;
        return panaquaticZoneColor;
    }

    private static IEnumerable<Color> PanaquaticZoneColors()
    {
        yield return Color.Lerp(new Color(0f, 1f, 1f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0f, 0.6f, 1f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0.2f, 1f, 0.6f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0.2f, 0.8f, 1f), Color.gray, 0.25f);
        yield return Color.Lerp(new Color(0.2f, 1f, 0.8f), Color.gray, 0.25f);
    }
}
