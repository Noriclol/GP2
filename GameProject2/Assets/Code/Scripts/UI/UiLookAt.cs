using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLookAt : MonoBehaviour
{

    [SerializeField] Camera playerCamera;

    void Update()
    {

        transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward, playerCamera.transform.rotation * Vector3.up);

    }
}
