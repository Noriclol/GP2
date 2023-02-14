using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNode : MonoBehaviour
{
    public List<BossNode> Neighbours;


    public void OnDrawGizmosSelected()
    {
        foreach (var node in Neighbours)
        {
            Gizmos.color = Color.blue;
            Vector3 additive = Vector3.up * 0.1f;
            Gizmos.DrawLine(transform.position + additive, node.transform.position + additive);
        }
    }
}
