using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody Bullet;
    public GameObject Barrel;
    public GameObject Camera;
    public float Speed = 20;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = Instantiate(Bullet, Barrel.transform.position, Camera.transform.rotation);
            bullet.velocity = transform.TransformDirection(new Vector3(0, 0, Speed));
        }
    }
}
