using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyHealthScript : MonoBehaviour
{
    
    [SerializeField] private Stats stats;
    private ResourceSystem healthSystem;

    private void Awake()
    {
        
        healthSystem = new ResourceSystem(stats.maxHealth);
    }

    private void Start()
    {
        stats.SetUp();
        Debug.Log(stats.currentHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stats.currentHealth = healthSystem.SubtractResource(10);
            Debug.Log(healthSystem.GetResourceAmount());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            healthSystem.GainResource(10);
            Debug.Log(healthSystem.GetResourceAmount());
        }

        #region Older Healt System
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
            Debug.Log(stats.currentHealth);

        }
    }
}
