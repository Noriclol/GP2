using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float range;

    [SerializeField] 
    private Projectile projectile;

    [SerializeField] 
    private Transform projectileSpawn;
    
    
    private float timeSinceFire;

    private bool targetInBounds;
    
    
    
    private void Update()
    {
        //Look At TargetPosition
        if (Vector3.Distance(this.transform.position, target.position) <= range)
        {
            transform.LookAt(target);
            targetInBounds = true;
        }
        else
        {
            targetInBounds = false;
        }
        timeSinceFire += Time.deltaTime;
    }
    
    
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);

        if (targetInBounds)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawWireSphere(target.position, 0.5f);
    }

    [ContextMenu("Shoot")]
    private void Shoot()
    {
        Debug.Log("Shooting");

        var newBullet = Instantiate(projectile.Prefab, projectileSpawn.position, Quaternion.LookRotation(target.position));

        var rb = newBullet.GetComponent<Rigidbody>();
        
        rb.AddForce(projectileSpawn.forward * projectile.MuzzleSpeed, ForceMode.Impulse);
    }
    
    
    
    
}
