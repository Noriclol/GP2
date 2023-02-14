using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TC_Attack : ITurretCombatState
{
    public BossAttacks type;
    public ITurretCombatState DoState(TurretCombatFSM obj)
    {
        obj.stateIndicator = TurretCombatFSMStates.attack;
        // State
        
        
        // State Exit
        if (obj.distanceToTarget <= obj.combatRange)
            return obj.Attacking;

        return obj.Idle;
    }


    private ITurretCombatState Shoot(TurretCombatFSM obj)
    {
        switch (type)
        {
            case BossAttacks.none:
                break;
            case BossAttacks.MortarAttack:
                Mortar();
                break;
            case BossAttacks.FlameThrowerAttack:
                FlameThrower();
                break;
        }

        return null;
    }

    
    private void Mortar()
    {
        
    }
    
    private void FlameThrower()
    {
        
    }
}
