using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private OverheatHandler _overheatHandler = new OverheatHandler();
    public int damage;
    public int fireRate;
    public int overheating;

    public void Fire()
    {
        if (overheating > 100)
        {
            overheating = 100;
        }
        else if(overheating > 0)
        {
            overheating -= fireRate;
        }

        OverheatState state = _overheatHandler.CheckOverheat(overheating);

        switch (state)
        {
            case OverheatState.Cool:
                print("Weapon is cool, do nothing");
                break;
            case OverheatState.Warning:
                print("Weapon is getting hot, do something");
                break;
            case OverheatState.Overheated:
                print("Weapon is overheated, do something");
                break;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

}
