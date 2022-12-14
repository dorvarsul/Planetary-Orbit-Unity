using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings _settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        _settings = settings;
        if (texture == null || texture.height != settings._biomeSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution, settings._biomeSettings.biomes.Length);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings._biomeSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _settings._planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - _settings._biomeSettings.noiseOffset) * _settings._biomeSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = _settings._biomeSettings.biomes.Length;
        float blendRange = _settings._biomeSettings.blendAmount / 2f + .001f;

        for (int i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - _settings._biomeSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1-weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, (numBiomes - 1));
    }
    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;
        foreach (var biome in _settings._biomeSettings.biomes)
        {
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientCol = biome.gradient.Evaluate(i / (textureResolution - 1f));
                Color tintCol = biome.tint;
                colors[colorIndex] = gradientCol * (1-biome.tintPercent) + tintCol * biome.tintPercent;
                colorIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        _settings._planetMaterial.SetTexture("_texture", texture);
    }

}
