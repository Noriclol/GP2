using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TM_MovingToLocation : ITurretMoveState
{
    public virtual ITurretMoveState DoState(TurretMoveFSM obj)
    {
        
        
        return obj.AtLocation;
        return obj.MovingToLocation;

    }
}
