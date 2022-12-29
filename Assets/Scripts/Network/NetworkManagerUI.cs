using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;

    private void Awake()
    {
        _serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            transform.gameObject.SetActive(false);
        });
        _hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            transform.gameObject.SetActive(false);
        });
        _clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            transform.gameObject.SetActive(false);
        });
    }
}
