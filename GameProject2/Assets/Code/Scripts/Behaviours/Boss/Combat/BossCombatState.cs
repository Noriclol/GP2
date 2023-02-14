using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossCombatState
{
    public IBossCombatState DoState(BossCombatFSM obj)
    {
        return null;
    }
}
