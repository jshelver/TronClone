using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("References")]
    public static InputManager instance;
    PlayerControls playerControls;

    [Header("Input Variables")]
    [HideInInspector] public float accelerationInput;
    [HideInInspector] public float steeringInput;
    [HideInInspector] public Vector2 lookInput;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerControls = new PlayerControls();
    }

    void Update()
    {
        accelerationInput = playerControls.Player.Acceleration.ReadValue<float>();
        steeringInput = playerControls.Player.Steering.ReadValue<float>();
        lookInput = playerControls.Player.Look.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }
}
