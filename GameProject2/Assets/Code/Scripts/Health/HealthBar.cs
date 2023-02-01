using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    [SerializeField] private Slider slider;

    //public HealthBar(/*float currentHealth, float maxHealth*/)
    //{
    //    //this.currentHealth = currentHealth;
    //    //this.maxHealth = maxHealth;
    //}

    public void SetValue(float currentHealth, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void UpdateValue(float currentHealth)
    {
        slider.value = currentHealth;
    }
}
