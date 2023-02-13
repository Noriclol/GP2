using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveFSM : MonoBehaviour
{
    private IBossMoveState state;
    
    //States
    public BM_AtLocation AtLocation = new BM_AtLocation();
    public BM_MovingToLocation MovingToLocation = new BM_MovingToLocation();

    //Refs
    public List<BossNode> Nodes;

    public BossNode TargetNode;

    public Transform target;
    
    public void Start()
    {
        state = AtLocation;
        StartCoroutine(MakeDecision());
    }


    private IEnumerator MakeDecision()
    {
        while (true)
        {
            state = state.DoState(this);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
