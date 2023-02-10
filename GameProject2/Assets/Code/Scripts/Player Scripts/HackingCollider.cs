using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kept in this script to make sure that only the hacking sphere collider is used.
public class HackingCollider : MonoBehaviour
{
    [SerializeField] private Hacking hackingScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHackable>(out IHackable comp))
        {
            hackingScript.TargetEntered(comp);
        }
    }

}
