using UnityEngine;
using System.Collections;
using Mirror;
using Mirror.Discovery;

using Mono.Nat;

public class MultiplayerMenuScript : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_InputField hostInputField;
	private CustomNetworkRoomManager networkManager;
	private NetworkDiscovery networkDiscovery;

	private void Awake()
	{
		networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkRoomManager>();
		networkDiscovery = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkDiscovery>();
	}

	public void OnHost()
	{		
		StartCoroutine("UpnpPortMapping", 10.0f);
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

	IEnumerator UpnpPortMapping(float maxTime)
	{
		NatUtility.DeviceFound += DeviceFound;
		NatUtility.StartDiscovery();

		Debug.Log($"Upnp Searching: {NatUtility.IsSearching}");

		while (maxTime > 0.0f)
		{
			if (!NatUtility.IsSearching) 
			{
				yield break;
			}
			
			maxTime -= 0.5f;
			yield return new WaitForSeconds(.5f);
		}

		NatUtility.StopDiscovery();
		Debug.Log("Upnp Port forwarding has failes.");
		yield break;
	}

	async void DeviceFound(object sender, DeviceEventArgs args)
	{
		NatUtility.StopDiscovery();
		INatDevice device = args.Device;
		await device.CreatePortMapAsync(new Mapping(Protocol.Udp, 7777, 7777));
		Debug.Log("Upnp has successfully port forwarded.");
	}
}
