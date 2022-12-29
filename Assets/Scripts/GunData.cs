using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{

    [Header("Info")]
    public string Name;

    [Header("Shooting")]
    public int damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;
    public float reloadTime;
    [HideInInspector] public bool reloading;

}
