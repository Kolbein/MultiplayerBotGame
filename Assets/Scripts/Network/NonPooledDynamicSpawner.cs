using Unity.Netcode;
using UnityEngine;

public class NonPooledDynamicSpawner : NetworkBehaviour
{
    public GameObject PrefabToSpawn;
    public bool DestroyWithSpawner;
    private GameObject _PrefabInstance;
    private NetworkObject _SpawnedNetworkObject;

    public override void OnNetworkSpawn()
    {
        // Only the server spawns, clients will disable this component on their side
        enabled = IsServer;
        if (!enabled || PrefabToSpawn == null)
            return;

        // Instantiate the GameObject Instance
        _PrefabInstance = Instantiate(PrefabToSpawn);

        // Optional, this example applies the spawner's position and rotation to the new instance
        _PrefabInstance.transform.position = transform.position;
        _PrefabInstance.transform.rotation = transform.rotation;

        // Get the instance's NetworkObject and Spawn
        _SpawnedNetworkObject = _PrefabInstance.GetComponent<NetworkObject>();
        _SpawnedNetworkObject.Spawn();
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && DestroyWithSpawner && _SpawnedNetworkObject != null && _SpawnedNetworkObject.IsSpawned)
            _SpawnedNetworkObject.Despawn();
        
        base.OnNetworkDespawn();
    }
}