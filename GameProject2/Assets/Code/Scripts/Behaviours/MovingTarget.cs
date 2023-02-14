using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovingTarget : MonoBehaviour
{
    public Vector3 centerpos;
    public float speed = 1;
    public float radius = 15;
    private void Update()
    {
        transform.position = centerpos + new Vector3(Mathf.Sin(Time.timeSinceLevelLoad / (1/speed)) * radius, 0, Mathf.Cos(Time.timeSinceLevelLoad / (1/speed)) * radius) + Vector3.up;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerpos, radius);
    }
}
