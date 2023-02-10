using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using System;

public class HealthScript : NetworkBehaviour, IThrowableAction
{
    //A ScriptableObject holding data
    [SerializeField] private Stats stats;

    [SyncVar]
    [NonSerialized] public float playerHealth;
    TMP_Text testText;

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
        playerHealth = healthSystem.Amount;
        healthBar.SetValue(playerHealth, playerHealth);
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
        testText.text = playerHealth.ToString();

        if (!isLocalPlayer) return;
		if (stats.enableHealthRegeneration && stats.healthState == Stats.HealthState.Alive)
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
    private void RPCUpdateBars() // Gets called on all clients, currently only after Throwaction as in player stands in healing field
    {
        healthBar.UpdateValue(playerHealth);
        //healthBar.UpdateValue(healthSystem.Amount);

        //Debug.Log($"Health Updated to: {healthSystem.Amount}");
    }

    [Command]
    private void CMDChangedHealth(float health) // Gets called on all clients, currently only after Throwaction as in player stands in healing field
    {
        playerHealth = healthSystem.ChangeValue(health);
        if (playerHealth == 0)
        {
            reviveScript.PlayerDown(true);
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
