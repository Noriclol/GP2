using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossMoveState
{
    public virtual IBossMoveState DoState(BossMoveFSM obj)
    {
        Debug.Log("Run Base");
        return null;
    }
}
