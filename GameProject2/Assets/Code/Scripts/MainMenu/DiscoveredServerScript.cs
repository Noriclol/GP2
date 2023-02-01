using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DiscoveredServerScript : MonoBehaviour
{
	public System.Uri uri;

	void Start()
	{

		if (uri != null)
        {
	        GetComponentInChildren<TMPro.TMP_Text>().text = uri.Host;
        }
	}

	public void OnButton()
	{
		var networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkRoomManager>();

		Debug.Log("abow");
	}
}
