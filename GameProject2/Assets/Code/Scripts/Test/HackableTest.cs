using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HackableTest : NetworkBehaviour, IHackable
{
    public void StartedHack()
    {
        Debug.Log("Hackad :)");

        Debug.Log($"is Server: {isServer}");
    }
}
