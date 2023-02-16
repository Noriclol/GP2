using Cinemachine;
using Mirror;
using UnityEngine;

public class PlayerCameraScript : NetworkBehaviour
{
    //public Camera playerCamera;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtual;

    [SerializeField] private Transform lookTarget;

    private Vector3 playerRelative;
    private Vector2 camDir;
    private float camDist = 40;

    private void Start()
    {
        
        if (isLocalPlayer)
        {
            //FindObjectOfType is a "heavy" method
            cinemachineVirtual = CinemachineVirtualCamera.FindObjectOfType<CinemachineVirtualCamera>();
            lookTarget = GameObject.Find("CameraTarget").transform;
        }
    }

    private void Update()
    {
        if(isLocalPlayer)
        {
            playerRelative = gameObject.transform.position - lookTarget.position;
            camDir = new Vector2(playerRelative.x, playerRelative.z).normalized;
            cinemachineVirtual.transform.position = lookTarget.position + new Vector3(camDir.x * camDist, 20, camDir.y * camDist);
            cinemachineVirtual.LookAt = transform;
        }
    }

}
