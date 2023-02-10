using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Hacking : NetworkBehaviour
{
    [SerializeField] private GameObject hackingField;
    [SerializeField] private HealthScript healthScript;
    // [SerializeField] private Stats stats;
    [SerializeField] private float energyDrain;
    [SerializeField] private float activeTime;
    private bool hacking = false;

    private List<IHackable> hackedObjects;


    public void OnHackClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (context.ReadValue<float>() > 0.5f)
        { // Pressed
            // Hacking started
            hacking = true;
            hackingField.SetActive(true);

            StartHacking();
        }
    }

    private void DrainEnergy()
    {
        // Drain Energy
        // healthScript.
    }

    [Command]
    private void StartHacking()
    {
        DrainEnergy();
        StartCoroutine(ActiveTime());
    }

    [ClientRpc]
    private void StopHacking()
    {
        hacking = false;
        hackingField.SetActive(false);
    }

    IEnumerator ActiveTime()
    {
        yield return new WaitForSeconds(activeTime);

        // Hacking disabled
        StopHacking();
        hacking = false;
        hackingField.SetActive(false);
    }

    public void TargetEntered(IHackable target)
    {
        if (!hacking || !isServer) return;

        target.StartedHack();
    }
}
