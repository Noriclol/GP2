using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFSM : MonoBehaviour
{
    private ITurretState state;

    public T_Idle idleState = new T_Idle();
    public T_Attack attackState = new T_Attack();

    public Turret turret;
    public TurretFSMStates StateViewer;
    
    
    public void Start()
    {
        state = idleState;
        StateViewer = TurretFSMStates.none;
    }

    public void Update()
    {
        state = state.DoState(this);
    }
}

