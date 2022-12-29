using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour, IDamageable
{
    public int MaxHealth = 100;
    public int CurrentHealth = 100;
    private NetworkVariable<int> _netCurrentHealth = new(100);

    // TODO: Track position on bot on network

    public EnemyHealthBar HealthBar;
    public ParticleSystem ParticleSystem;

    public int Damage = 10;
    public float DamageDelay = 1f;

    private bool _canDealDamage = true;


    // Start is called before the first frame update
    void Start()
    {
        _netCurrentHealth.Value = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    private void OnCollisionStay(Collision collision)
    {

        if (_canDealDamage && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthController>().TakeDamage(Damage);
            StartCoroutine(DamageTimer());
        }
    }

    private IEnumerator DamageTimer()
    {
        _canDealDamage = false;
        yield return new WaitForSeconds(DamageDelay);
        _canDealDamage = true;
    }

    public void TakeDamage(int damage)
    {
        _netCurrentHealth.Value -= damage;
        HealthBar.SetHealth(_netCurrentHealth.Value);

        if (_netCurrentHealth.Value <= 0)
        {
            DeathEffect();
            DespawnEnemyServerRpc();
        }
    }

    // TODO: Make DeathEffect be used on network
    private void DeathEffect()
    {
        ParticleSystem ps = Instantiate(ParticleSystem, transform.parent);
        ps.transform.position = transform.position;
        Destroy(ps, ps.main.startLifetime.constantMax);
    }

    public bool HasBlood()
    {
        return true;
    }

    [ServerRpc]
    private void DespawnEnemyServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
