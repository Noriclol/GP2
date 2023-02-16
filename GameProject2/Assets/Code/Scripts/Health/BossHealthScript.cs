using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class BossHealthScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateHealthBar))]
    private float health;
    private float armor;
    private bool armorDown;
    private Slider armorSlider;
    [SerializeField] private Slider healthSlider;


    private ResourceSystem healthSystem;
    private ResourceSystem armorSystem;

    private void Awake()
    {
        health = 1000;
        armor = 1000;
        healthSystem = new ResourceSystem(health);
        armorSystem = new ResourceSystem(armor);
        //healthSlider = GameObject.Find("BossHealthBar").GetComponent<Slider>();
        SetHealthBar();

    }


    void Start()
    {
    }


    void Update()
    {

    }

    private void SetHealthBar()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

    }


    private void UpdateHealthBar(float _Old, float _New)
    {

        healthSlider.value = health;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isServer)
        {
            Debug.Log("AAAAAAH");
            BulletController bulletController = collision.gameObject.GetComponent<BulletController>();
            health = healthSystem.ChangeValue(-bulletController.damage);
            if (health == 0)
            {
                Destroy(gameObject);

            }
            //CommandUpdateHealthBar();

        }
    }


}
