using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileInstance : NetworkBehaviour
{
    private Rigidbody rb;

    private Vector3 LookPos;
    void Start()
    {
        if (!isServer) return;
		Destroy(this.gameObject, 1f);
    }


    private void LateUpdate()
    {
		if (!isServer) return;

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
