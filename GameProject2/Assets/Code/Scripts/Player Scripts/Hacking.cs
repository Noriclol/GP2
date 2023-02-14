using System.Collections;
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
    [SerializeField] private float activeMoveSpeed;
    [SerializeField] private PlayerInputController playerInputController;

    private bool hacking = false;
    private float originalMoveSpeed;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        originalMoveSpeed = playerInputController.moveSpeed;
    }

    public void OnHackClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (hacking) return;

        if (context.ReadValue<float>() > 0.5f)
        { // Pressed
            // Hacking started


            CMDStartHacking();
        }
    }

    private void DrainEnergy()
    {
        // Drain Energy
        // healthScript.
    }

    [Command]
    private void CMDStartHacking()
    {
        RPCStartHacking();
        DrainEnergy();
        StartCoroutine(ActiveTime());
    }

    [ClientRpc]
    private void RPCStartHacking()
    {
        hacking = true;
        hackingField.SetActive(true);

        if (isLocalPlayer)
        {
            playerInputController.moveSpeed = activeMoveSpeed;
        }
    }

    [ClientRpc]
    private void RPCStopHacking()
    {
        hacking = false;
        hackingField.SetActive(false);

        if (isLocalPlayer)
        {
            playerInputController.moveSpeed = originalMoveSpeed;
        }
    }

    IEnumerator ActiveTime()
    {
        yield return new WaitForSeconds(activeTime);

        // Hacking disabled
        RPCStopHacking();
        hacking = false;
        hackingField.SetActive(false);
    }

    public void TargetEntered(IHackable target)
    {
        if (!hacking || !isServer) return;

        target.StartedHack();
    }
}
