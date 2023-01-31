using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHealthScript : MonoBehaviour
{
    private float currentHealth;
    [SerializeField] private float maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth = Health.ReciveDamage(currentHealth, 10);
            Debug.Log(currentHealth);
        }

    }
}
