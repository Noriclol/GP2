using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Health
{



    public static float ReciveDamage(float health, float damageRecived)
    {

        health = health - damageRecived;

        DeathCheck(health);

        return health;


    }

    public static float ReciveHealing(float health, float maxHealth, float healingRecived)
    {
        health = health + healingRecived;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        return health;
    }

    private static void DeathCheck(float health)
    {

        if (health <= 0)
        {
            //Kill the player
            
        }

       
    }
}
