using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Idle : IBossCombatState
{
    public IBossCombatState DoState(BossCombatFSM obj)
    {
        obj.stateIndicator = BossCombatFSMStates.Idle;
        // State Run
        
        
        // State Exit
        if (obj.distanceToTarget <= obj.combatRange)
            return obj.Attacking;

        return obj.Idle;
    }
}
