using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretNode : MonoBehaviour
{
    public TurretNode Left; // Yellow
    public TurretNode Right; // Blue
    
    public void OnDrawGizmosSelected()
    {
        Vector3 additive = Vector3.up * 0.1f;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + additive, Left.transform.position + additive);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + additive, Right.transform.position + additive);
    }
    
}
