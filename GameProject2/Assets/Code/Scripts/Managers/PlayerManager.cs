using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static GameObject[] playerArray;

    [SerializeField] private GameObject[] SerializeFieldHelper;
    private void Awake()
    {
        //playerArray= new GameObject[2];
    }

    private void Start()
    {

    }

    public static void SetUpArray()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
       
    }

}
