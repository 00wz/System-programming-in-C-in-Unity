using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject PrefabToSpawn;
    public bool DestroyWithSpawner;
    private GameObject m_PrefabInstance;
    private NetworkObject m_SpawnedNetworkObject;
    private bool isInstantiated = false;
    void OnGUI()
    {
        if (IsOwner&&!isInstantiated)
        {
            GUILayout.BeginArea(new Rect(10, 310, 300, 300));
            if (GUILayout.Button("Spawn"))
            {
                SpawnServerRpc();
                isInstantiated = true;
            }
            GUILayout.EndArea();
        }
    }

    [ServerRpc]
    private void SpawnServerRpc()
    {
        // Only the server spawns, clients will disable this component on their side
        /*enabled = IsServer;
        if (!enabled || PrefabToSpawn == null)
        {
            return;
        }*/
        // Instantiate the GameObject Instance
        m_PrefabInstance = Instantiate(PrefabToSpawn);
        /*
        // Optional, this example applies the spawner's position and rotation to the new instance
        m_PrefabInstance.transform.position = transform.position;
        m_PrefabInstance.transform.rotation = transform.rotation;
        */

        // Get the instance's NetworkObject and Spawn
        m_SpawnedNetworkObject = m_PrefabInstance.GetComponent<NetworkObject>();
        m_SpawnedNetworkObject.SpawnWithOwnership(OwnerClientId);
    }

    /*
    public override void OnNetworkDespawn()
    {
        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkObject != null && m_SpawnedNetworkObject.IsSpawned)
        {
            m_SpawnedNetworkObject.Despawn();
        }
        base.OnNetworkDespawn();
    }*/
}
