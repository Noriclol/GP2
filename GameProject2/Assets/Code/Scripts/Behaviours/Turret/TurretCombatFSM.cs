using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCombatFSM : MonoBehaviour
{
    private ITurretCombatState combatState;

    public TC_Idle TcIdleState = new TC_Idle();
    public TC_Attack TcAttackState = new TC_Attack();

    public Turret turret;
    public TurretFSMStates StateViewer;
    
    
    public void Start()
    {
        combatState = TcIdleState;
        StateViewer = TurretFSMStates.none;
    }

    public void Update()
    {
        combatState = combatState.DoState(this);
    }
}

