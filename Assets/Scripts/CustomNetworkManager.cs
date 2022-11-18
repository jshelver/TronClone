using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : NetworkManager
{
    [Header("References")]
    public static CustomNetworkManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }
}
