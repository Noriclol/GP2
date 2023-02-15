using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.UI;
using System;

public class PlayerDamage : NetworkBehaviour
{
    
    [Header("GameObjects, transforms etc")]
    public GameObject bulletPrefab;
    public GameObject bigBulletPrefab;
    public Transform firePoint;
    private EnergyManagement energyManager;

    [Header("Primary fire settings")]
    public float fireRate;
    public float bulletSpeed;
    public int damage;
    public float bulletEnergyCost;

    Coroutine fireCoroutine;
    WaitForSeconds rapidFireWait;

    [Header("Secondary fire settings")]
    [SerializeField] private int chargeTime;
    public int chargedDamage;
    public float bigBulletEnergyCost;
    public float bigBulletSpeed;
    private bool isCharging = false;
    private float chargeStartTime;
  

    
    private void Awake(){
        rapidFireWait = new WaitForSeconds(1 / fireRate);
    }

    void Start(){
        energyManager = GetComponent<EnergyManagement>();
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
            float chargeDuration = Time.time - chargeStartTime;
            if(chargeDuration < chargeTime){
                CMDShoot();
            }
            else{
                CMDSecondFire();
            }
            isCharging = false;
        }
    }

    void Shoot(){
        CMDShoot();
        return;
    }

    [Command]
    void CMDShoot()
    {
		if (energyManager.ConsumeEnergy(bulletEnergyCost)){

            // The player has enough energy, shoot the bullet.
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
            BulletController bc = bullet.GetComponent<BulletController>();
            NetworkServer.Spawn(bullet);
        }
    }

    [Command]

    void CMDSecondFire(){
        if (energyManager.ConsumeEnergy(bigBulletEnergyCost))
        {
            // The player has enough energy, shoot the big bullet.
            GameObject bigBullet = Instantiate(bigBulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bigBullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * bigBulletSpeed, ForceMode.Impulse);
            BulletController bc = bigBullet.GetComponent<BulletController>();
            NetworkServer.Spawn(bigBullet);
        }
    }

    public IEnumerator RapidFire(){
        float nextFire = 0f;

        while(true){
            if (Time.time >= nextFire){
            CMDShoot();
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
