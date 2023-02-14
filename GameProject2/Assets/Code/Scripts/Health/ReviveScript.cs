using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.InputSystem;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using DG.Tweening.Core.Easing;
using Newtonsoft.Json.Linq;


public class ReviveScript : NetworkBehaviour
{

    private HealthScript healthScript;

    private GameObject secondPlayer;
    [SerializeField] private GameObject[] playerArray;

    private GameObject localReviveIcon;
    private GameObject reviveIcon;

    private Image localReviveBorder;
    private Image reviveBorder;

    [SerializeField] private GameObject reviveVisualization;

    [Header("Revive Settings")]
    //BOOLS
    #region Bools
    [SyncVar(hook = nameof(RPCSetPlayerDownedBool))]
    [NonSerialized] public bool isPlayerDowned;
    private bool testRevive;
    private bool isPlayerCloseEnough;
    #endregion

    //FLOATS
    #region Floats
    [SerializeField] private float downedTime;
    private float countDown;
    [SyncVar(hook = nameof(SetScaledValue))]
    private float scaledValue;
    private float countUp;  
    private float reviveTime;
    private float reviveRadius;
    #endregion

    //VECTORS
    #region Vectors
    private Vector3 reviveVisualizationSize;
    private Vector3 reviveVisualizationLocation;
    [SerializeField] private Vector3 secondPlayerPosition;
    #endregion

    private void Awake()
    {

        healthScript = GetComponent<HealthScript>();

        #region CURSED HUD
        const string hudTag = "Hud";
        var Hud = GameObject.FindGameObjectWithTag(hudTag);

        GameObject localPlayerProfile;
        GameObject playerProfile;

        localPlayerProfile = Hud.transform.Find("PlayerProfile").gameObject;
        playerProfile = Hud.transform.Find("SecondPlayerProfile").gameObject;

        reviveIcon = playerProfile.transform.Find("ReviveIcon").gameObject;
        reviveBorder = reviveIcon.transform.Find("Border").GetComponent<Image>();

        localReviveIcon = localPlayerProfile.transform.Find("ReviveIcon").gameObject;
        localReviveBorder = localReviveIcon.transform.Find("Border").GetComponent<Image>();

        reviveIcon.SetActive(false);
        localReviveIcon.SetActive(false);
        #endregion

    }

    private void Start()
    {
        #region Setting Values
        //REVIVE BOOLS
        isPlayerDowned = false;
        testRevive = false;
        isPlayerCloseEnough = false;

        //REVIVE TIMER VARIABLES
        downedTime = 25;
        countDown = downedTime;
        countUp = 0;
        reviveTime = 5;

        //REVIVE RANGE VARIABLES
        #region RANGE VARIABLES
        reviveRadius = 5;
        reviveVisualizationSize = new Vector3(reviveRadius * 2, 0.3f, reviveRadius * 2);
        reviveVisualizationLocation = new Vector3(0, 0, 0);
        reviveVisualization.transform.localScale = reviveVisualizationSize;
        reviveVisualization.transform.localPosition = reviveVisualizationLocation;
        #endregion

        reviveVisualization.SetActive(false);
        #endregion


    }

    private void Update()
    {
        if (secondPlayer != null)
        {
            if (isPlayerDowned && Vector3.Distance(this.gameObject.transform.position, secondPlayer.transform.position) < reviveRadius)
            {
                testRevive = true;
            }

        }
        else
        {
            testRevive = false;
            countUp = 0;
        }

        if (isPlayerDowned && !testRevive)
        {
            ReviveCountdown();
        }

        if (isPlayerDowned && testRevive)
        {

            RevivePlayer();

        }

        if (playerArray.Length < 2)
        {
            playerArray = GameObject.FindGameObjectsWithTag("Player");
        }

    }

    private void FixedUpdate()
    {
    }

    //public void OnRevive(InputAction.CallbackContext context)
    //{
    //    if (isPlayerDowned && context.performed)
    //    {
    //        testRevive = true;
    //        //RevivingPlayer();
    //    }

    //    if (isPlayerDowned && context.canceled)
    //    {
    //        testRevive = false;
    //        countUp = 0;
    //    }
    //}

    //Gets a scaled value and changes the images fill based on the scaled value
    private void ReviveCountdown()
    {
        countDown -= Time.deltaTime;
        scaledValue = countDown / downedTime;

        if (countDown <= 0)
        {
            //Replace with kill code
            Destroy(gameObject);
        }

    }

    public void RevivePlayer()
    {
        countUp += Time.deltaTime;
        scaledValue = countUp / reviveTime;

        if (countUp >= reviveTime)
        {
            isPlayerDowned = false;
            testRevive = false;
            healthScript.health = healthScript.healthSystem.GainResource(healthScript.healthSystem.MaxAmount / 5);
            countDown = downedTime;
            countUp = 0;
            //Debug.Log("player Healed");
        }

    }



    

    private void SetScaledValue(float oldValue, float newValue)
    {
        scaledValue = newValue;

        if (isLocalPlayer)
        {
            localReviveBorder.fillAmount = scaledValue;

        }
        if (!isLocalPlayer)
        {
            reviveBorder.fillAmount = scaledValue;

        }
    }



    [ClientRpc] //Runs on all clients
    private void RPCSetPlayerDownedBool(bool oldBool, bool newBool)
    {
        isPlayerDowned = newBool;

        if (isLocalPlayer)
        {
            localReviveIcon.SetActive(isPlayerDowned);
        }
        if (!isLocalPlayer)
        {
            reviveIcon.SetActive(isPlayerDowned);
        }

        reviveVisualization.SetActive(isPlayerDowned);

        if (secondPlayer == null)
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
    {
        NetworkIdentity thisNetworkIdentity;
        NetworkIdentity otherNetworkIdentity;

        foreach (GameObject player in playerArray)
        {
            //thisNetworkIdentity = this.gameObject.GetComponent<NetworkIdentity>();
            //otherNetworkIdentity= player.GetComponent<NetworkIdentity>();

            //if (thisNetworkIdentity.netId != otherNetworkIdentity.netId)
            //{
            //    secondPlayer = player;
            //}

            if (this.gameObject != player)
            {
                secondPlayer = player;
            }
        }
    }

    [Command]
    private void CMDRevivePlayer()
    {
        
    }
}
