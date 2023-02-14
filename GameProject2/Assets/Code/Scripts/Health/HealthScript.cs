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
    [SyncVar]
    [SerializeField] private Stats stats;

    [SyncVar]
    [NonSerialized] public float health;
    TMP_Text testText;
    TMP_Text bossText;

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

        //bossText = GameObject.Find("123abcBossText").GetComponent<TMP_Text>();

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        testText = GameObject.Find("123abc").GetComponent<TMP_Text>();


    }


    private void FixedUpdate()
    {
        //RPCUpdateBars();

        if (!isLocalPlayer) return;
        testText.text = health.ToString();

        if (!isLocalPlayer) return;
		if (stats.enableHealthRegeneration && !reviveScript.isPlayerDowned) //Using a bool in revive script but if possible i would like to use the states in stats
        {
            CMDChangedHealth(stats.healthRegeneration);
			//Debug.Log(stats.currentHealth);

		}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {

            if (isLocalPlayer)
            {
               CMDChangedHealth(-50);

            }

        }
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
