using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("References")]
    public static InputManager instance;
    public static PlayerControls playerControls;
    public static event Action<InputActionMap> actionMapChange;

    [Header("Game Input Variables")]
    [HideInInspector] public float accelerationInput;
    [HideInInspector] public float steeringInput;
    [HideInInspector] public Vector2 lookInput;
    [HideInInspector] public bool gameEscapeInput;

    [Header("UI Input Variables")]
    [HideInInspector] public bool menuEscapeInput;

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

    void Start()
    {
        SwitchActionMap(playerControls.UI);
    }

    void Update()
    {
        accelerationInput = playerControls.Player.Acceleration.ReadValue<float>();
        steeringInput = playerControls.Player.Steering.ReadValue<float>();
        lookInput = playerControls.Player.Look.ReadValue<Vector2>();

        // Enter pause menu
        gameEscapeInput = playerControls.Player.Escape.triggered;
        if (gameEscapeInput)
        {
            SwitchActionMap(playerControls.UI);
        }

        // Exit pause menu
        menuEscapeInput = playerControls.UI.Escape.triggered;
        if (menuEscapeInput && (CustomNetworkManager.Instance.IsClient || CustomNetworkManager.Instance.IsHost || CustomNetworkManager.Instance.IsServer))
        {
            SwitchActionMap(playerControls.Player);
        }
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public static void SwitchActionMap(InputActionMap actionMap)
    {
        // Disables every action map
        playerControls.Disable();
        // Call the action map change event so scripts are aware of the change (optional)
        actionMapChange?.Invoke(actionMap);
        // Enable desired action map
        actionMap.Enable();
    }
}
