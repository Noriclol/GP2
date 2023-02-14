using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_Attack : ITurretCombatState
{
    public BossAttacks type;
    public ITurretCombatState DoState(TurretCombatFSM obj)
    {
        // State
        
        
        // State Exit
        return obj.TcAttackState;
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
