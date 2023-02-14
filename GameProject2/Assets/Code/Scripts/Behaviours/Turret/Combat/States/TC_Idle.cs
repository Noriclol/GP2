using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_Idle : ITurretCombatState
{
    public ITurretCombatState DoState(TurretCombatFSM obj)
    {
        obj.stateIndicator  = TurretCombatFSMStates.idle;

        // State
        
        
        // State Exit
        if (obj.distanceToTarget <= obj.combatRange)
            return obj.Attacking;

        return obj.Idle;
    }
}
