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

    public TurretNode node;
    public TurretMoveDirection direction;
    
    private void Start()
    {
        state = AtLocation;
        StartCoroutine(MakeDecicsion());
    }

    private IEnumerator MakeDecicsion()
    {
        while (true)
        {
            state = state.DoState(this);
            yield return new WaitForSeconds(0.1f);
        }
    }


    private void Right()
    {
        node = node.Right;
    }

    private void Left()
    {
        node = node.Left;
    }

    public enum TurretMoveDirection
    {
        none,
        Left,
        Right,
    }

}
