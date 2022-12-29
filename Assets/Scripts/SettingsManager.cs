using KinematicCharacterController.Examples;
using Unity.Netcode;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager s_instance;
    public static SettingsManager Instance
    {
        get
        {
            if (s_instance == null)
                Debug.LogError("Settings Manager is null");

            return s_instance;
        }
    }

    private void Awake()
    {
        s_instance = this;
    }

    public void SetFieldOfView(float value)
    {
        var localClient = NetworkManager.Singleton.LocalClient;
        if (localClient == null)
            return;

        var camera = localClient.PlayerObject.gameObject.GetComponentInChildren<Camera>();
        camera.fieldOfView = value;
    }

    public void SetMouseSensitivity(float value)
    {
        var cameraController = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponentInChildren<ExampleCharacterCamera>();
        cameraController.RotationSpeed = value;
    }

    public void LoadSettings()
    {
        var fov = PlayerPrefs.GetFloat("fov");
        if (fov is not default(float))
            SetFieldOfView(fov);

        var mouseSens = PlayerPrefs.GetFloat("mouseSens");
        if (mouseSens is not default(float))
            SetMouseSensitivity(mouseSens);
    }

    public void SavePlayerColor(Color color)
    {
        PlayerPrefs.SetFloat("playerColor", color.r);
        PlayerPrefs.SetFloat("playerColor", color.g);
        PlayerPrefs.SetFloat("playerColor", color.b);
    }

    public Color GetPlayerColor()
    {
        float R = PlayerPrefs.GetFloat("playerColorR");
        float G = PlayerPrefs.GetFloat("playerColorG");
        float B = PlayerPrefs.GetFloat("playerColorB");
        return new Color(R, G, B);
    }
}
