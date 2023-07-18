using System;
using Unity.Netcode;
using UnityEngine;

public class Player2 : NetworkBehaviour
    {
    [SerializeField]
    private GameObject camera;
    private void Start()
    {
        if (IsOwner)
        {
            gameObject.AddComponent<FirstPersonMovementNew>();
            Instantiate(camera, transform, false);
        }
    }
}
