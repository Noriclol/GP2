using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthScript : MonoBehaviour
{
    //A ScriptableObject holding data
    [SerializeField] private Stats stats;

    private ReviveScript reviveScript;
    private HealthBar healthBar;
    private ResourceSystem healthSystem;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && stats.healthState == Stats.HealthState.Alive)
        {
            healthSystem.GainResource(10);
            healthBar.UpdateValue(healthSystem.Amount);

        }
        
    }
    private void FixedUpdate()
    {
        if (stats.enableHealthRegeneration && stats.healthState == Stats.HealthState.Alive)
        {
            healthSystem.PassivelyGainResource(stats.healthRegeneration);
            healthBar.UpdateValue(healthSystem.Amount);
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

}
