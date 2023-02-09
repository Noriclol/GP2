using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyPlayerScript : NetworkRoomPlayer
{
	private Button attackButton;
	private Button supportButton;

	[SyncVar(hook = nameof(SelectionChanged))]
	public SelectedCharacter selection = SelectedCharacter.none;

	private LobbyManager lobbyManager;

	private new void Start()
	{
		lobbyManager = GameObject.FindWithTag("LobbyManager").GetComponent<LobbyManager>();

		attackButton = GameObject.Find("AttackCharacterButton").GetComponent<Button>();
		supportButton = GameObject.Find("SupportCharacterButton").GetComponent<Button>();

		if (isLocalPlayer)
		{
			attackButton.onClick.AddListener(OnAttackButtonClick);
			supportButton.onClick.AddListener(OnSupportButtonClick);
		}
		else
		{
			lobbyManager.otherPlayerJoined = true;
		}

		DontDestroyOnLoad(this);
	}

	public override void OnClientExitRoom()
	{
		if (!isLocalPlayer)
		{
			SelectionChanged(selection, SelectedCharacter.none);
			lobbyManager.otherPlayerJoined = false;
		}
	}

	private void SelectionChanged(SelectedCharacter _Old, SelectedCharacter _New)
	{
		if (_New == SelectedCharacter.Attack)
		{
			attackButton.interactable = false;
			return;
		}

		if (_New == SelectedCharacter.Support)
		{
			supportButton.interactable = false;
			return;
		}

		if (_Old == SelectedCharacter.Attack)
		{
			attackButton.interactable = true;
		}
		else if (_Old == SelectedCharacter.Support)
		{
			supportButton.interactable = true;
		}
	}

	private void OnAttackButtonClick() // Attack select
	{
		CMDSetPlayerSelection(SelectedCharacter.Attack);
		GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
	}

	private void OnSupportButtonClick() // Support select
	{
		CMDSetPlayerSelection(SelectedCharacter.Support);
		GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
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
