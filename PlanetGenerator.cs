
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using static ShapeSettings;

public class PlanetGenerator : MonoBehaviour
{
    //on UI
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public OffsetPaletteSettings offsetPaletteSettings;
    public GradientPaletteSettings gradientPaletteSettings;
    public HarmoniesPaletteSettings harmoniesPaletteSettings;
    public RandomWalkPaletteSettings randomWalkPaletteSettings;
    public HSVPaletteSettings hSVPaletteSettings;
    public RandomMixPaletteSettings randomMixPaletteSettings;

    [Range(2, 256)]
    public int resolution = 10;
    public int inputBaseSeed;
    public float speed;
    public enum Show { Color, Surface, Both }
    public Show show;
    public Color[] colorPalette;

    //other variables
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;
    private ShapeGenerator shapeGenerator = new ShapeGenerator();
    private ColorGenerator colorGenerator = new ColorGenerator();
    private Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;
    [HideInInspector]
    public bool offsetPlaletteSettingsFoldout;
    [HideInInspector]
    public bool gradientPlaletteSettingsFoldout;
    [HideInInspector]
    public bool harmoniesPlaletteSettingsFoldout;
    [HideInInspector]
    public bool randomWalkPlaletteSettingsFoldout;
    [HideInInspector]
    public bool hsvPlaletteSettingsFoldout;
    [HideInInspector]
    public bool randomMixPlaletteSettingsFoldout;

