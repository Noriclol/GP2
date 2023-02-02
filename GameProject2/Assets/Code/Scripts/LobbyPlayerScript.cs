using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyPlayerScript : NetworkBehaviour
{
    private Button Button1;
    private Button Button2;

    private LobbyManager lobbyManager;

    public void Start()
    {
        if(isLocalPlayer)
        {
            lobbyManager = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>();
            lobbyManager.SetLocalPlayer();

            Button1 = GameObject.Find("ButtonPlayer1").GetComponent<Button>();
            Button2 = GameObject.Find("ButtonPlayer2").GetComponent<Button>();

            Button1.onClick.AddListener(() => OnButton1Click());
            Button2.onClick.AddListener(() => OnButton2Click());
        }
    }

    public void OnButton1Click()
    {
        lobbyManager.SetPlayerCharacter(SelectedCharacter.Attack);
        GetComponent<NetworkRoomPlayer>().readyToBegin = true;
    }

    public void OnButton2Click()
    {
        lobbyManager.SetPlayerCharacter(SelectedCharacter.Support);
        GetComponent<NetworkRoomPlayer>().readyToBegin = true;
    }

}
