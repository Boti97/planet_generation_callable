using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HSVPaletteSettings : ScriptableObject
{
    [Range(0f, 1f)]
    public float hue;
    [Range(0f, 1f)]
    public float saturation;
    [Range(0f, 1f)]
    public float value;
}
