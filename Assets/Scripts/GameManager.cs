using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject Enemy;

    private static GameManager s_instance;
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
                Debug.LogError("Game Manager is null");

            return s_instance;
        }
    }

    private void Awake()
    {
        s_instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SpawnEnemyFromPoolServerRpc();

        if (Input.GetKeyDown(KeyCode.R))
            DespawnEnemyFromPoolServerRpc();

    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnEnemyFromPoolServerRpc()
    {
        GameObject go = NetworkObjectPool.Singleton.GetNetworkObject(Enemy).gameObject;
        go.transform.position = new Vector3(x: Random.Range(-20, 20), y: 2, z: Random.Range(-20, 20));
        go.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnEnemyFromPoolServerRpc()
    {
        var gameObject = GameObject.FindGameObjectsWithTag("Enemy").FirstOrDefault();
        var networkObject = gameObject?.GetComponent<NetworkObject>();
        if (networkObject != null)
            networkObject.Despawn();
    }
}
