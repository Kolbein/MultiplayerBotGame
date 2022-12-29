using UnityEngine;
using UnityEngine.UI;

public class PlayerCreateColorPicker : MonoBehaviour
{
    public Color SelectedColor { get; private set; }
    private Button _selectedButton;

    [SerializeField] private Button[] _colorButtons;
    [SerializeField] private GameObject _playerGameObject;

    void Start()
    {
        foreach (Button button in _colorButtons)
        {
            button.onClick.AddListener(() => ButtonClicked(button));
        }

        _colorButtons[0].Enlargen();
        _playerGameObject.GetComponent<Renderer>().sharedMaterial.color = SettingsManager.Instance.GetPlayerColor();
    }

    private void ButtonClicked(Button buttonClicked)
    {
        if (_selectedButton != buttonClicked)
        {
            buttonClicked.Enlargen();

            if (_selectedButton != null)
                _selectedButton.Shrink();

            _selectedButton = buttonClicked;
            SelectedColor = buttonClicked.colors.normalColor;
            _playerGameObject.GetComponent<Renderer>().sharedMaterial.color = buttonClicked.colors.normalColor;
        }
    }

}
public static class ButtonExtensions
{
    public static void Enlargen(this Button button)
    {
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1.2f, 1);
    }

    public static void Shrink(this Button button)
    {
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}
