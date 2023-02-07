using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DiscoveredServerScript : MonoBehaviour
{
    public System.Uri uri;
    public long serverID;

    void Start()
    {

        if (uri != null)
        {
            GetComponentInChildren<TMPro.TMP_Text>().text = uri.Host;
        }
    }

    public void OnButton()
    {
        var networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkRoomManager>();

        const string scheme = "kcp";
        const int port = 7777;

        var host = uri.Host;
        var uriBuilder = new System.UriBuilder();

        uriBuilder.Host = host;
        uriBuilder.Port = port;
        uriBuilder.Scheme = scheme;

        networkManager.StartClient(uriBuilder.Uri);
    }
}
