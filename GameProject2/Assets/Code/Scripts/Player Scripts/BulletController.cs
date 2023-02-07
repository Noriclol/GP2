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


        // This need to be setup properly with the right health script and damage taken method. 

        //Health health = collision.gameObject.GetComponent<Health>();
        //if(health != null)
        //{
        //    health.damageTaken(damageAmount);
        //}

        //Destroy(gameObject);
    }

    private void Update()
    {
        if(Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
