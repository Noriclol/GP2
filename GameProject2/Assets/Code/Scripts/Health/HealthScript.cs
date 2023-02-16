using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEditor;

public class HealthScript : NetworkBehaviour, IThrowableAction
{
    //A ScriptableObject holding data

    [SerializeField] private Stats stats;

    [SyncVar]
    [NonSerialized] public float health;

    private ReviveScript reviveScript;
    private HealthBar healthBar;
    private PlayerInputController playerInputController;
    public ResourceSystem healthSystem;

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
        reviveScript = GetComponent<ReviveScript>();
        playerInputController = GetComponent<PlayerInputController>();
        stats.SetUp();

    }

    private void Start()
    {
        healthSystem = new ResourceSystem(stats.maxHealth);
        health = healthSystem.Amount;
        healthBar.SetValue(health, health);


    }


    private void FixedUpdate()
    {

        if (!isLocalPlayer) return;
        if (stats.enableHealthRegeneration && !reviveScript.isPlayerDowned)
        {
            CMDChangedHealth(stats.healthRegeneration * Time.fixedDeltaTime);
            //Debug.Log(stats.currentHealth);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BossAttack") || collision.gameObject.CompareTag("Danger") && gameObject.CompareTag("Player"))
        {

            if (isLocalPlayer)
            {
                CMDChangedHealth(-10);

            }

        }

        //if (collision.gameObject.CompareTag("Bullet") && gameObject.CompareTag("Boss"))
        //{
        //    if (isLocalPlayer) return;
        //    NotCMDChangedHealth(-25);


        //}

    }

    [ClientRpc]
    private void RPCUpdateBars()
    {
        healthBar.UpdateValue(health);
        //healthBar.UpdateValue(healthSystem.Amount);

        //Debug.Log($"Health Updated to: {healthSystem.Amount}");
    }

    [Command]
    private void CMDChangedHealth(float value)
    {
        this.health = healthSystem.ChangeValue(value);
        if (this.health == 0)
        {
            //reviveScript.PlayerDown(true);
            reviveScript.isPlayerDowned = true;
            stats.healthState = Stats.HealthState.Downed;
        }
        RPCUpdateBars();
    }

    //private void NotCMDChangedHealth(float value)
    //{
    //    this.health = healthSystem.ChangeValue(value);
    //    if (this.health == 0)
    //    {
    //        //reviveScript.PlayerDown(true);
    //        reviveScript.isPlayerDowned = true;
    //        stats.healthState = Stats.HealthState.Downed;
    //    }
    //    RPCUpdateBars();
    //}

    [ClientRpc]
    private void RPCTrowAction(ThrowableAction action, float value)
    {
        if (!isLocalPlayer) return;
        CMDChangedHealth(value);
    }

    // Only runs on the server
    public void ThrowAction(ThrowableAction action, float value)
    {
        //healthSystem.GainResource(value);
        //RPCUpdateBars();

        RPCTrowAction(action, value);
    }
}
