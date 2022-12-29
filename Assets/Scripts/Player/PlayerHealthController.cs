using Unity.Netcode;
using UnityEngine;

public class PlayerHealthController : NetworkBehaviour
{
    public int InitialHealth = 100;
    public int MaxHealth = 100;
    public int CurrentHealth = 100;
    [SerializeField] private NetworkVariable<int> _netCurrentHealth = new(100);


    public GameObject Camera;
    private CameraShake _cameraShake;

    public override void OnNetworkSpawn()
    {
        ResetHealth();
    }

    void Start()
    {
        ResetHealth();
        _cameraShake = Camera.GetComponent<CameraShake>();
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
        {
            Debug.Log("Player died");
            CurrentHealth = 0;
        }

        _netCurrentHealth.Value = CurrentHealth;
        _cameraShake.StartShaking = true;
    }

    public void Heal(int health)
    {
        CurrentHealth = CurrentHealth + health > MaxHealth
            ? MaxHealth
            : CurrentHealth + health;
        _netCurrentHealth.Value = CurrentHealth;
    }

    public void ResetHealth()
    {
        CurrentHealth = InitialHealth;
        _netCurrentHealth.Value = CurrentHealth;
    }

    public float GetHealthPercentage()
    {
        return (float)CurrentHealth / (float) MaxHealth;
    }
}
