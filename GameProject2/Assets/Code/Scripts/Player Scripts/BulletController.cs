using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    PlayerDamage PlayerDamage;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            return;
        }
        HealthScript health = collision.gameObject.GetComponent<HealthScript>();
        if (health != null)
        {
            health.healthSystem.SubtractResource(PlayerDamage.damage);
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
