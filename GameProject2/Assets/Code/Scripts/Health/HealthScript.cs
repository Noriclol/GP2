using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Unity.VisualScripting;

public class HealthScript : NetworkBehaviour, IThrowableAction
{
    //A ScriptableObject holding data
    [SerializeField] private Stats stats;

    [SyncVar]
    [SerializeField] private float healthTest;
    TMP_Text testText;

    private ReviveScript reviveScript;

    private HealthBar healthBar;

    private PlayerInputController playerInputController;

    [SyncVar]
    public ResourceSystem healthSystem;

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
        reviveScript = GetComponent<ReviveScript>();
        playerInputController = GetComponent<PlayerInputController>();
        healthSystem = new ResourceSystem(100);

    }

    private void Start()
    {
        healthTest = 100;
        healthBar.SetValue(healthTest, healthTest);
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
        testText.text = healthTest.ToString();

        if (!isServer) return;
		if (stats.enableHealthRegeneration && stats.healthState == Stats.HealthState.Alive)
        {
            //healthSystem.PassivelyGainResource(stats.healthRegeneration);
			//Debug.Log(stats.currentHealth);

		}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {

            if (isLocalPlayer)
            {
               CMDChangedHealth(-1);

            }


        }
    }

    [ClientRpc]
    private void RPCUpdateBars() // Gets called on all clients, currently only after Throwaction as in player stands in healing field
    {
        healthBar.UpdateValue(healthTest);
        //healthBar.UpdateValue(healthSystem.Amount);

        //Debug.Log($"Health Updated to: {healthSystem.Amount}");
    }

    [Command]
    private void CMDChangedHealth(float health) // Gets called on all clients, currently only after Throwaction as in player stands in healing field
    {
        healthTest += health;
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
