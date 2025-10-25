using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform; // get AR camera
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Make this object face the camera but stay upright
        Vector3 lookDirection = transform.position - cameraTransform.position;
        //lookDirection.y = 0; // optional: keep it level
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}