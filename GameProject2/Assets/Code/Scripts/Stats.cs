using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float currentEnergy;
    public float maxEnergy;
    public float movementSpeed;

    private void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }
}
