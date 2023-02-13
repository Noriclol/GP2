using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.UI;

public class PlayerDamage : NetworkBehaviour
{
    [Header("GameObjects")]
    public GameObject bulletPrefab;
    public GameObject bigBulletPrefab;
    public Transform firePoint;
    public OverheatHandler overheatHandler;
    public Slider overheatingSlider;

    [Header("Primary fire settings")]
    public float fireRate;
    public float bulletSpeed;
    public int damage;
    private float fireCooldown;
    private int overheating = 0;
    [SerializeField] private int cooldownRate;
    [SerializeField] private int bulletHeat;

    [Header("Secondary fire settings")]
    [SerializeField] private float chargeTime;
    public int chargedDamage;
    public float bigBulletSpeed;
    private bool isCharging = false;
    private float chargeStartTime;
  

    public void OnFire(InputAction.CallbackContext shootContext)
    {
        Shoot();
    }

    public void OnSecondaryFire(InputAction.CallbackContext chargeContext){
        if(chargeContext.started){
            isCharging = true;
            chargeStartTime = Time.time;
        }
        if(chargeContext.canceled){
            if(chargeTime < 2){
                Shoot();
            }
            else{
                GameObject bullet = Instantiate(bigBulletPrefab, firePoint);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.AddForce(firePoint.forward * bigBulletSpeed, ForceMode.Impulse);
            }
            isCharging = false;
        }
    }

    private void Update()
    {
        if (fireCooldown > 0)
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

        if (overheating > 0 && state != OverheatState.Overheated)
        {
            overheating -= (int)(cooldownRate * Time.deltaTime);
            // overheatingSlider.value = overheating;
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
		fireCooldown = fireRate;
		overheating += bulletHeat;

		BulletController bc = bullet.GetComponent<BulletController>();
		bc.damageAmount = damage;

		NetworkServer.Spawn(bullet);
    }
}
