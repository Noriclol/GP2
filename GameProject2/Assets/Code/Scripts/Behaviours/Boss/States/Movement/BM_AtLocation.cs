using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_AtLocation : IBossMoveState
{
    public IBossMoveState DoState(BossMoveFSM obj)
    {
        Debug.Log("Run AtLocation");
        return null;
    }
}
