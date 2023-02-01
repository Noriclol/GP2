using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class MultiplayerMenuScript : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField hostInputField;
    private NetworkRoomManager networkManager;
    private NetworkDiscovery networkDiscovery;

    private void Awake()
    {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkRoomManager>();
        networkDiscovery = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkDiscovery>();
    }

    public void OnHost()
    {
        networkManager.maxConnections = 2;
        networkManager.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    public void OnLocal()
    {
        networkManager.maxConnections = 1;
        // TODO: The second local player?
        networkManager.StartHost();
    }

    public void OnJoin()
    {
        const string scheme = "kcp";
        const int port = 7777;

        var host = hostInputField.text;
        var uriBuilder = new System.UriBuilder();

        uriBuilder.Host = host;
        uriBuilder.Port = port;
        uriBuilder.Scheme = scheme;

        networkManager.StartClient(uriBuilder.Uri);
    }
}
