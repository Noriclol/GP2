using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HealthBar : NetworkBehaviour
{

    private Slider healthBar;
    private Slider localHealthBar;
    private Slider bossHealthBar;

    private void Awake()
    {
        const string hudTag = "Hud";
        var Hud = GameObject.FindGameObjectWithTag(hudTag);

        GameObject localPlayerProfile;
        GameObject playerProfile;
        


        localPlayerProfile = Hud.transform.Find("PlayerProfile").gameObject;

        playerProfile = Hud.transform.Find("SecondPlayerProfile").gameObject;


        localHealthBar = localPlayerProfile.transform.Find("PlayerHealthbar").GetComponent<Slider>();
        healthBar = playerProfile.transform.Find("PlayerHealthbar").GetComponent<Slider>();
        bossHealthBar = Hud.transform.Find("BossHealth").GetComponent<Slider>();

        //Debug.Log(this.gameObject.name + healthBar);
    }


    public void SetValue(float currentHealth, float maxHealth)
    {
        if (isLocalPlayer)
        {
            localHealthBar.maxValue = maxHealth;
            localHealthBar.value = currentHealth;

        }
        else if (!isLocalPlayer)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;

        }


    }

    public void UpdateValue(float currentHealth)
    {
        if (isLocalPlayer)
        {
            localHealthBar.value = currentHealth;

        }
        else if (!isLocalPlayer)
        {
            healthBar.value = currentHealth;

        }


    }
}
