using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SinglePooledDynamicSpawner : NetworkBehaviour, INetworkPrefabInstanceHandler
{
    public GameObject PrefabToSpawn;
    public bool SpawnPrefabAutomatically;

    private GameObject _PrefabInstance;
    private NetworkObject _SpawnedNetworkObject;


    private void Start()
    {
        // Instantiate our instance when we start (for both clients and server)
        _PrefabInstance = Instantiate(PrefabToSpawn);

        // Get the NetworkObject component assigned to the prefab instance
        _SpawnedNetworkObject = _PrefabInstance.GetComponent<NetworkObject>();

        // Set it to be inactive
        _PrefabInstance.SetActive(false);
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(2);
        _SpawnedNetworkObject.Despawn();
        StartCoroutine(SpawnTimer());
        yield break;
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(2);
        SpawnInstance();
        yield break;
    }

    /// <summary>
    /// Invoked only on clients and not server or host
    /// INetworkPrefabInstanceHandler.Instantiate implementation
    /// Called when Netcode for GameObjects need an instance to be spawned
    /// </summary>
    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        _PrefabInstance.SetActive(true);
        _PrefabInstance.transform.position = transform.position;
        _PrefabInstance.transform.rotation = transform.rotation;
        return _SpawnedNetworkObject;
    }

    /// <summary>
    /// Client and Server side
    /// INetworkPrefabInstanceHandler.Destroy implementation
    /// </summary>
    public void Destroy(NetworkObject networkObject)
    {
        _PrefabInstance.SetActive(false);
    }

    public void SpawnInstance()
    {
        if (!IsServer)
        {
            return;
        }

        if (_PrefabInstance != null && _SpawnedNetworkObject != null && !_SpawnedNetworkObject.IsSpawned)
        {
            _PrefabInstance.SetActive(true);
            _SpawnedNetworkObject.Spawn();
            StartCoroutine(DespawnTimer());
        }
    }

    public override void OnNetworkSpawn()
    {
        // We register our network prefab and this NetworkBehaviour that implements the
        // INetworkPrefabInstanceHandler interface with the prefab handler
        NetworkManager.PrefabHandler.AddHandler(PrefabToSpawn, this);

        if (!IsServer || !SpawnPrefabAutomatically)
        {
            return;
        }

        if (SpawnPrefabAutomatically)
        {
            SpawnInstance();
        }
    }

    public override void OnNetworkDespawn()
    {
        if (_SpawnedNetworkObject != null && _SpawnedNetworkObject.IsSpawned)
        {
            _SpawnedNetworkObject.Despawn();
        }
        base.OnNetworkDespawn();
    }

    public override void OnDestroy()
    {
        // This example destroys the
        if (_PrefabInstance != null)
        {
            // Always deregister the prefab
            NetworkManager.Singleton.PrefabHandler.RemoveHandler(PrefabToSpawn);
            Destroy(_PrefabInstance);
        }
        base.OnDestroy();
    }
}
