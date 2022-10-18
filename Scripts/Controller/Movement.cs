using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float rotSpeed = 5;
    public float rollSpeed = 30;
    public float rotSmoothSpeed = 10;

    Quaternion targetRot;
    Quaternion smoothedRot;
    
    Vector3 thrusterInput;
    // Movement Keys
    KeyCode ascendKey = KeyCode.Space;
    KeyCode descendKey = KeyCode.LeftShift;
    KeyCode rollCounterKey = KeyCode.Q;
    KeyCode rollClockwiseKey = KeyCode.E;
    KeyCode forwardKey = KeyCode.W;
    KeyCode backwardKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;


    void HandleMovement()
    {
        // Thrust input
        int thrustInputX = GetInputAxis(leftKey, rightKey);
        int thrustInputY = GetInputAxis(descendKey, ascendKey);
        int thrustInputZ = GetInputAxis(backwardKey, forwardKey);
        thrusterInput = new Vector3(thrustInputX, thrustInputY, thrustInputZ);

        // Rotation input
        float yawInput = Input.GetAxisRaw("Mouse X") * rotSpeed;
        float pitchInput = Input.GetAxisRaw("Mouse Y") * rotSpeed;
        float rollInput = GetInputAxis (rollCounterKey, rollClockwiseKey) * rollSpeed * Time.deltaTime;

        // Calculate rotation
        Quaternion yaw = Quaternion.AngleAxis(yawInput, transform.up);
        Quaternion pitch = Quaternion.AngleAxis(-pitchInput, transform.right);
        Quaternion roll = Quaternion.AngleAxis(-rollInput, transform.forward);

        targetRot = yaw * pitch * roll * targetRot;
        smoothedRot = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSmoothSpeed);
    }

    int GetInputAxis(KeyCode negativeAxis, KeyCode positiveAxis)
    {
        int axis = 0;
        if (Input.GetKey (positiveAxis))
        {
            axis++;
        }
        if (Input.GetKey (negativeAxis))
        {
            axis--;
        }
        return axis;
    }
}
