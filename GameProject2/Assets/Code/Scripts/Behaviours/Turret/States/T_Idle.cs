using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Idle : ITurretState
{
    public ITurretState DoState(TurretFSM obj)
    {
        Debug.Log("Run Idle");
        return null;
    }
}
