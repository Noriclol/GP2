using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
	[SerializeField]
	private Button attackButton;

	[SerializeField]
	private Button supportButton;

	[SerializeField]
	private GameObject secondPlayerIndicator;

	private bool _otherPlayerJoined = false;
	public bool otherPlayerJoined
	{
		get { return _otherPlayerJoined; }
		set
		{
			SetOtherPlayerIndicator(value);
			_otherPlayerJoined = value;
		}
	}

	private CustomNetworkRoomManager networkManager;

	// Makes sure that if a character has already been selected then that button will be grayed out.
	// As well as second player indicator
	private void Start()
	{
		const string lobbyPlayersTag = "LobbyPlayer";
		var players = GameObject.FindGameObjectsWithTag(lobbyPlayersTag);

		networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkRoomManager>();
		var playerCount = networkManager.numPlayers;

		if (playerCount == 2)
		{
			otherPlayerJoined = true;
		}
		else
		{
			otherPlayerJoined = false;
		}

		foreach (GameObject player in players)
		{
			var selection = player.GetComponent<LobbyPlayerScript>().selection;

			if (selection == SelectedCharacter.Attack)
			{
				attackButton.interactable = false;
			}
			else if (selection == SelectedCharacter.Attack)
			{
				supportButton.interactable = false;
			}
		}
	}

	private void SetOtherPlayerIndicator(bool joined)
	{
		if (SceneManager.GetActiveScene().name != "Lobby") return;
		if (joined)
		{
			secondPlayerIndicator.SetActive(true);
		}
		else
		{
			secondPlayerIndicator.SetActive(false);
		}

	}

	public void OnExitButtonClick()
	{
		GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkRoomManager>().StopHost();
	}
}