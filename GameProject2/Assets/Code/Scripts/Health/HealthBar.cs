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
    private HealthScript healthScript;

    private void Awake()
    {
        const string hudTag = "Hud";
        var Hud = GameObject.FindGameObjectWithTag(hudTag);

        GameObject localPlayerProfile;
        GameObject playerProfile;
        
        healthScript = GetComponent<HealthScript>();

        localPlayerProfile = Hud.transform.Find("LocalPlayerPortrait").gameObject;

        playerProfile = Hud.transform.Find("SecondPlayerPortrait").gameObject;


        localHealthBar = localPlayerProfile.transform.Find("HealthBarSlider").GetComponent<Slider>();
        healthBar = playerProfile.transform.Find("HealthBarSlider").GetComponent<Slider>();
        bossHealthBar = Hud.transform.Find("BossHealthBar").GetComponent<Slider>();

        //Debug.Log(this.gameObject.name + healthBar);
    }


    public void SetValue(float currentHealth, float maxHealth)
    {
        if (isLocalPlayer)
        {
            localHealthBar.maxValue = maxHealth;
            localHealthBar.value = currentHealth * 0.75f;

        }
        else if (!isLocalPlayer)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth * 0.75f;

        }

        //if (!gameObject.CompareTag("Player"))
        //{
        //    bossHealthBar.maxValue = maxHealth;
        //    bossHealthBar.value = currentHealth;

            
        //}

    }

    public void UpdateValue(float currentHealth)
    {
        if (isLocalPlayer)
        {
            localHealthBar.value = currentHealth * 0.75f;

        }
        else if (!isLocalPlayer)
        {
            healthBar.value = currentHealth * 0.75f;

        }

        //if (!gameObject.CompareTag("Player"))
        //{
            
        //    bossHealthBar.value = currentHealth;
        //}

    }
}
