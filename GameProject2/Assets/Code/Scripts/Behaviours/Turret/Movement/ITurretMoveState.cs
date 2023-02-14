using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretMoveState
{
    public virtual ITurretMoveState DoState(TurretMoveFSM obj)
    {
        return null;
    }
}
