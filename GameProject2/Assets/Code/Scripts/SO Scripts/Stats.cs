using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/CharacterStats", order = 1)]
public class Stats : ScriptableObject
{
    //A state to chek what... "state" the players are in. Just adding a enum here for a quick solution
    //Probably gonna swap this out for a better solution
    public enum HealthState
    {
        Alive,
        Downed,
        Reviving,
        Dead
    }

    public HealthState healthState;
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
        healthState = HealthState.Alive;
    }

}
