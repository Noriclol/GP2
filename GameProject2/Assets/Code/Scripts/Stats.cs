using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/CharacterStats", order = 1)]
public class Stats : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    public bool enableHealthRegeneration;
    public float healthRegeneration;
    public float currentEnergy;
    public float maxEnergy;
    public float movementSpeed;

    public void SetUp()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }
}
