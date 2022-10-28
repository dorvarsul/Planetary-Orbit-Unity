using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings _settings;
    INoiseFilter[] _noiseFilter;
    public MinMax _elevationMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        _settings = settings;
        _noiseFilter = new INoiseFilter[_settings._noiseLayers.Length];

        // This loop initializes each noise filter with the factory design pattern
        for (int i=0; i < _noiseFilter.Length; i++)
        {
            _noiseFilter[i] = NoiseFilterFactory.CreateNoiseFilter(settings._noiseLayers[i].noiseSettings);
        }
        _elevationMinMax = new MinMax();
    }

    // This functions calculates the height of each point on the planet based on a few parameters
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

        for (int i = 1; i < _noiseFilter.Length; i++)
        {
            if (_settings._noiseLayers[i].enabled)
            {
                float mask = (_settings._noiseLayers[i].useFirstLayerAsMask)? firstLayerValue: 1;
                elevation += _noiseFilter[i].Evaluate(pointOnUnitSphere);
            }
        }
        elevation =  _settings._planetRadius * (1+elevation);
        _elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}
