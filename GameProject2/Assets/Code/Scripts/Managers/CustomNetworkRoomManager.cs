using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkRoomManager : NetworkRoomManager
{
	[SerializeField] private NetworkIdentity damagePrefab;
	[SerializeField] private NetworkIdentity supportPrefab;

	[HideInInspector]
	public SelectedCharacter localPlayerCharacter = SelectedCharacter.none;

	// private bool isServer = false;

	public override void OnRoomStartServer()
	{
		base.OnRoomStartServer();
		// isServer = true;
	}

	public override void OnRoomStartClient()
	{
		base.OnRoomStartClient();

		NetworkClient.RegisterPrefab(damagePrefab.gameObject);
		NetworkClient.RegisterPrefab(supportPrefab.gameObject);
	}

	public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
	{
		var selected = roomPlayer.GetComponent<LobbyPlayerScript>().selection;
		Destroy(roomPlayer);

		if (selected == SelectedCharacter.Attack) return Instantiate(damagePrefab.gameObject);
		else return Instantiate(supportPrefab.gameObject);
	}

	public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
	{
		return Instantiate(roomPlayerPrefab.gameObject);
	}

}
