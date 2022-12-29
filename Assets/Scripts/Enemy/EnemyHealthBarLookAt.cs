using Unity.Netcode;
using UnityEngine;

public class EnemyHealthBarLookAt : NetworkBehaviour
{
    private Camera _camera;

    public override void OnNetworkSpawn()
    {
        _camera = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponentInChildren<Camera>();
        if (_camera)
            Debug.Log("Camera is null");
    }

    void Update()
    {
        if (_camera is null) return;

        transform.LookAt(_camera.transform);
    }
}
