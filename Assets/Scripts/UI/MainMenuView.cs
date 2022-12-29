using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [SerializeField] private Button _createPlayerButton;
    [SerializeField] private Button _joinGameButton;
    [SerializeField] private Button _hostGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitGameButton;

    public override void Initialize()
    {
        _createPlayerButton.onClick.AddListener(() => ViewManager.Show<PlayerCreateView>());
        _joinGameButton.onClick.AddListener(() => ViewManager.Show<JoinGameView>());
        _hostGameButton.onClick.AddListener(() => HostGame());
        _settingsButton.onClick.AddListener(() => ViewManager.Show<SettingsMenuView>());
        _exitGameButton.onClick.AddListener(() => Application.Quit());
    }

    private void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        ViewManager.GetView<MainMenuView>().Hide();
    }
}
