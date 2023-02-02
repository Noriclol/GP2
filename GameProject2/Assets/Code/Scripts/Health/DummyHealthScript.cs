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

    private void Awake()
    {
        healthSystem = new ResourceSystem(stats.maxHealth);
        healthBar = GetComponent<HealthBar>();
    }

    private void Start()
    {
        stats.SetUp();
        healthBar.SetValue(stats.currentHealth, stats.maxHealth);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stats.currentHealth = healthSystem.SubtractResource(10);
            healthBar.UpdateValue(stats.currentHealth);

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stats.currentHealth = healthSystem.GainResource(10);
            healthBar.UpdateValue(stats.currentHealth);
            
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
        if (stats.enableHealthRegeneration)
        {
            stats.currentHealth = healthSystem.PassivelyGainResource(stats.healthRegeneration);
            healthBar.UpdateValue(stats.currentHealth);
            //Debug.Log(stats.currentHealth);

        }
    }
}
