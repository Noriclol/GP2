using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IBossState
{
    public IBossState DoState()
    {
        Debug.Log("Run Idle");
        return null;
    }
}
