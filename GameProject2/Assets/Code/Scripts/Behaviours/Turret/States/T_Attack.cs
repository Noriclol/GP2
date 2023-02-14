using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Attack : ITurretState
{
    public BossAttacks type;
    public ITurretState DoState(TurretFSM obj)
    {
        Debug.Log("Run Attack");
        
        
        
        return obj.attackState;
    }


    private ITurretState Shoot(TurretFSM obj)
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
