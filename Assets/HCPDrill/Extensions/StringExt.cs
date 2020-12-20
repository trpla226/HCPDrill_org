using UnityEngine;

public static class StringExt
{
    public static Color ToColor(this string self)
    {
        var color = default(Color);
        if (!ColorUtility.TryParseHtmlString(self, out color))
        {
            Debug.LogWarning("Unknown color code... " + self);
        }
        return color;
    }
}