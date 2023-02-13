using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.InputSystem;
using System.Net.Http.Headers;
using System.Text;

[RequireComponent(typeof(SphereCollider))]
public class ReviveScript : NetworkBehaviour
{

    private HealthScript healthScript;

    private GameObject localReviveIcon;
    private GameObject reviveIcon;

    private Image localReviveBorder;
    private Image reviveBorder;

    [SerializeField] private GameObject reviveVisualization;
    private SphereCollider reviveZone;

    [Header("Revive Settings")]
    //BOOLS
    [SyncVar] [NonSerialized] public bool isPlayerDowned;
    [SyncVar] private bool flag;
    [SyncVar] private bool testRevive;

    //FLOATS
    [SerializeField] private float downedTime;
    [SyncVar] private float countDown;
    [SyncVar] private float scaledValue;
    [SyncVar] private float countUp;
    private float reviveTime;
    private float reviveRadius;

    //VECTORS
    private Vector3 reviveVisualizationSize;
    private Vector3 reviveVisualizationLocation;

    private void Awake()
    {
        healthScript = GetComponent<HealthScript>();

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

        reviveZone = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        //REVIVE BOOLS
        isPlayerDowned = false;
        flag = true;
        testRevive = false;

        //REVIVE TIMER VARIABLES
        downedTime = 25;
        countDown = downedTime;
        countUp = 0;
        reviveTime = 5;

        //REVIVE RANGE VARIABLES
        reviveRadius = 5;
        reviveZone.isTrigger = true;
        reviveZone.radius = reviveRadius;
        reviveVisualizationSize = new Vector3(reviveRadius * 2, 0.3f, reviveRadius * 2);
        reviveVisualizationLocation = new Vector3(0, 0, 0);
        reviveVisualization.transform.localScale = reviveVisualizationSize;
        reviveVisualization.transform.localPosition = reviveVisualizationLocation;
        reviveVisualization.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerDowned && flag)
        {
            PlayerDown(true); //Wanted to run this code in HealthScript, CMDChangedHealth but it didn't work if i did that. But it works if I run it directly in ReviveScript
            flag = false;
        }
        if (isPlayerDowned && !testRevive)
        {
            ReviveCountdown();
        }

        if (isPlayerDowned && testRevive)
        {
            RevivingPlayer();
        }
    }

    

    public void OnRevive(InputAction.CallbackContext context)
    {
        if (isPlayerDowned && context.performed)
        {
            testRevive = true;
        }

        if (isPlayerDowned && context.canceled)
        {
            testRevive = false;
            countUp = 0;
        }
    }

    //Gets a scaled value and changes the images fill based on the scaled value
    private void ReviveCountdown()
    {
        countDown -= Time.deltaTime;
        scaledValue = countDown / downedTime;

        if (isLocalPlayer)
        {
            localReviveBorder.fillAmount = scaledValue;
        }

        if (!isLocalPlayer)
        {
            reviveBorder.fillAmount = scaledValue;

        }

        if (countDown <= 0)
        {
            //Replace with kill code
            Destroy(gameObject);
        }

    }

    private void RevivingPlayer()
    {
        countUp += Time.deltaTime;
        scaledValue = countUp / reviveTime;

        if (isLocalPlayer)
        {
            localReviveBorder.fillAmount = scaledValue;

        }
        if (!isLocalPlayer)
        {
            reviveBorder.fillAmount = scaledValue;

        }

        if (countUp >= reviveTime)
        {
            PlayerDown(false);
            healthScript.health = healthScript.healthSystem.GainResource(100);
            countDown = downedTime;
            //Debug.Log("player Healed");
        }

    }

    //Method to enable or disable to revive icons
    public void PlayerDown(bool toggle)
    {

        if (isLocalPlayer)
        {
            localReviveIcon.SetActive(toggle);
        }
        if (!isLocalPlayer)
        {
            reviveIcon.SetActive(toggle);
        }
        isPlayerDowned = toggle;

        reviveVisualization.SetActive(toggle);
    }

    [Command]
    private void CMDUpdateBorders()
    {
        ReviveCountdown();
    }


}
