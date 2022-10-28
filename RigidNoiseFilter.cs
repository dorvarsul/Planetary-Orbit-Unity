using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RigidNoiseSettings _settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        _settings = settings;
    }

    // For each point on our sphere we calculate the height based on a few parameters
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < _settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + _settings.center));
            v*=v;
            v *= Mathf.Clamp01(_settings.weightMultiplier);
            weight = v;
            
            noiseValue += v * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistance;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strength;
    }
}
