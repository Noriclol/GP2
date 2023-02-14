using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatFSM : MonoBehaviour
{
    private IBossCombatState state;
    
    // States
    public BC_Idle Idle = new BC_Idle();
    public BC_Attacking Attacking = new BC_Attacking();
    
    
    
    
    // Fields
    [Header("Fields")]
    public Transform target;
    
    public float decisionCooldown = 0.1f;
    public float combatRange = 5f;
    public float distanceToTarget = 0f;
    public BossCombatFSMStates stateIndicator = BossCombatFSMStates.none;
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
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
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
            case BossCombatFSMStates.none:
                break;
            case BossCombatFSMStates.Idle:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
                break;
            case BossCombatFSMStates.Attacking:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(target.transform.position, 1f);
                Gizmos.DrawLine(target.transform.position, transform.position);
                break;
            default:
                break;
        }
    }
}
