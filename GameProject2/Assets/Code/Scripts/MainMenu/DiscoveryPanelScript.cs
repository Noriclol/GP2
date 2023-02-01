using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Discovery;
using UnityEngine.UI;

public class DiscoveryPanelScript : MonoBehaviour
{
    private NetworkDiscovery networkDiscovery;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject discoveredServerPrefab;

    void Start()
    {
        networkDiscovery = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkDiscovery>();
        networkDiscovery.enableActiveDiscovery = true;
    }

    void OnDestroy()
    {
        networkDiscovery.enableActiveDiscovery = false;
    }

    public void OnServerFound(ServerResponse response)
    {
        foreach (Transform child in content.transform)
        {
            if (child.GetComponent<DiscoveredServerScript>().serverID == response.serverId)
            {
                return;
            }
        }

        var obj = Instantiate(discoveredServerPrefab, content.transform);
        obj.GetComponent<DiscoveredServerScript>().uri = response.uri;
        obj.GetComponent<DiscoveredServerScript>().serverID = response.serverId;
        
    }

    public void ResetServerList()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
