using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float range;

    private float time;
    private void Update()
    {
        if (Vector3.Distance(this.transform.position, target.position) <= range)
        {
            transform.LookAt(target);
            
        }

        time += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
