using System;
using Unity.Netcode;
using UnityEngine;

public class Player2 : NetworkBehaviour,IDisposable
    {
    [SerializeField]
    private GameObject camera;
    FirstPersonLook firstPersonLook;
    private void Start()
    {
        if (IsOwner)
        {
            gameObject.AddComponent<FirstPersonMovementNew>();
            firstPersonLook= Instantiate(camera, transform, false).GetComponent<FirstPersonLook>();
            firstPersonLook.OnShoot += ShootServerRpc;
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 origin,Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            NetworkObject other =hit.transform.GetComponentInParent<NetworkObject>();
            if (other!=null)
                Debug.Log(OwnerClientId + " shoot at " + other.OwnerClientId);
        }
    }

    public void Dispose()
    {
        firstPersonLook.OnShoot -= ShootServerRpc;
    }
}
