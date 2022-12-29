using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    private GunData _gunData;
    [SerializeField]
    private ParticleSystem _shootingSystem;
    [SerializeField]
    private ParticleSystem _impactParticalSystem;
    [SerializeField]
    private Transform _camPosition;
    [SerializeField]
    private Transform _bulletSpawnPosition;
    [SerializeField]
    private TrailRenderer _bulletTrail;
    [SerializeField]
    private float _bulletSpeed = 100;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClipArray;

    private float _timeSinceLastShot;

    private Animator _animator;

    private void Start()
    {
        PlayerShoot.ShootInput += Shoot;
        PlayerShoot.ReloadInput += StartReload;
        _gunData.currentAmmo = _gunData.magSize;
        UIManager.Instance.AmmoText.text = $"{_gunData.currentAmmo} / {_gunData.magSize}";
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(_camPosition.position, transform.forward * _gunData.maxDistance);
    }

    private void OnDisable() => _gunData.reloading = false;

    private bool CanShoot() => !_gunData.reloading && _timeSinceLastShot > 1f / (_gunData.fireRate / 60f);

    private void Shoot()
    {
        if (_gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                //_animator.SetBool("IsShooting", true);
                _animator.SetTrigger("Shoot");
                PlayShootingSound();

                if (_shootingSystem != null)
                    _shootingSystem.Play();

                if (Physics.Raycast(_camPosition.position, transform.forward, out RaycastHit hit, float.MaxValue))
                {
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(_gunData.damage);
                    var bloodOnImpact = damageable?.HasBlood() ?? false;

                    TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPosition.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true, bloodOnImpact));
                }
                else
                {
                    TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPosition.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, _bulletSpawnPosition.position + transform.forward * 100, Vector3.zero, false));
                }

                //_animator.SetBool("IsShooting", false);

                _gunData.currentAmmo--;
                _timeSinceLastShot = 0;
                OnGunShot();
            }
        }
        else if (!_gunData.reloading)
        {
            StartReload();
        }
    }

    public void StartReload()
    {
        if (!_gunData.reloading && gameObject.activeSelf && _gunData.currentAmmo != _gunData.magSize)
            StartCoroutine(Reload());
    }
    private IEnumerator Reload()
    {
        Debug.Log("Reloading");
        _animator.SetBool("IsReloading", true);

        _gunData.reloading = true;

        UIManager.Instance.ReloadingText.SetActive(true);

        yield return new WaitForSeconds(_gunData.reloadTime);

        Debug.Log("Stopped Reloading");
        _animator.SetBool("IsReloading", false);

        UIManager.Instance.ReloadingText.SetActive(false);

        _gunData.currentAmmo = _gunData.magSize;

        UIManager.Instance.AmmoText.text = $"{_gunData.currentAmmo} / {_gunData.magSize}";

        _gunData.reloading = false;
    }

    private void OnGunShot()
    {
        UIManager.Instance.AmmoText.text = $"{_gunData.currentAmmo} / {_gunData.magSize}";
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact, bool bloodOnImpact = false)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= _bulletSpeed * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hitPoint;
        if (madeImpact && _impactParticalSystem != null)
        {
            if (bloodOnImpact)
                Instantiate(_impactParticalSystem, hitPoint, Quaternion.LookRotation(hitNormal));
            // else => different impact system
        }

        Destroy(trail.gameObject, trail.time);
    }

    private void PlayShootingSound()
    {
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(_audioClipArray[Random.Range(0, _audioClipArray.Length)]);
        }
    }
}
