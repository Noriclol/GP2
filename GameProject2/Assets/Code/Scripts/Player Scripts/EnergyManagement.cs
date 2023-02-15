using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManagement : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float rechargeSpeed = 10f;

    private float currentEnergy;
    public Slider energySlider;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy; 
    }

    // Update is called once per frame
    void Update()
    {
        Recharge();

        if (energySlider != null)
        {
            energySlider.value = currentEnergy;

        }       
    }

    private void Recharge(){
        if(currentEnergy < maxEnergy){
            currentEnergy += rechargeSpeed * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        }
    }

    public bool ConsumeEnergy(float amount){
        if(amount <= currentEnergy){
            currentEnergy -= amount;
            return true;
        }
        else{
            return false;
        }
    }

    public float GetMaxEnergy(){
        return maxEnergy;
    }

    public float GetCurrentEnergy(){
        return currentEnergy;
    }
}
