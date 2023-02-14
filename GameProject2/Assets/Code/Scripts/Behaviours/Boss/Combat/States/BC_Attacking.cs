using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Attacking : IBossCombatState
{
    public IBossCombatState DoState(BossCombatFSM obj)
    {
        obj.stateIndicator = BossCombatFSMStates.Attacking;
        // State Run
        
        // Shooty thingies
        obj.TryShoot();
        
        // State Exit
        if (obj.distanceToTarget <= obj.combatRange)
            return obj.Attacking;

        return obj.Idle;

    }
}
