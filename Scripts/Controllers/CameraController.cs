using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float dis = 10f;
    public float height = 5f;

    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    void LateUpdate()
    {
        if (!target) { return; }

        Vector3 followPos = new Vector3(0.0f, height, -dis);
        Quaternion lookRotation = Quaternion.identity;

        lookRotation.eulerAngles = new Vector3(30.0f,0.0f,0.0f);

        Matrix4x4 m1 = Matrix4x4.TRS(target.position, target.rotation, Vector3.one);
        Matrix4x4 m2 = Matrix4x4.TRS(followPos, lookRotation, Vector3.one);
        Matrix4x4 combined = m1 * m2;

        // Get pos and rotation from a matrix
        Vector3 position = combined.GetColumn(3);

        Quaternion rotation = Quaternion.LookRotation
        (
            combined.GetColumn(2),
            combined.GetColumn(1)
        );

        Quaternion wantedRotation = rotation;
        Quaternion currentRotation = transform.rotation;

        Vector3 wantedPos = position;
        Vector3 currentPos = transform.position;

        currentRotation = Quaternion.Lerp
        (
            currentRotation,
            wantedRotation,
            rotationDamping * Time.deltaTime
        );

        currentPos = Vector3.Lerp
        (
            currentPos,
            wantedPos,
            heightDamping * Time.deltaTime
        );

        transform.localRotation = currentRotation;
        transform.localPosition = currentPos;
    }
}
