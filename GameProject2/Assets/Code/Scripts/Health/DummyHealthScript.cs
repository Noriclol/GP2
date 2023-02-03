using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DummyHealthScript : MonoBehaviour
{
    
    [SerializeField] private Stats stats;
    
    private HealthBar healthBar;
    private ResourceSystem healthSystem;
    private ResourceSystem manaSystem;

    private bool isPlayerDowned;
    private bool isTimerStarted;
    private bool isTimerPaused;
    private float downedTime;
    private float timeCountdown;


    private void Awake()
    {
        healthSystem = new ResourceSystem(stats.maxHealth);
        manaSystem = new ResourceSystem(stats.maxEnergy);
        healthBar = GetComponent<HealthBar>();
    }

    private void Start()
    {
        stats.SetUp();
        healthBar.SetValue(healthSystem.Amount, healthSystem.MaxAmount);
        isPlayerDowned = false;
        isTimerStarted = false;
        isTimerPaused = false;
        downedTime = 45;
        
    }

    private void Update()
    {
        #region Space to loose hp code
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    healthSystem.SubtractResource(10);
        //    //Not the best solution, probably gonna change this.
        //    if (healthSystem.CheckIfResourceIsEmpty(healthSystem.Amount))
        //    {
        //        stats.healthState = Stats.HealthState.Downed;
        //    }

        //    healthBar.UpdateValue(healthSystem.Amount);

        //}
        #endregion

        if (isPlayerDowned && !isTimerStarted)
        {
            
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && stats.healthState == Stats.HealthState.Alive)
        {
            healthSystem.GainResource(10);
            healthBar.UpdateValue(healthSystem.Amount);
            
        }
        #region Older Health System
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    stats.currentHealth = Health.ReciveDamage(stats.currentHealth, 10);
        //    Debug.Log(stats.currentHealth);
        //}

        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    stats.currentHealth = Health.ReciveDamage(stats.currentHealth, 10);
        //    Debug.Log(stats.currentHealth);
        //}
        #endregion


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
            healthSystem.SubtractResource(30);
            //Not the best solution, probably gonna change this.
            if (healthSystem.CheckIfResourceIsEmpty(healthSystem.Amount))
            {
                stats.healthState = Stats.HealthState.Downed;
            }

            healthBar.UpdateValue(healthSystem.Amount);
        }
    }

}
