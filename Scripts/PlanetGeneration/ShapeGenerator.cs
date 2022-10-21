using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings _settings;
    NoiseFilter[] _noiseFilter;

    public ShapeGenerator(ShapeSettings settings)
    {
        _settings = settings;
        _noiseFilter = new NoiseFilter[_settings._noiseLayers.Length];

        for (int i=0; i < _noiseFilter.Length; i++)
        {
            _noiseFilter[i] = new NoiseFilter(_settings._noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (_noiseFilter.Length > 0)
        {
            firstLayerValue = _noiseFilter[0].Evaluate(pointOnUnitSphere);
            if (_settings._noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 0; i < _noiseFilter.Length; i++)
        {
            if (_settings._noiseLayers[i].enabled)
            {
                float mask = (_settings._noiseLayers[i].useFirstLayerAsMask)? firstLayerValue: 1;
                elevation += _noiseFilter[i].Evaluate(pointOnUnitSphere);
            }
        }
        return pointOnUnitSphere * _settings._planetRadius * (1+elevation);
    }
}
