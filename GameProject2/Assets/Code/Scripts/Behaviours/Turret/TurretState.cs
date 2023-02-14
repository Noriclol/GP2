using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretState
{
    public virtual ITurretState DoState(TurretFSM obj)
    {
        Debug.Log("Run Base");
        return null;
    }
}
