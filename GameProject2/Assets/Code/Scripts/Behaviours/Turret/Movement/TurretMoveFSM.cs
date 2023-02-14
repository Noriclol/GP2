using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMoveFSM : MonoBehaviour
{
    private ITurretMoveState state;
    
    //States
    public TM_AtLocation AtLocation = new TM_AtLocation();
    public TM_MovingToLocation MovingToLocation = new TM_MovingToLocation();

    private void Start()
    {
        state = AtLocation;
    }
}
