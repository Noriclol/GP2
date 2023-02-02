using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public GameObject CharacterSelectionPanel;
    public GameObject LobbyPanel;

    List<Player> Players;

    [SyncVar(hook = nameof(UpdateLobbyPanel))]
    private SelectedCharacter player1Selected;
    [SyncVar(hook = nameof(UpdateLobbyPanel))]
    private SelectedCharacter player2Selected;

    public int localPlayer;

    [SerializeField]
    private Button button1;
    [SerializeField]
    private Button button2;

    private NetworkRoomManager networkRoomManager;

    private void Start()
    {
        networkRoomManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkRoomManager>();
    }

    [Command]
    private void CMDSetPlayerCharacter(SelectedCharacter selection, int player)
    {
        if(player == 1) { player1Selected = selection; }
        else { player2Selected = selection; }
    }

    public void SetPlayerCharacter(SelectedCharacter selection)
    {
        CMDSetPlayerCharacter(selection, localPlayer);
    }

    public void UpdateLobbyPanel(SelectedCharacter _Old, SelectedCharacter _New)
    {
        if(player1Selected == SelectedCharacter.Attack || player2Selected == SelectedCharacter.Attack)
        {
            button1.interactable = false;
        }
        else if(player1Selected == SelectedCharacter.Support || player2Selected == SelectedCharacter.Support)
        {
            button2.interactable = false;
        }

        if(networkRoomManager.allPlayersReady)
        {
            Debug.Log("Start Game");
            //play
        }
        

    }

    public void SetLocalPlayer()
    {
        localPlayer = networkRoomManager.numPlayers;
    }



}


public class Player
{
    public string name;
    public SelectedCharacter role;
}