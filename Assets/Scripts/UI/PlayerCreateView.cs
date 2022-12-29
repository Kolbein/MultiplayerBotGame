using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreateView : View
{
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TextMeshProUGUI _errorText;

    public override void Initialize()
    {
        _createButton.onClick.AddListener(() => CreatePlayer());
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        LoadSettings();
    }

    private void CreatePlayer()
    {
        Color selectedColor = GetComponent<PlayerCreateColorPicker>().SelectedColor;

        if (_playerNameInput == null || selectedColor == null) return;

        var trimmedName = _playerNameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName) || !IsAlphaNumeric(trimmedName))
        {
            _errorText.gameObject.SetActive(true);
            return;
        }

        // Store player data
        PlayerPrefs.SetString("playerName", name);
        SettingsManager.Instance.SavePlayerColor(selectedColor);

        _errorText.gameObject.SetActive(false);
        ViewManager.ShowLast();
    }

    private void LoadSettings()
    {
        _playerNameInput.text = PlayerPrefs.GetString("playerName") ?? string.Empty;
    }

    private static bool IsAlphaNumeric(string @this)
    {
        return !Regex.IsMatch(@this, "[^a-zA-Z0-9]");
    }


}
