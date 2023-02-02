using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
	[SerializeField]
	private Button button1;

	[SerializeField]
	private Button button2;

	private NetworkRoomManager networkRoomManager;

	private void Start()
	{
		networkRoomManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkRoomManager>();

		const string lobbyPlayersTag = "LobbyPlayer";
		var players = GameObject.FindGameObjectsWithTag(lobbyPlayersTag);

		foreach (GameObject player in players)
		{
			var selection = player.GetComponent<LobbyPlayerScript>().selection;

			if (selection == SelectedCharacter.Attack)
			{
				button1.interactable = false;
			}
			else if (selection == SelectedCharacter.Attack)
			{
				button2.interactable = false;
			}
		}
	}
}