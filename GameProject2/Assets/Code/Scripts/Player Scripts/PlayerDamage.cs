using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.UI;
using System;

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
    private int overheating = 0;
    [SerializeField] private int cooldownRate;
    [SerializeField] private int bulletHeat;

    Coroutine fireCoroutine;
    WaitForSeconds rapidFireWait;

    [Header("Secondary fire settings")]
    [SerializeField] private float chargeTime;
    public int chargedDamage;
    public float bigBulletSpeed;
    private bool isCharging = false;
    private float chargeStartTime;
  


    private void Awake(){
        rapidFireWait = new WaitForSeconds(1 / fireRate);
    }

    public void OnFire(InputAction.CallbackContext shootContext)
    {
        Shoot();

        if(shootContext.performed){
            StartFiring();
        }

        else if (shootContext.canceled){
            StopFiring();
        }
    }

    public void OnSecondaryFire(InputAction.CallbackContext chargeContext){
        
        if(chargeContext.started){
            isCharging = true;
            chargeStartTime = Time.time;
        }
        if(chargeContext.canceled){
            if(chargeTime < 3){
                CMDShoot();
            }
            else{
                CMDSecondFire();
            }
            isCharging = false;
        }
    }

    private void Update()
    {

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
		overheating += bulletHeat;

		BulletController bc = bullet.GetComponent<BulletController>();

		NetworkServer.Spawn(bullet);
    }

    [Command]

    void CMDSecondFire(){
        GameObject bigBullet = Instantiate(bigBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bigBullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bigBulletSpeed, ForceMode.Impulse);
        BulletController bc = bigBullet.GetComponent<BulletController>();

        NetworkServer.Spawn(bigBullet);
    }

    public IEnumerator RapidFire(){
        float nextFire = 0f;

        while(true){
            if (Time.time >= nextFire){
            Shoot();
            nextFire = Time.time + 1 /fireRate;
            yield return rapidFireWait;
            }
        }
    }

    void StartFiring(){
        fireCoroutine = StartCoroutine(RapidFire());
    }


    void StopFiring(){
        if(fireCoroutine != null){
            StopCoroutine(fireCoroutine);
        }
    }
}
