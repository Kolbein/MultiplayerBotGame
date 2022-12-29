using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ConnectedClients : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerText;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        //PopulateClientList();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        _playerText.text = clientId.ToString();
        Instantiate(_playerText, transform);
    }

    private void PopulateClientList()
    {
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
                continue; 

            HandleClientConnected(clientId);
        }
    }
}
