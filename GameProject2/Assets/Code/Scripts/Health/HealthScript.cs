using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class HealthScript : NetworkBehaviour, IThrowableAction
{
    //A ScriptableObject holding data
    [SerializeField] private Stats stats;

    private ReviveScript reviveScript;
    private HealthBar healthBar;

    [SyncVar]
    private ResourceSystem healthSystem;

    [SyncVar]
    private ResourceSystem energySystem;


    private void Awake()
    {
        healthSystem = new ResourceSystem(stats.maxHealth);
        energySystem = new ResourceSystem(stats.maxEnergy);
        healthBar = GetComponent<HealthBar>();
        reviveScript = GetComponent<ReviveScript>();
    }

    private void Start()
    {
        stats.SetUp();
        healthBar.SetValue(healthSystem.Amount, healthSystem.MaxAmount);

    }

    private void FixedUpdate()
    { 
        if (!isServer) return;
		if (stats.enableHealthRegeneration && stats.healthState == Stats.HealthState.Alive)
        {
            healthSystem.PassivelyGainResource(stats.healthRegeneration);
			RPCUpdateBars();
			//Debug.Log(stats.currentHealth);

		}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            //Instead of hard coding the value the attack/collision could have a damage value, obviously
            healthSystem.SubtractResource(30);

            if (healthSystem.CheckIfResourceIsEmpty())
            {
                stats.healthState = Stats.HealthState.Downed;

                reviveScript.PlayerDown(true);

            }

            healthBar.UpdateValue(healthSystem.Amount);
        }
    }


    [ClientRpc]
    private void RPCUpdateBars() // Gets called on all clients, currently only after Throwaction as in player stands in healing field
    {
        healthBar.UpdateValue(healthSystem.Amount);
        //healthBar.UpdateValue(healthSystem.Amount);

        Debug.Log($"Health Updated to: {healthSystem.Amount}");
    }

    // Only runs on the server
    public void ThrowAction(ThrowableAction action, float value)
    {
        healthSystem.GainResource(value);
        RPCUpdateBars();
    }
}
