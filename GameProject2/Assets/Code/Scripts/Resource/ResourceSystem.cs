using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem
{
    //Amount is a general name for a resource for example health, mana... heck could probably work with coins/score
    private float maxAmount;
    private float amount;

    public ResourceSystem(float maxAmount)
    {
        this.maxAmount = maxAmount;
        amount = maxAmount;
    }

    public float GetResourceAmount()
    {
        return amount;
    }

    //Removes X amount 
    public float SubtractResource(float amountToSubtract)
    {
        amount -= amountToSubtract;
        return amount;

    }

    //Adds X amount
    public float GainResource(float amountToGain)
    {
        amount += amountToGain;
        AmountCheck();
        return amount;
    }

    //Passivel amount (hp, mana) overtime
    public float PassivelyGainResource(float regenerationAmount)
    {
        if (amount < maxAmount)
        {
            amount += regenerationAmount * Time.deltaTime;

            AmountCheck();

            RoundUp();
            return amount;

        }

        
        return amount;
    }

    //Used to make sure u can't go beyond the maximum value
    private void AmountCheck()
    {
        if (amount > maxAmount)
        {
            amount = maxAmount;
        }
    }

    //Rounding the number incase of printing it out, looks better without 5 decimals
    private void RoundUp()
    {
        amount = Mathf.Round(amount * 100.0f) * 0.01f;
    }
}
