using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 2;
    public float distance = 2;
    public float smoothSpeed = 40;
    public float rotationSmoothSpeed = 40;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.TransformPoint(0f, height, -distance),
            smoothSpeed * Time.deltaTime
        );
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(target.position - transform.position + (Vector3.up * 1.5f), Vector3.up),
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
