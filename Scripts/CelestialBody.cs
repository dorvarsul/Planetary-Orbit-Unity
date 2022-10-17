using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    Rigidbody _rigid;
    public float _mass;
    public Vector3 initialVelocity;
    Vector3 _currentVelocity;
    public Vector3 _rotation;

    void Awake()
    {
        _currentVelocity = initialVelocity;
        _rigid = this.GetComponent<Rigidbody>();
    }

    public void UpdateVelocity(CelestialBody[] bodies, float timeStep)
    {
        // For each celestial body in the system we calculate the force that is influencing the current object orbit
        foreach (CelestialBody otherBody in bodies)
        {
            if (otherBody != this)
            {
                float sqrDis = (otherBody._rigid.position - _rigid.position).sqrMagnitude; // calculate square distance
                Vector3 forceDir = (otherBody._rigid.position - _rigid.position).normalized; // the direction of the force applied

                Vector3 force = forceDir * _mass * otherBody._mass / sqrDis;
                Vector3 acceleration = force / _mass;
                _currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        _rigid.position += _currentVelocity * timeStep;
        this.transform.Rotate(_rotation * Time.deltaTime);
    }
}
