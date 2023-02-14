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

[RequireComponent(typeof(SphereCollider))]
public class ReviveScript : NetworkBehaviour
{

    private HealthScript healthScript;

    private GameObject secondPlayer;
    private ReviveScript reviveScript;

    private GameObject localReviveIcon;
    private GameObject reviveIcon;

    private Image localReviveBorder;
    private Image reviveBorder;

    [SerializeField] private GameObject reviveVisualization;
    private SphereCollider reviveZone;

    [Header("Revive Settings")]
    //BOOLS
    [SyncVar(hook = nameof(SetPlayerDownedBool))]
    [NonSerialized] public bool isPlayerDowned;
    private bool flag;
    private bool testRevive;
    private bool isPlayerCloseEnough;

    //FLOATS
    [SerializeField] private float downedTime;
    private float countDown;
    [SyncVar(hook = nameof(SetScaledValue))]
    private float scaledValue;
    private float countUp;  
    private float reviveTime;
    private float reviveRadius;

    //VECTORS
    private Vector3 reviveVisualizationSize;
    private Vector3 reviveVisualizationLocation;

    private void Awake()
    {
        PlayerManager.playerList.Add(this.gameObject);

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
        isPlayerCloseEnough = false;

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
        reviveZone.enabled = false;

    }

    private void Update()
    {

        if (isPlayerDowned && !testRevive)
        {
            ReviveCountdown();
        }

        if (isPlayerDowned && testRevive)
        {



        }
    }



    public void OnRevive(InputAction.CallbackContext context)
    {
        if (isPlayerDowned && /*isPlayerCloseEnough && isLocalPlayer &&*/ context.performed)
        {
            testRevive = true;
            //RevivingPlayer();
        }

        if (isPlayerDowned && context.canceled)
        {
            testRevive = false;
            countUp = 0;
        }
    }

    //Gets a scaled value and changes the images fill based on the scaled value
    public void ReviveCountdown()
    {
        countDown -= Time.deltaTime;
        scaledValue = countDown / downedTime;

        if (countDown <= 0)
        {
            //Replace with kill code
            Destroy(gameObject);
        }

    }

    private void ReviePlayer()
    {
        countUp += Time.deltaTime;
        scaledValue = countUp / reviveTime;

        if (countUp >= reviveTime)
        {
            isPlayerDowned = false;
            healthScript.health = healthScript.healthSystem.GainResource(healthScript.healthSystem.MaxAmount / 5);
            countDown = downedTime;
            //Debug.Log("player Healed");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this.gameObject && other.gameObject.CompareTag("Player"))
        {
            isPlayerCloseEnough = true;
            Debug.Log("Is Close Enough");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != this.gameObject && other.gameObject.CompareTag("Player"))
        {
            isPlayerCloseEnough = false;
            Debug.Log("Isn't Close Enough");
        }
    }

    private void GetSecondPlayer()
    {
        foreach (GameObject player in PlayerManager.playerList)
        {
            if (player != this.gameObject)
            {
                secondPlayer = player;
            }
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

    private void SetPlayerDownedBool(bool oldBool, bool newBool)
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

        reviveZone.enabled = isPlayerDowned;
        reviveVisualization.SetActive(isPlayerDowned);
    }

}
