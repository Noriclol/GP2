using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damageAmount;
    public float lifeTime;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HealthScript health = collision.gameObject.GetComponent<HealthScript>();
        if (health != null)
        {
            health.healthSystem.SubtractResource(damageAmount);
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