    void Start()
    {
        if (show == Show.Color)
        {
            StartCoroutine(StartColorShow());
        }
        else if (show == Show.Surface)
        {
            StartCoroutine(StartSurfaceShow());
        }
        else
        {
            StartCoroutine(StartSurfaceAndColorShow());
        }

    }

    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    private IEnumerator StartColorShow()
    {
        GameObject text = GameObject.FindGameObjectWithTag("ShowCaseText");
        TMP_Text textMeshPro = text.GetComponent<TMP_Text>();
        textMeshPro.text = "Random Color Generation";

        //showcase for random color generation
        for (int i = 0; i < 5; i++)
        {
            GenerateInputWithRandom();
            yield return new WaitForSeconds(2);
        }

        textMeshPro.text = "Random Walk Algorithm";
        //showcase for random walk color generation
        for (int i = 0; i < 5; i++)
        {
            randomWalkPaletteSettings.baseColor = Random.ColorHSV();
            GenerateInputWithRandomWalk();
            yield return new WaitForSeconds(2);
        }

        textMeshPro.text = "Random Offset Algorithm";
        //showcase for random offset color generation
        for (int i = 0; i < 5; i++)
        {
            offsetPaletteSettings.baseColor = Random.ColorHSV();
            GenerateInputWithOffset();
            yield return new WaitForSeconds(2);
        }

        textMeshPro.text = "Random Mix Algorithm";
        //showcase for random offset color generation
        for (int i = 0; i < 5; i++)
        {
            randomMixPaletteSettings.color1 = Random.ColorHSV();
            randomMixPaletteSettings.color2 = Random.ColorHSV();
            randomMixPaletteSettings.color3 = Random.ColorHSV();
            GenerateInputWithRandomMix();
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator StartSurfaceShow()
    {
        GameObject text = GameObject.FindGameObjectWithTag("ShowCaseText");
        TMP_Text textMeshPro = text.GetComponent<TMP_Text>();
        //showcase for random offset color generation
        for (int i = 0; i < 15; i++)
        {
            inputBaseSeed = Random.Range(0, 10000);
            textMeshPro.text = "Seed: " + inputBaseSeed;
            RegeneratePlanetSurface();
            yield return new WaitForSeconds(2);
        }
    }

    private IEnumerator StartSurfaceAndColorShow()
    {
        GameObject text = GameObject.FindGameObjectWithTag("ShowCaseText");
        TMP_Text textMeshPro = text.GetComponent<TMP_Text>();
        //showcase for random offset color generation
        for (int i = 0; i < 15; i++)
        {
            inputBaseSeed = Random.Range(0, 10000);
            Random.InitState(inputBaseSeed);
            Debug.Log("Randomization seed: " + inputBaseSeed);
            textMeshPro.text = "Seed: " + inputBaseSeed;

            randomWalkPaletteSettings.baseColor = Random.ColorHSV();
            colorSettings.gradient = GenerateGradient(GenerateColorWithRandomWalk);

            RegeneratePlanetSurface();
            yield return new WaitForSeconds(2);
        }
    }

    // ----------------------------- CALLBACKS METHODS -------------------------------------------

    public void GenerateInputWithOffset()
    {
        colorSettings.gradient = GenerateGradient(GenerateColorWithOffset);
        GeneratePlanet();
    }

    public void GenerateInputWithGradient()
    {
        colorSettings.gradient = GenerateGradient(GenerateColorWithGradient);
        GeneratePlanet();
    }

    public void GenerateInputWithRandom()
    {
        colorSettings.gradient = GenerateGradient(GenerateRandomColor);
        GeneratePlanet();
    }

    public void GenerateInputWithHarmonies()
    {
        colorSettings.gradient = GenerateGradient(GenerateColorFromHarmonies);
        GeneratePlanet();
    }

    public void GenerateInputWithRandomWalk()
    {
        colorSettings.gradient = GenerateGradient(GenerateColorWithRandomWalk);
        GeneratePlanet();
    }

    public void GenerateInputWithHSV()
    {
        colorSettings.gradient = GenerateGradient(GenerateColorWithHSV);
        GeneratePlanet();
    }

    public void GenerateInputWithRandomMix()
    {
        colorSettings.gradient = GenerateGradient(GenerateColorWithRandomMix);
        GeneratePlanet();
    }

    public void RegeneratePlanetSurface()
    {

        Random.InitState(inputBaseSeed);
        randomWalkPaletteSettings.baseColor = Random.ColorHSV();
        colorSettings.gradient = GenerateGradient(GenerateColorWithRandomWalk);
        Debug.Log("Randomization seed: " + inputBaseSeed);
        RandomizeSurfaceInput();
        GeneratePlanet();
    }

    public void RandomizePlanetSurfaceWithRandomSeed()
    {
        inputBaseSeed = Random.Range(0, 10000);
        RegeneratePlanetSurface();
    }

    public void OnColorSettingsUpdated()
    {
        Initialize();
        GenerateTexture();
    }

    public void OnShapeAndNoiseSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }

    // ----------------------------- COLOR GENERATOR METHODS -------------------------------------------

    private Gradient GenerateGradient(Func<Color> ColorGenerationAlgorithm)
    {
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKey = new GradientColorKey[6];
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[6];

        GenerateColorPalette(ColorGenerationAlgorithm);

        //GenerateColor(colorKey, GenerateRandomColor);
        AssaignColorsFromPalette(colorKey);

        colorKey[0].time = 0.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[0].alpha = 1.0f;

        colorKey[1].time = 0.018f;
        alphaKey[1].time = 0.018f;
        alphaKey[1].alpha = 1.0f;

        colorKey[2].time = 0.079f;
        alphaKey[2].time = 0.079f;
        alphaKey[2].alpha = 1.0f;

        colorKey[3].time = 0.438f;
        alphaKey[3].time = 0.438f;
        alphaKey[3].alpha = 1.0f;

        colorKey[4].time = 0.8f;
        alphaKey[4].time = 0.8f;
        alphaKey[4].alpha = 1.0f;

        colorKey[5].time = 1.0f;
        alphaKey[5].time = 1.0f;
        alphaKey[5].alpha = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient;
    }

    private void GenerateColorPalette(Func<Color> ColorGenerationAlgorithm)
    {
        for (int i = 0; i < colorPalette.Length; i++)
        {
            colorPalette[i] = ColorGenerationAlgorithm();
        }
    }

    private void AssaignColorsFromPalette(GradientColorKey[] colorKey)
    {
        for (int i = 0; i < colorKey.Length; i++)
        {
            colorKey[i].color = colorPalette[i];
        }
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

    private Color GenerateColorWithOffset()
    {
        Color baseColor = offsetPaletteSettings.baseColor;
        float offset = offsetPaletteSettings.offset;

        return new Color(baseColor.r + Random.value * 2 * offset - offset,
            baseColor.g + Random.value * 2 * offset - offset,
            baseColor.b + Random.value * 2 * offset - offset,
            255);
    }

    private Color GenerateColorWithGradient()
    {
        //for (int i = 0; i < n; i++)
        //    color[i] = GradientPaletteSettings.Gradient.colorKeys.le(
        //       (i + 0.5 + (2 * Random. - 1) * maxJitter) * intervalSize);

        //TODO: FINISH IMPLEMENTATION
        return new Color();
    }

    private Color GenerateColorFromHarmonies()
    {
        float referenceAngle = Random.value * 360;

        float randomAngle =
         Random.value * (harmoniesPaletteSettings.rangeAngle0 + harmoniesPaletteSettings.rangeAngle1 + harmoniesPaletteSettings.rangeAngle2);

        if (randomAngle > harmoniesPaletteSettings.rangeAngle0)
        {
            if (randomAngle < harmoniesPaletteSettings.rangeAngle0 + harmoniesPaletteSettings.rangeAngle1)
            {
                randomAngle += harmoniesPaletteSettings.offsetAngle1;
            }
            else
            {
                randomAngle += harmoniesPaletteSettings.offsetAngle2;
            }
        }

        //HSL hslColor = new HSL(
        //   ((referenceAngle + randomAngle) / 360.0f) % 1.0f,
        //   saturation,
        //   luminance);

        //TODO: FINISH IMPLEMENTATION
        return new Color();
    }

    private Color GenerateColorWithRandomWalk()
    {
        Color newColor = randomWalkPaletteSettings.baseColor;

        float range = randomWalkPaletteSettings.max - randomWalkPaletteSettings.min;

        int rSign = Random.Range(0, 2) % 2 == 0 ? 1 : -1;
        int gSign = Random.Range(0, 2) % 2 == 0 ? 1 : -1;
        int bSign = Random.Range(0, 2) % 2 == 0 ? 1 : -1;
        return new Color(
            newColor.r + rSign * (randomWalkPaletteSettings.min + Random.value * range),
            newColor.g + gSign * (randomWalkPaletteSettings.min + Random.value * range),
            newColor.b + bSign * (randomWalkPaletteSettings.min + Random.value * range),
            255);
    }

    private Color GenerateColorWithHSV()
    {
        return Random.ColorHSV(
            hSVPaletteSettings.hue,
            hSVPaletteSettings.hue,
            hSVPaletteSettings.saturation,
            hSVPaletteSettings.saturation,
            hSVPaletteSettings.value,
            hSVPaletteSettings.value);
    }

    private Color GenerateColorWithRandomMix()
    {
        int randomIndex = Random.Range(1, 4);

        float mixRatio1 = (randomIndex == 0) ? Random.value * randomMixPaletteSettings.greyControl : Random.value;
        float mixRatio2 = (randomIndex == 1) ? Random.value * randomMixPaletteSettings.greyControl : Random.value;
        float mixRatio3 = (randomIndex == 2) ? Random.value * randomMixPaletteSettings.greyControl : Random.value;

        float sum = mixRatio1 + mixRatio2 + mixRatio3;

        mixRatio1 /= sum;
        mixRatio2 /= sum;
        mixRatio3 /= sum;

        return new Color(
            mixRatio1 * randomMixPaletteSettings.color1.r + mixRatio2 * randomMixPaletteSettings.color2.r + mixRatio3 * randomMixPaletteSettings.color3.r,
            mixRatio1 * randomMixPaletteSettings.color1.g + mixRatio2 * randomMixPaletteSettings.color2.g + mixRatio3 * randomMixPaletteSettings.color3.g,
            mixRatio1 * randomMixPaletteSettings.color1.b + mixRatio2 * randomMixPaletteSettings.color2.b + mixRatio3 * randomMixPaletteSettings.color3.b,
            255);
    }

    // ----------------------------- PLANET SURFACE RANDOMIZATION METHODS ----------------------------------

    private void RandomizeSurfaceInput()
    {
        shapeSettings = new ShapeSettings();
        if (inputBaseSeed > 10000)
        {
            inputBaseSeed = 10000;
        }
        if (inputBaseSeed < 0)
        {
            inputBaseSeed = 0;
        }
        Random.InitState(inputBaseSeed);
        shapeSettings.radius = Random.Range(5, 10);

        shapeSettings.noiseLayers = new NoiseLayer[3];
        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++)
        {
            NoiseSettings noiseSettings = new NoiseSettings();

            NoiseLayer noiseLayer = new NoiseLayer();

            shapeSettings.noiseLayers[i] = noiseLayer;


            noiseLayer.noiseSettings = noiseSettings;
            noiseLayer.enabled = true;
            // generating the continents
            if (i == 0)
            {
                noiseLayer.useFirstLayerAsMask = false;
                noiseSettings.noiseAmplitude = Random.Range(0.1f, 0.2f);
                noiseSettings.numberOfLayers = Random.Range(2, 4);
                noiseSettings.noiseRoughness = Random.Range(0.2f, 1f);
                noiseSettings.persistance = Random.Range(0.8f, 1f);
                noiseSettings.baseRoughness = Random.Range(0.5f, 1.5f);
                if (noiseSettings.numberOfLayers == 2)
                {
                    noiseSettings.minimumValue = Random.Range(0.5f, 1f);
                }
                else
                {
                    noiseSettings.minimumValue = Random.Range(0.8f, 1.5f);
                }
            }
            // generating the mountains
            else if (i == 1)
            {
                noiseSettings.numberOfLayers = Random.Range(2, 4);
                noiseSettings.noiseAmplitude = Random.Range(0.1f, 0.5f);
                noiseSettings.persistance = Random.Range(1.5f, 2f);
                noiseSettings.noiseRoughness = Random.Range(1f, 2f);
                noiseLayer.useFirstLayerAsMask = true;
                noiseSettings.minimumValue = Random.Range(1.5f, 2f);
                noiseSettings.baseRoughness = Random.Range(1f, 2f);
            }
            else
            {
                noiseSettings.numberOfLayers = Random.Range(2, 4);
                if (noiseSettings.numberOfLayers == 2)
                {
                    noiseSettings.noiseAmplitude = Random.Range(1f, 3f);
                    noiseSettings.noiseRoughness = Random.Range(1f, 2f);
                    noiseSettings.minimumValue = Random.Range(1f, 1.5f);
                }
                else
                {
                    noiseSettings.noiseAmplitude = Random.Range(0.1f, 0.8f);
                    noiseSettings.noiseRoughness = Random.Range(1f, 1.5f);
                    noiseSettings.minimumValue = Random.Range(2f, 3f);
                }

                noiseSettings.persistance = Random.Range(1.5f, 2f);
                noiseLayer.useFirstLayerAsMask = true;
                noiseSettings.baseRoughness = Random.Range(1f, 3f);
            }
            noiseSettings.noiseCenter = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        }
    }

    // ----------------------------- PLANET GENERATION METHODS ---------------------------------------------
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateTexture();
    }

    private void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("TerrainFace");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, directions[i], resolution, shapeGenerator);
        }
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.ElevationMinMax);
    }

    private void GenerateTexture()
    {
        colorGenerator.UpdateColors();
    }
}