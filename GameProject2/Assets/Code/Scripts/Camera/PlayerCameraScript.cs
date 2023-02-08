using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : NetworkBehaviour
{
    //public Camera playerCamera;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtual;

    private void Start()
    {
        if (isLocalPlayer)
        {
            //FindObjectOfType is a "heavy" method
            cinemachineVirtual = CinemachineVirtualCamera.FindObjectOfType<CinemachineVirtualCamera>();
            cinemachineVirtual.LookAt = this.gameObject.transform;
        }
    }
}
