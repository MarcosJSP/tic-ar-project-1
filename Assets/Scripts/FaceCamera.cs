using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform cameraTransform;

    // How fast it rotates toward the camera
    [SerializeField] private float rotationSpeed = 10f;

    void Start()
    {
        cameraTransform = Camera.main.transform; // get AR camera
    }

    void LateUpdate()
{
    if (cameraTransform == null) return;

    Vector3 lookDirection = transform.position - cameraTransform.position;
    // lookDirection.y = 0;

    // Smoothly rotate toward camera
    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
}
}