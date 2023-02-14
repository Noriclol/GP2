using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BM_MovingToLocation : IBossMoveState
{
    private Transform boss;
    private Transform target;
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(boss.position, target.position);
    }
    
    
    
    public IBossMoveState DoState(BossMoveFSM obj)
    {
        obj.stateIndicator = BossMoveFSMStates.movingToLocation;
        // State

        switch (obj.Moving)
        {
            case false:
                
                obj.Moving = true;
                obj.NextNode = obj.FindNextNode();

                target = obj.NextNode.transform;
                break;
            
            
            case true:
                obj.transform.DOMove(obj.NextNode.transform.position, obj.MoveDuration);
                boss = obj.transform;
                
                
                var distanceToNext =
                    Vector3.Distance(obj.NextNode.transform.position, obj.transform.transform.position);
                if (distanceToNext < 0.1f)
                {
                    obj.Moving = false;
                    obj.CurrentNode = obj.NextNode;
                }
                break;

        }
        
        // State Exit
        if (obj.NextNode == obj.CurrentNode)
        {
            return obj.AtLocation;
        }
        else
        {
            return obj.MovingToLocation;
        }
    }
}
