using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerHealthController _playerHealthController;

    void Start()
    {
        _playerHealthController = GetComponent<PlayerHealthController>();
        UIManager.Instance.PlayerHealthText.text = "HP: ";
    }

    void Update()
    {
        UIManager.Instance.PlayerHealthValue.text = _playerHealthController?.CurrentHealth.ToString() ?? "0";
        UIManager.Instance.PlayerHealthValue.color = CalculateHealthColor();
    }

    private Color CalculateHealthColor()
    {
        var healthPercentage = _playerHealthController.GetHealthPercentage();

        if (healthPercentage >= 0.7)
            return Color.green;
        else if (healthPercentage >= 0.4)
            return Color.yellow;

        return Color.red;
    }
}
