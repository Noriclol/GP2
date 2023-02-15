using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BossMoveFSM : NetworkBehaviour
{
    private IBossMoveState state;

    //States
    public BM_AtLocation AtLocation = new BM_AtLocation();
    public BM_MovingToLocation MovingToLocation = new BM_MovingToLocation();

    //Refs
    [Header("NodeReferences")]
    public List<BossNode> Nodes = new List<BossNode>();

    public BossNode TargetNode;
    public BossNode NextNode;

    public BossNode CurrentNode;
    [Space]
    [Header("Fields")]

    [SyncVar]
    private Transform target;

    public float MoveDuration = 3f;

    [Space]
    [Header("No Touch")]
    public bool Moving;
    public BossMoveFSMStates stateIndicator;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AllPlayersReadyListener(AllPlayersReady);

        CurrentNode = Nodes[0];
        state = AtLocation;

    }

    private void AllPlayersReady()
    {
        target = gameManager.players[0].transform;

        StartCoroutine(MakeDecision());
    }

    public void Update()
    {
        if (!target) return;
        if (!isServer) return;

        SetTargetNode();
    }

    private IEnumerator MakeDecision()
    {
        while (true)
        {
            state = state.DoState(this);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public BossNode FindNextNode()
    {
        bool nodeFound = false;
        BossNode foundNode = null;

        //look for Target in node neighbours
        foreach (var node in CurrentNode.Neighbours)
        {
            if (node == TargetNode)
            {
                return node;
            }
        }

        //Look for Node that will have target in its neighbours
        foreach (var node in CurrentNode.Neighbours)
        {
            foreach (var n in node.Neighbours)
            {
                //if we find that a node has target as neighbour
                if (n == TargetNode)
                {
                    return node;
                }
            }
        }
        return null;
    }

    private void SetTargetNode()
    {
        if (!isServer) return;

        float closest = 1000f;
        float measuredDistance;
        BossNode closestNode = null;
        foreach (var node in Nodes)
        {
            measuredDistance = Vector3.Distance(target.transform.position, node.transform.position);
            //Debug.Log($"{node.name} Distance from target = {measuredDistance}");
            if (measuredDistance < closest)
            {
                closest = measuredDistance;
                closestNode = node;
            }
        }

        TargetNode = closestNode;
    }
}
