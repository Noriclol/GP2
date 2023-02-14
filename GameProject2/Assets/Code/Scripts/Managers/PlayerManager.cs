using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static List<GameObject> playerList = new List<GameObject>();
   [SerializeField] private List<GameObject> SerializeFieldHelper = new List<GameObject>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        SerializeFieldHelper = playerList;
    }
}
