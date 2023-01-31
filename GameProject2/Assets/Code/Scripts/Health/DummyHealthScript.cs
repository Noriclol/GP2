using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHealthScript : MonoBehaviour
{
    
    private Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    private void Start()
    {
        stats.currentHealth = stats.maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stats.currentHealth = Health.ReciveDamage(stats.currentHealth, 10);
            Debug.Log(stats.currentHealth);
        }

    }
}
