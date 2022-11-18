using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currentConnectionStatusText;
    [SerializeField] TextMeshProUGUI hostText;
    [SerializeField] TextMeshProUGUI clientText;
    [SerializeField] TextMeshProUGUI serverText;
    [SerializeField] GameObject networkUI;

    void Awake()
    {
        hostText.text = "Start Host";
        clientText.text = "Start Client";
        serverText.text = "Start Server";

        InputManager.actionMapChange += OnActionMapChange;
    }

    void Update()
    {
        UpdateConnectionStatus();
    }

    void OnDestroy()
    {
        InputManager.actionMapChange -= OnActionMapChange;
    }

    public void StartHost()
    {
        if (CustomNetworkManager.Instance.IsHost)
        {
            CustomNetworkManager.Instance.Shutdown();
            hostText.text = "Start Host";
            return;
        }

        if (CustomNetworkManager.Instance.StartHost())
        {
            hostText.text = "Stop Host";
            InputManager.SwitchActionMap(InputManager.playerControls.Player);
        }
        else
        {
            hostText.text = "Start Host";
        }
    }

    public void StartClient()
    {
        if (CustomNetworkManager.Instance.IsClient)
        {
            CustomNetworkManager.Instance.Shutdown();
            clientText.text = "Start Client";
            return;
        }

        if (CustomNetworkManager.Instance.StartClient())
        {
            clientText.text = "Stop Client";
            InputManager.SwitchActionMap(InputManager.playerControls.Player);
        }
        else
        {
            clientText.text = "Start Client";
        }
    }

    public void StartServer()
    {
        if (CustomNetworkManager.Instance.IsServer)
        {
            CustomNetworkManager.Instance.Shutdown();
            serverText.text = "Start Server";
            return;
        }

        if (CustomNetworkManager.Instance.StartServer())
        {
            serverText.text = "Stop Server";
            InputManager.SwitchActionMap(InputManager.playerControls.Player);
        }
        else
        {
            serverText.text = "Start Server";
        }
    }

    private void OnActionMapChange(InputActionMap actionMap)
    {
        Debug.Log(actionMap.name);
        if (actionMap.name == "UI")
        {
            networkUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            networkUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void UpdateConnectionStatus()
    {
        if (CustomNetworkManager.Instance.IsHost)
        {
            currentConnectionStatusText.text = "Host";
        }
        else if (CustomNetworkManager.Instance.IsClient)
        {
            currentConnectionStatusText.text = "Client";
        }
        else if (CustomNetworkManager.Instance.IsServer)
        {
            currentConnectionStatusText.text = "Server";
        }
        else
        {
            currentConnectionStatusText.text = "Not Connected";
        }
    }
}
