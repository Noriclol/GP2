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

    private float timeSinceFire;

    private bool targetInBounds;

    private List<Transform> targets = new List<Transform>();

    [SyncVar]
    private Transform currentTarget;

    private void Start()
    {
        if (!isServer) return;

		StartCoroutine(FindTargets());
		timeSinceFire = 5;
    }

    IEnumerator FindTargets()
    {
        while(true)        
        {
			var players = GameObject.FindGameObjectsWithTag("Player"); // Get all possible targets
			float minDistance = Mathf.Infinity;
			foreach (var player in players)
			{
				targets.Add(player.transform);
				var distance = Vector3.Distance(player.transform.position, transform.position);
				if (minDistance > distance)
                {
					minDistance = distance;
					currentTarget = player.transform;
				}

			}



			yield return new WaitForSeconds(0.5f);
		}
	}

    private void Update()
    {
        //Look At TargetPosition
        if (Vector3.Distance(this.transform.position, currentTarget.position) <= range)
        {
            transform.LookAt(currentTarget);
            targetInBounds = true;
            if (isServer)
            {
                TryShoot();
            }
        }
        else
        {
            targetInBounds = false;
        }

        if (!isServer) return;
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

        if (currentTarget != null)
        {
            Gizmos.DrawWireSphere(currentTarget.position, 0.5f);
        }
    }


    private void TryShoot()
    {
        if(!isServer) return;
        if (timeSinceFire > 5)
        {
            timeSinceFire = 0;
            var newBullet = Instantiate(projectile.Prefab, projectileSpawn.position, Quaternion.LookRotation(currentTarget.position));

            var rb = newBullet.GetComponent<Rigidbody>();

            rb.AddForce(projectileSpawn.forward * projectile.MuzzleSpeed, ForceMode.Impulse);

            NetworkServer.Spawn(newBullet);
        }
    }
}
