using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    //The reason theres two icons is because one is supposed to on their own canvas 
    //while the other is supposed to be on the other player canvas
    [SerializeField] private Slider mainHealthBar;
    [SerializeField] private Slider secondaryHealthBar;

  
    //public HealthBar(/*float currentHealth, float maxHealth*/)
    //{
    //    //this.currentHealth = currentHealth;
    //    //this.maxHealth = maxHealth;
    //}

    public void SetValue(float currentHealth, float maxHealth)
    {
        if (mainHealthBar != null)
        {
            mainHealthBar.maxValue = maxHealth;
            mainHealthBar.value = currentHealth;

        }

        if (secondaryHealthBar !=  null)
        {
            secondaryHealthBar.maxValue = maxHealth;
            secondaryHealthBar.value = currentHealth;

        }
    }

    public void UpdateValue(float currentHealth)
    {
        if (mainHealthBar != null)
        {
            mainHealthBar.value = currentHealth;

        }
        if (secondaryHealthBar != null)
        {
            secondaryHealthBar.value = currentHealth;

        }
    }
}
