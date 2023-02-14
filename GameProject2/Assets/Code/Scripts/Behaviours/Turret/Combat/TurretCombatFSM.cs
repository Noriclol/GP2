using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCombatFSM : MonoBehaviour
{
    private ITurretCombatState state;

    public TC_Idle Idle = new TC_Idle();
    public TC_Attack Attacking = new TC_Attack();

    //public Turret turret;
    
    [Header("Fields")]
    public Transform target;
    [Space]
    public float decisionCooldown = 0.1f;
    public float combatRange = 5f;
    public float distanceToTarget = 0f;
    
    public TurretCombatFSMStates stateIndicator;

    public void Start()
    {
        state = Idle;
        StartCoroutine(MakeDecision());
    }


    
    private IEnumerator MakeDecision()
    {
        while (true)
        {
            //Calculate values
            //distanceToTarget = Vector3.Distance(transform.position, turret.Target.transform.position);
            //Run States
            state = state.DoState(this);
            yield return new WaitForSeconds(decisionCooldown);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, combatRange);

        switch (stateIndicator)
        {
            case TurretCombatFSMStates.None:
                break;
            case TurretCombatFSMStates.Idle:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
                break;
            case TurretCombatFSMStates.Attack:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
                Gizmos.DrawLine(target.transform.position, transform.position);
                break;
            default:
                break;
        }
    }
    
    
}

