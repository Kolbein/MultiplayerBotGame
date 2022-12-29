using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : View
{
    [SerializeField] private Slider _fovSlider;
    [SerializeField] private TextMeshProUGUI _fovValueText;
    [SerializeField] private Slider _mouseSensSlider;
    [SerializeField] private TextMeshProUGUI _mouseSensValueText;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;

    private float _fov;
    private float _mouseSens;

    public override void Initialize()
    {
        _saveButton.onClick.AddListener(() => Save());
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _fovSlider.onValueChanged.AddListener((value) => ChangeFov(value));
        _mouseSensSlider.onValueChanged.AddListener((value) => ChangeMouseSens(value));
        LoadPrefs();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("fov", _fov);
        PlayerPrefs.SetFloat("mouseSens", _mouseSens);
        SettingsManager.Instance.SetFieldOfView(_fov);
        SettingsManager.Instance.SetMouseSensitivity(_mouseSens);
        ViewManager.ShowLast();
    }

    private void ChangeFov(float value)
    {
        _fov = value;
        _fovValueText.text = _fov.ToString();
    }

    private void ChangeMouseSens(float value)
    {
        _mouseSens = value;
        _mouseSensValueText.text = $"{_mouseSens:0.##}";
    }

    private void LoadPrefs()
    {
        _fov = (int)ValidatedValue(PlayerPrefs.GetFloat("fov"), _fovSlider);
        _mouseSens = ValidatedValue(PlayerPrefs.GetFloat("mouseSens"), _mouseSensSlider);

        _fovValueText.text = _fov.ToString();
        _mouseSensValueText.text = _mouseSens.ToString();

        _fovSlider.value = _fov;
        _mouseSensSlider.value = _mouseSens;
    }

    private float ValidatedValue(float value, Slider slider)
    {
        if (value < slider.minValue)
            return slider.minValue;
        if (value > slider.maxValue)
            return slider.maxValue;
        return value;
    }
}
