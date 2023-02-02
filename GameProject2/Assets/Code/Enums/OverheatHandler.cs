using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OverheatState
{
    Cool,
    Warning,
    Overheated
}
public class OverheatHandler: MonoBehaviour
{
    public float cooldownDuration = 2.0f;
    private bool _isCoolingDown = false;
    public OverheatState CheckOverheat(int overheating)
    {
        if(overheating <= 0)
        {
            _isCoolingDown = false;
            return OverheatState.Cool;
        }
        else if(overheating > 0 && overheating <= 75)
        {
            return OverheatState.Warning;
        }
        else
        {
            if (!_isCoolingDown)
            {
                StartCoroutine(CoolDown());
            }
            return OverheatState.Overheated;
        }
    }

    private IEnumerator CoolDown()
    {
        _isCoolingDown = true;
        Debug.Log("Weapon is overheated, starting cooldown");
        yield return new WaitForSeconds(cooldownDuration);
        _isCoolingDown = false;
        Debug.Log("Weapon cooldown complete");
    }
}
