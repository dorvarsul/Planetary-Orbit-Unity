using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public bool throttle => Input.GetKey(KeyCode.Space);

    public float pitchPower, rollPower, yawPower, enginePower;
    private float activeRoll, activePitch, activeYaw;

    void Update()
    {
        if (throttle)
        {
            transform.position += transform.forward * enginePower * Time.deltaTime;
        }
        HandleMovement(throttle);
    }

    // Handles rotation of spaceship
    void HandleMovement(bool isThrottle)
    {
        float multiplier = (isThrottle)? 1f:0.5f;

        activePitch = Input.GetAxisRaw("Vertical") * (pitchPower * multiplier) * Time.deltaTime;
        activeRoll = Input.GetAxisRaw("Horizontal") * (rollPower * multiplier) * Time.deltaTime;
        activeYaw = Input.GetAxisRaw("Yaw") * (yawPower * multiplier) * Time.deltaTime;

        transform.Rotate
        (
            activePitch * pitchPower * Time.deltaTime,
            activeRoll * rollPower * Time.deltaTime,
            activeYaw * yawPower * Time.deltaTime
        );
    }
}
