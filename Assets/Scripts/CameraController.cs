using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform lookTarget;
    Rigidbody rb;
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
    float currentPitch, targetPitch, pitchSmoothVelocity;
    float currentYaw, targetYaw, yawSmoothVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = 0;
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        HandleCameraControls(InputManager.instance.lookInput);
        SetCameraDistance();
    }

    private void HandleCameraControls(Vector2 lookInput)
    {
        // Get mouse input
        targetYaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        targetPitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        targetPitch = Mathf.Clamp(targetPitch, minViewAngle, maxViewAngle);

        ResetCameraRotation(lookInput);

        // Rotate camera
        currentPitch = Mathf.SmoothDampAngle(currentPitch, targetPitch, ref pitchSmoothVelocity, resetCamera ? resetCameraSmoothTime : rotationSmoothTime);
        currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawSmoothVelocity, resetCamera ? resetCameraSmoothTime : rotationSmoothTime);
        mainCamera.transform.eulerAngles = new Vector3(currentPitch, currentYaw, 0f);
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
            // If any look input then stop resetting the camera
            if (lookInput != Vector2.zero)
            {
                resetCamera = false;
            }
            // Set target values
            targetPitch = defaultPitchAngle;
            targetYaw = rb.transform.eulerAngles.y;
        }
    }

    private void SetCameraDistance()
    {
        mainCamera.transform.position = lookTarget.position - mainCamera.transform.forward * distanceFromTarget;
    }

}
