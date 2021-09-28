using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GradientPaletteSettings : ScriptableObject
{
    public Gradient gradient;
    [Range(0f, 1f)]
    public float maxJitter;
    public float intervalSize;
}
