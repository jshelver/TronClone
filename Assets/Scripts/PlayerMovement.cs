using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform lookTarget;
    Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 9f;
    [SerializeField] float turnSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleAcceleration(InputManager.instance.accelerationInput);
        HandleSteering(InputManager.instance.steeringInput);
    }

    private void HandleAcceleration(float accelerationInput)
    {
        // Get input
        Vector3 moveDirection = rb.transform.forward * Mathf.Clamp(accelerationInput, 0f, 1f);

        // Move player
        rb.AddForce(moveDirection * moveSpeed * 100f * Time.deltaTime);

        LimitVelocity();
    }

    private void LimitVelocity()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void HandleSteering(float steeringInput)
    {
        lookTarget.forward = rb.transform.forward;

        Vector3 torque = rb.transform.up * steeringInput;

        rb.AddTorque(torque * turnSpeed * 100 * Time.deltaTime);
    }
}
