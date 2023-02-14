using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretCombatState
{
    public virtual ITurretCombatState DoState(TurretCombatFSM obj)
    {
        return null;
    }
}
