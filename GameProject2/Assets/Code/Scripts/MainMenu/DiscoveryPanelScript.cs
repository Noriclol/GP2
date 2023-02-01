using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Discovery;

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
		Debug.Log(response);
	}


}
