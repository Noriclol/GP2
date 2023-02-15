using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BossCombatFSM : NetworkBehaviour
{
    private IBossCombatState state;

    // States
    public BC_Idle Idle = new BC_Idle();
    public BC_Attacking Attacking = new BC_Attacking();


    // References
    [Header("References")]
    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private Transform projectileSpawn;

    [Space]

    // Fields
    [Header("Fields")]
    private Transform target;

    public float decisionCooldown = 0.1f;
    public float shootCooldown = 2f;


    public float combatRange = 5f;
    public float distanceToTarget = 0f;
    public float timeSinceFire = 0f;

    private GameManager gameManager;





    public BossCombatFSMStates stateIndicator = BossCombatFSMStates.none;
    public void Start()
    {
        state = Idle;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AllPlayersReadyListener(OnPlayersReady);
    }

    private void OnPlayersReady()
    {
        if (!isServer) return;
        target = gameManager.players[0].transform;
        StartCoroutine(MakeDecision());
    }

    private IEnumerator MakeDecision()
    {
        while (true)
        {
            //Calculate values
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);  // OMG CANT FIND TARGET XD LMAO
            //Run States
            state = state.DoState(this);
            yield return new WaitForSeconds(decisionCooldown);
        }
    }


    public void TryShoot()
    {
        if (!isServer) return;
        if (timeSinceFire < Time.time + shootCooldown)
        {
            timeSinceFire = Time.time;

            projectileSpawn.transform.LookAt(target);

            var newBullet = Instantiate(projectile.Prefab, projectileSpawn.position, Quaternion.LookRotation(target.position));

            var rb = newBullet.GetComponent<Rigidbody>();

            rb.AddForce(projectileSpawn.forward * projectile.MuzzleSpeed, ForceMode.Impulse);

            NetworkServer.Spawn(newBullet);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, combatRange);

        switch (stateIndicator)
        {
            case BossCombatFSMStates.none:
                break;
            case BossCombatFSMStates.Idle:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
                break;
            case BossCombatFSMStates.Attacking:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
                Gizmos.DrawLine(target.transform.position, transform.position);
                break;
            default:
                break;
        }
    }
}
