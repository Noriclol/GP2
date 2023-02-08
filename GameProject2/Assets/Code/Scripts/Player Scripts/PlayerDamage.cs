using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerDamage : NetworkBehaviour
{
    public int ammoCapacity;
    public float fireRate;
    public float bulletSpeed;
    public int damage;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public OverheatHandler overheatHandler;

    public UnityEvent OnShoot;

    private float fireCooldown;
    private int currentAmmo;
    private int overheating = 0;

    // New variable to store how much each bullet heats up the weapon
    [SerializeField] private int bulletHeat;

    public InputMaster InputActions;


    private void Start()
    {
        currentAmmo = ammoCapacity;
    }

    public void OnFire(InputAction.CallbackContext shootContext)
    {
            Shoot();
    }

    private void Update()
    {
        if(fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }

        OverheatState state = overheatHandler.CheckOverheat(overheating);
        switch (state)
        {
            case OverheatState.Cool:
                // Debug.Log("System is cool.");
                break;
            case OverheatState.Warning:
                Debug.Log("System is approaching overheating");
                break;
            case OverheatState.Overheated:
                Debug.Log("System is overheated");
                break;
            default:
                break;
        }
    }


    private void Shoot()
    {
        if (overheatHandler.CheckOverheat(overheating) != OverheatState.Overheated) 
        {
			CMDShoot();
			return;
        }
    }

    [Command]
    void CMDShoot()
    {
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Rigidbody rb = bullet.GetComponent<Rigidbody>();
		rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
		currentAmmo--;
		fireCooldown = fireRate;
		overheating += bulletHeat;

		BulletController bc = bullet.GetComponent<BulletController>();
		bc.damageAmount = damage;

		NetworkServer.Spawn(bullet);

		OnShoot.Invoke();
    }
}
