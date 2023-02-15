using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private UnityEvent allPlayersFoundCallback;

    private GameObject[] _players;

    public GameObject[] players
    {
        get { return _players; }
    }

    private Coroutine searchForPlayersRoutine;
    // Start is called before the first frame update
    void Start()
    {
        searchForPlayersRoutine = StartCoroutine(SearchForPlayers());

    }

    public void AllPlayersReadyListener(UnityAction call)
    {
        if (_players != null && _players.Length == 2)
        {
            call.Invoke();
        }
        else
        {
            allPlayersFoundCallback.AddListener(call);
        }
    }

    IEnumerator SearchForPlayers()
    {
        while(true)
        {
            var foundPlayers = GameObject.FindGameObjectsWithTag("Player");

            if (foundPlayers.Length == 2)
            {
                _players = foundPlayers;
                allPlayersFoundCallback.Invoke();
                StopCoroutine(searchForPlayersRoutine);
            }

            yield return null;
        }
        
    }
}
