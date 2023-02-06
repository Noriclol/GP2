using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    [SerializeField] private Slider mainHealthBar;
    [SerializeField] private Slider secondaryHealthBar;

    public GameObject reviveIcon;

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
