using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyPlayerScript : NetworkBehaviour
{
	private Button button1;
	private Button button2;

	private LobbyManager lobbyManager;

	[SyncVar(hook = nameof(SelectionChanged))]
	public SelectedCharacter selection = SelectedCharacter.none;

	void Start()
	{
		button1 = GameObject.Find("ButtonPlayer1").GetComponent<Button>();
		button2 = GameObject.Find("ButtonPlayer2").GetComponent<Button>();
		lobbyManager = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>();

		if (isLocalPlayer)
		{
			button1.onClick.AddListener(() => OnButton1Click());
			button2.onClick.AddListener(() => OnButton2Click());
		}
	}

	private void SelectionChanged(SelectedCharacter _Old, SelectedCharacter _New)
	{
		if (_New == SelectedCharacter.Attack)
		{
			button1.interactable = false;
		}
		else if (_New == SelectedCharacter.Support)
		{
			button2.interactable = false;
		}
	}

	private void OnButton1Click() // Attack select
	{
		GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
		CMDSetPlayerSelection(SelectedCharacter.Attack);
	}

	private void OnButton2Click() // Support select
	{
		GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
		CMDSetPlayerSelection(SelectedCharacter.Support);
	}

	[Command]
	private void CMDSetPlayerSelection(SelectedCharacter selected)
	{
		if (selection == SelectedCharacter.none)
		{
			selection = selected;
		}
	}
}
