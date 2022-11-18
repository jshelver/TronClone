using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 9f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float steeringDamper = 0.5f;
    [SerializeField] float stabilizeSpeed = 0.5f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        base.OnNetworkSpawn();
        transform.position = new Vector3(Random.Range(-10, 10), 1f, Random.Range(-10, 10));
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        HandleAcceleration(InputManager.instance.accelerationInput);
        HandleSteering(InputManager.instance.steeringInput);
        StabilizeRigidbodyRotation();
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
        Vector3 torque = rb.transform.up * steeringInput;

        rb.AddTorque(torque * turnSpeed * 100 * Time.deltaTime);

        LimitAngularVelocity();
    }

    private void LimitAngularVelocity()
    {
        rb.maxAngularVelocity = 1.8f;
        rb.AddTorque(-rb.angularVelocity * steeringDamper, ForceMode.Acceleration);
    }

    private void StabilizeRigidbodyRotation()
    {
        // Find quaternion of distance between current up and world up
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);
        // Get the angle and axis of that quaternion
        deltaQuat.ToAngleAxis(out float angle, out Vector3 axis);
        // Apply torque that returns rb to world up
        rb.AddTorque(axis.normalized * angle * stabilizeSpeed, ForceMode.Acceleration);
    }
}
