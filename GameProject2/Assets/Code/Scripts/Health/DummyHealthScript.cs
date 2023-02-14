//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;

//public class DummyHealthScript : MonoBehaviour
//{
//    //A ScriptableObject holding data
//    [SerializeField] private Stats stats;

//    private ReviveScript reviveScript;
//    private HealthBar healthBar;
//    private ResourceSystem healthSystem;
//    private ResourceSystem energySystem;


//    private void Awake()
//    {
//        healthSystem = new ResourceSystem(stats.maxHealth);
//        energySystem = new ResourceSystem(stats.maxEnergy);
//        healthBar = GetComponent<HealthBar>();
//        reviveScript = GetComponent<ReviveScript>();   
//    }

//    private void Start()
//    {
//        stats.SetUp();
//        healthBar.SetValue(healthSystem.Amount, healthSystem.MaxAmount);
        
//    }

//    private void Update()
//    {
//        #region Space to loose hp code
//        //if (Input.GetKeyDown(KeyCode.Space))
//        //{
//        //    healthSystem.SubtractResource(10);
//        //    //Not the best solution, probably gonna change this.
//        //    if (healthSystem.CheckIfResourceIsEmpty(healthSystem.Amount))
//        //    {
//        //        stats.healthState = Stats.HealthState.Downed;
//        //    }

//        //    healthBar.UpdateValue(healthSystem.Amount);

//        //}
//        #endregion


//        if (Input.GetKeyDown(KeyCode.LeftShift) && stats.healthState == Stats.HealthState.Alive)
//        {
//            healthSystem.GainResource(10);
//            healthBar.UpdateValue(healthSystem.Amount);
            
//        }
//        #region Older Health System
//        //if (Input.GetKeyDown(KeyCode.Space))
//        //{
//        //    stats.currentHealth = Health.ReciveDamage(stats.currentHealth, 10);
//        //    Debug.Log(stats.currentHealth);
//        //}

//        //if (Input.GetKeyDown(KeyCode.LeftShift))
//        //{
//        //    stats.currentHealth = Health.ReciveDamage(stats.currentHealth, 10);
//        //    Debug.Log(stats.currentHealth);
//        //}
//        #endregion


//    }

//    private void FixedUpdate()
//    {
//        if (stats.enableHealthRegeneration && stats.healthState == Stats.HealthState.Alive)
//        {
//            healthSystem.PassivelyGainResource(stats.healthRegeneration);
//            healthBar.UpdateValue(healthSystem.Amount);
//            //Debug.Log(stats.currentHealth);

//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Danger"))
//        {
//            //Instead of hard coding the value the attack/collision could have a damage value, obviously
//            healthSystem.SubtractResource(30);

//            if (healthSystem.CheckIfResourceIsEmpty())
//            {
//                stats.healthState = Stats.HealthState.Downed;

//                reviveScript.PlayerDown(true);
                
//            }

//            healthBar.UpdateValue(healthSystem.Amount);
//        }
//    }

//}
