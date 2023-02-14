using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TC_Attack : ITurretCombatState
{
    public ITurretCombatState DoState(TurretCombatFSM obj)
    {
        obj.stateIndicator = TurretCombatFSMStates.Attack;
        // State
        
        
        // State Exit
        if (obj.distanceToTarget <= obj.combatRange)
            return obj.Attacking;

        return obj.Idle;
    }
}
