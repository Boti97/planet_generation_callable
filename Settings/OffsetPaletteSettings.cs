using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OffsetPaletteSettings : ScriptableObject
{
    public Color baseColor;
    [Range(0, 1)]
    public float offset;
}
