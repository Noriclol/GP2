using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject CharacterSelectionPanel;
    public GameObject LobbyPanel;

    List<Player> Players;

    public void SetPlayerCharacter(SelectedCharacter selection, Player player)
    {
        player.role = selection;
    }

    public void RefreshLobbyPanel()
    {
       // clear lobby panel

        //for each player in Players

            // Instantiate Text in lobby panel
            // Set Text to player name and ping
    }

    public void UpdateLobbyPanel()
    {
        //for each player in Players

            // update each player ping on board

    }

}


public class Player
{
    public string name;
    public SelectedCharacter role;
}