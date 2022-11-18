using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform[] wheelTransforms;
    Rigidbody rb;
    [SerializeField] LayerMask groundLayer;

    [Header("Suspension Settings")]
    [SerializeField] float suspensionRestDistance = 0.5f;
    [SerializeField] float springStrength = 10f;
    [SerializeField] float springDamper = 1f;
    float lastHitDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        foreach (Transform t in wheelTransforms)
        {
            ApplySuspensionForce(t);
        }
    }

    private void ApplySuspensionForce(Transform wheelTransform)
    {
        if (Physics.Raycast(wheelTransform.position, -wheelTransform.up, out RaycastHit hit, suspensionRestDistance, groundLayer))
        {
            float forceAmount = HooksLawDampen(hit.distance);
            rb.AddForceAtPosition(transform.up * forceAmount, wheelTransform.position);
        }
        else
        {
            lastHitDistance = suspensionRestDistance * 1.5f;
        }
    }

    private float HooksLawDampen(float hitDistance)
    {
        float forceAmount = springStrength * (suspensionRestDistance - hitDistance) + springDamper * (lastHitDistance - hitDistance);
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDistance = hitDistance;

        return forceAmount;
    }
}
