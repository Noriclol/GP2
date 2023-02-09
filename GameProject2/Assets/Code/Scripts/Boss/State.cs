using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    public virtual IBossState DoState()
    {
        Debug.Log("Run Base");
        return null;
    }
}
