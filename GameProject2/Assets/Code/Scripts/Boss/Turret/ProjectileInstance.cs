using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstance : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 LookPos;
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }


    private void LateUpdate()
    {
        rb ??= gameObject.GetComponent<Rigidbody>();
        
        var LookPos = rb.velocity.normalized;
         transform.LookAt(transform.position + LookPos);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
