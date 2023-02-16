using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    PlayerDamage PlayerDamage;
    private float startTime;
    public float damage;

    private void Start()
    {
        startTime = Time.time;
        damage = 5;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            return;
        }

        if(collision.gameObject.tag == "Bullet"){
            return;
        }
        HealthScript health = collision.gameObject.GetComponent<HealthScript>();

        if(collision.gameObject.tag == "Enemy"){
            if (health != null)
            {
                health.healthSystem.SubtractResource(PlayerDamage.damage);
            }
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if(Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
