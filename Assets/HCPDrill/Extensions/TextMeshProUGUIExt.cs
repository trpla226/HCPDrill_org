using System.Collections;
using TMPro;
using UnityEngine;

public static class TextmeshProgUGUIExt
{
    public static void SetOpacity(this TextMeshProUGUI self, float alpha)
    {
        var c = self.color;
        self.color = new Color(c.r, c.g, c.b, alpha);
    }
}
