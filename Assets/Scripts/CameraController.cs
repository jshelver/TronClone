using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform lookTarget;
    Camera mainCamera;

    [Header("Camera Settings")]
    [SerializeField] float mouseSensitivity = 25f;
    [SerializeField] float distanceFromTarget = 3f;
    [SerializeField] float maxViewAngle = 85f;
    [SerializeField] float minViewAngle = -40f;
    [SerializeField] float rotationSmoothTime = .02f;
    [SerializeField] float resetCameraSmoothTime = .2f;
    [SerializeField] float resetCameraCooldownTime = 3f;
    [SerializeField] float defaultPitchAngle = 20f;
    bool resetCamera;
    float timer;
    float pitch; // X-axis rotation (vertical)
    float yaw; // Y-axis rotation (horizontal)
    Vector3 currentRotation, targetRotation, rotationSmoothVelocity;

    void Start()
    {
        timer = 0;
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        HandleCameraControls(InputManager.instance.lookInput);
    }

    private void HandleCameraControls(Vector2 lookInput)
    {
        // Get mouse input
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minViewAngle, maxViewAngle);

        ResetCameraRotation(lookInput);

        // Rotate camera
        targetRotation = new Vector3(pitch, yaw, 0f);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, resetCamera ? resetCameraSmoothTime : rotationSmoothTime);
        mainCamera.transform.eulerAngles = currentRotation;

        // Set distance away from target
        mainCamera.transform.position = lookTarget.position - mainCamera.transform.forward * distanceFromTarget;
    }

    private void ResetCameraRotation(Vector2 lookInput)
    {
        if (!resetCamera)
        {
            // Reset the timer if any look input
            if (lookInput != Vector2.zero)
            {
                timer = 0;
                return;
            }

            timer += Time.deltaTime;
            // If cooldown time is reached then reset camera rotation
            if (timer >= resetCameraCooldownTime)
            {
                timer = 0;
                resetCamera = true;
            }
        }
        else
        {
            pitch = defaultPitchAngle;

            // Yaw will be set to the closest unit circle equivalent of zero so it takes the shortest reset route
            float yawAngleOnUnitCircle = yaw % 360;
            if (yawAngleOnUnitCircle > 180f)
            {
                yaw = yaw + (360 - yawAngleOnUnitCircle);
            }
            else
            {
                yaw = yaw - yawAngleOnUnitCircle;
            }


            // If any look input then stop resetting the camera
            if (lookInput != Vector2.zero)
            {
                resetCamera = false;
            }
        }
    }

}
