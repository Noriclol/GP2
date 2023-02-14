using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class SupportFire : NetworkBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;

    private float lastShot;

    private void Awake()
    {
        lastShot = -fireRate;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (context.ReadValue<float>() > 0.5f)
        { // Pressed
            CMDShoot();
        }
    }

    [Command]
    private void CMDShoot()
    {
        if (lastShot + fireRate >= Time.time)
        {
            return;
        }

        lastShot = Time.time;

        GameObject bullet = Instantiate(bulletPrefab.gameObject, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);

        BulletController bc = bullet.GetComponent<BulletController>();

        NetworkServer.Spawn(bullet);
    }
}
