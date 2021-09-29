using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorPaletteGenerator
{
    public List<Color> GenerateColorPalette(int sizeOfColorPalette)
    {
        List<Color> colorPalette = new List<Color>();
        for (int i = 0; i < sizeOfColorPalette; i++)
        {
            colorPalette.Add(GenerateColorWithRandomWalk());
        }
        return colorPalette;
    }

    // ----------------------------- COLOR GENERATOR ALGORITHMS -------------------------------------------
    private Color GenerateRandomColor()
    {
        float minHue = Random.Range(0, 1);
        float maxHue = Random.Range(minHue, 1);
        float minSaturation = Random.Range(0, 1);
        float maxSaturation = Random.Range(minSaturation, 1);
        float minValue = Random.Range(0, 1);
        float maxValue = Random.Range(minValue, 1);

        return Random.ColorHSV(minHue, maxHue, minSaturation, maxSaturation, minValue, maxValue);
    }

    private Color GenerateColorWithRandomWalk()
    {
        Color newColor = Random.ColorHSV();

        float min = Random.Range(0f, 1f);
        float max = Random.Range(min, 1f);

        float range = max - min;

        int rSign = Random.Range(0, 2) % 2 == 0 ? 1 : -1;
        int gSign = Random.Range(0, 2) % 2 == 0 ? 1 : -1;
        int bSign = Random.Range(0, 2) % 2 == 0 ? 1 : -1;
        return new Color(
            newColor.r + rSign * (min + Random.value * range),
            newColor.g + gSign * (min + Random.value * range),
            newColor.b + bSign * (min + Random.value * range),
            255);
    }
}
