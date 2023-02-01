using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem
{
    //Amount is a general name for a resource for example health, mana... heck could probabky work with coins/score
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

    public float SubtractResource(float amountToSubtract)
    {
        amount -= amountToSubtract;
        return amount;

    }

    public float GainResource(float amountToGain)
    {
        amount += amountToGain;
        AmountCheck();
        return amount;
    }

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

    private void AmountCheck()
    {
        if (amount > maxAmount)
        {
            amount = maxAmount;
        }
    }

    private void RoundUp()
    {
        amount = Mathf.Round(amount * 100.0f) * 0.01f;
    }
}
