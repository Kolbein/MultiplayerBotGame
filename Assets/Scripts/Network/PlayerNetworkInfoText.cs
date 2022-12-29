using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkInfoText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private NetworkObject _networkObject;

    // Start is called before the first frame update
    void Start()
    {
        _networkObject = GetComponent<NetworkObject>();
        _text = GameObject.Find("NetworkInfoText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = PlayerInfoToString();
    }

    private string PlayerInfoToString()
    {
        return "NetworkObjectId: " + _networkObject.NetworkObjectId + "\n" +
            "OwnerClientId: " + _networkObject.OwnerClientId + "\n" +
            "IsLocalPlayer: " + _networkObject.IsLocalPlayer + "\n" +
            "IsOwner: " + _networkObject.IsOwner + "\n" +
            "IsOwnedByServer: " + _networkObject.IsOwnedByServer;
    }
}
