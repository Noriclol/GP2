using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : IBossState
{
    public BossAttacks type;
    public IBossState DoState()
    {
        Debug.Log("Run Attack");
        
        return Shoot();
    }


    private IBossState Shoot()
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
