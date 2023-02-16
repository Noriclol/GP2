using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Turret : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    private float range;

    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private Transform projectileSpawn;
    [SerializeField] private float timeBetweenFire = 5.0f;

    [SerializeField] private Transform basePlatform;
    [SerializeField] private Transform barrel;

    private float lastFireTimeStamp;

    private bool targetInBounds;

    [SyncVar]
    private Transform currentTarget;

    private GameManager gameManager;

    private bool ready = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AllPlayersReadyListener(AllPlayersReady);

        if (!isServer) return;
    }

    private void AllPlayersReady()
    {
        ready = true;
    }

    private void Update()
    {
        if (!ready) return;

        // Find closest player
        float minDistance = Mathf.Infinity;
        foreach(var player in gameManager.players)
        {
            var distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                currentTarget = player.transform;
            }
        }

        //Look At TargetPosition
        if (minDistance <= range)
        {
            var baseForward = (currentTarget.position - transform.position).normalized;

            basePlatform.forward = new Vector3(baseForward.x, 0.0f, baseForward.z);
            barrel.LookAt(currentTarget);
            TryShoot();
            targetInBounds = true;
        }
        else
        {
            targetInBounds = false;
        }

    }

    private void OnDrawGizmos()
    {
        if (!ready) return;
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

        if (currentTarget != null)
        {
            Gizmos.DrawWireSphere(currentTarget.position, 0.5f);
        }
    }


    private void TryShoot()
    {
        if(!isServer) return;
        if (Time.time - lastFireTimeStamp > timeBetweenFire)
        {
            lastFireTimeStamp = Time.time;
            var newBullet = Instantiate(projectile.Prefab, projectileSpawn.position, Quaternion.LookRotation(currentTarget.position));

            var rb = newBullet.GetComponent<Rigidbody>();

            rb.AddForce(projectileSpawn.forward * projectile.MuzzleSpeed, ForceMode.Impulse);

            NetworkServer.Spawn(newBullet);
        }
    }
}
