using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameStateManager : NetworkBehaviour
{

    private void Update()
    {
        if (!isServer) return;

        LoseCheck();
    }

    public void LoseCheck()
    {
        foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<ReviveScript>().isPlayerDowned) { return; }
        }

        //Lose

    }

    public void WinCheck()
    {
        //If Boss is Dead
        //Win
    }

}
