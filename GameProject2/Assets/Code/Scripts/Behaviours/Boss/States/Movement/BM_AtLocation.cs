using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_AtLocation : IBossMoveState
{
    public IBossMoveState DoState(BossMoveFSM obj)
    {
        //State
        
        
        
        //State Exit
        if (obj.TargetNode == obj.CurrentNode)
        {
            return obj.AtLocation;
        }
        else
        {
            return obj.MovingToLocation;
        }
        
        return null;
    }
}
