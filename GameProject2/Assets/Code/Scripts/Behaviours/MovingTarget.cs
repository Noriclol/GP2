using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovingTarget : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad / 7f) * 10f, 0, Mathf.Cos(Time.timeSinceLevelLoad / 7f) * 10f) + Vector3.up;
    }
}
