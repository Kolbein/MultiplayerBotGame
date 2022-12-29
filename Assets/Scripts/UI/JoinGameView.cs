using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameView : View
{
    // TODO InputField
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _backButton;

    public override void Initialize()
    {
        _joinButton.onClick.AddListener(() => Join());
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }

    private void Join()
    {
        // TODO: Join game logic here
        NetworkManager.Singleton.StartClient();
        ViewManager.GetView<JoinGameView>().Hide();
    }
}
