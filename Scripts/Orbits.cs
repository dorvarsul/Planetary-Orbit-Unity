using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbits : MonoBehaviour
{
    CelestialBody[] bodies;
    public float _timeStep;

    void Start()
    {
        bodies = FindObjectsOfType<CelestialBody>();
    }

    void FixedUpdate()
    {
        foreach (CelestialBody body in bodies)
        {
            if (Vector3.Distance(body.transform.position, this.transform.position) >= 2)
            {
               body.UpdateVelocity(bodies, _timeStep);
            }
        }
        foreach (CelestialBody body in bodies)
        {
            body.UpdatePosition(_timeStep);
        }
    }
}

