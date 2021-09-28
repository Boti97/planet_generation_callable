using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetEditor : Editor
{
    PlanetGenerator planet;
    Editor shapeEditor;
    Editor colorEditor;
    Editor offsetPaletteEditor;
    Editor gradientPaletteEditor;
    Editor harmoniesPaletteEditor;
    Editor randomWalkPaletteEditor;
    Editor hsvPaletteEditor;
    Editor randomMixPaletteEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Regenerate Planet Surface With Given Seed"))
        {
            planet.RegeneratePlanetSurface();
        }

        if (GUILayout.Button("Randomize Planet Surface With Random Seed"))
        {
            planet.RandomizePlanetSurfaceWithRandomSeed();
        }

        if (GUILayout.Button("Generate Random Palette"))
        {
            planet.GenerateInputWithRandom();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeAndNoiseSettingsUpdated, ref shapeEditor, ref planet.shapeSettingsFoldout);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref colorEditor, ref planet.colorSettingsFoldout);
        DrawSettingsEditor(planet.offsetPaletteSettings, planet.GenerateInputWithOffset, ref offsetPaletteEditor, ref planet.offsetPlaletteSettingsFoldout);
        DrawSettingsEditor(planet.gradientPaletteSettings, planet.GenerateInputWithGradient, ref gradientPaletteEditor, ref planet.gradientPlaletteSettingsFoldout);
        DrawSettingsEditor(planet.harmoniesPaletteSettings, planet.GenerateInputWithHarmonies, ref harmoniesPaletteEditor, ref planet.harmoniesPlaletteSettingsFoldout);
        DrawSettingsEditor(planet.randomWalkPaletteSettings, planet.GenerateInputWithRandomWalk, ref randomWalkPaletteEditor, ref planet.randomWalkPlaletteSettingsFoldout);
        DrawSettingsEditor(planet.hSVPaletteSettings, planet.GenerateInputWithHSV, ref hsvPaletteEditor, ref planet.hsvPlaletteSettingsFoldout);
        DrawSettingsEditor(planet.randomMixPaletteSettings, planet.GenerateInputWithRandomMix, ref randomMixPaletteEditor, ref planet.randomMixPlaletteSettingsFoldout);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref Editor editor, ref bool foldout)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (PlanetGenerator)target;
    }
}
