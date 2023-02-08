using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HealthBar : NetworkBehaviour
{
    private float currentHealth;
    private float maxHealth;
    //The reason theres two icons is because one is supposed to on their own canvas 
    //while the other is supposed to be on the other player canvas
    private Slider healthBar;

    //public HealthBar(/*float currentHealth, float maxHealth*/)
    //{
    //    //this.currentHealth = currentHealth;
    //    //this.maxHealth = maxHealth;
    //}

    private void Awake()
    {
        const string hudTag = "Hud";
        var Hud = GameObject.FindGameObjectWithTag(hudTag);

        GameObject playerProfile;
        if (isLocalPlayer)
        {
            playerProfile = Hud.transform.Find("PlayerProfile").gameObject;
        }
        else
        {
            playerProfile = Hud.transform.Find("SecondPlayerProfile").gameObject;
        }

        healthBar = playerProfile.transform.Find("PlayerHealthbar").GetComponent<Slider>();
    }

    public void SetValue(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;

        }

    }

    public void UpdateValue(float currentHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;

        }

    }
}
