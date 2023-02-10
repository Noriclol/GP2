using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.InputSystem;


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
    private bool isPlayerDowned;

    //FLOATS
    [SerializeField] private float downedTime;
    private float countDown;
    private float scaledValue;
    private float countUp;
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

		reviveIcon.SetActive(false);

		reviveZone = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        //REVIVE BOOLS
        isPlayerDowned = false;

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
        reviveVisualizationLocation = new Vector3(0, -1, 0);
        reviveVisualization.transform.localScale = reviveVisualizationSize;
        reviveVisualization.transform.localPosition = reviveVisualizationLocation;
        reviveVisualization.SetActive(false);
    }

    private void Update()
    {
        
    }

	public void OnRevive(InputAction.CallbackContext context)
    {
		
    }

    //Gets a scaled value and changes the images fill based on the scaled value
    private void ReviveCountdown()
    {
        countDown -= Time.deltaTime;
        scaledValue = countDown / downedTime;

        if (reviveBorder != null)
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

        if (reviveBorder != null)
        {
            reviveBorder.fillAmount = scaledValue;

        }
        
        if (countUp >= reviveTime)
        {
            PlayerDown(false);
            healthScript.healthSystem.GainResource(30);
            countDown = downedTime;
            Debug.Log("player Healed");
        }

    }

    public void PlayerDown(bool toggle)
    {
        isPlayerDowned = toggle;
        reviveIcon.SetActive(toggle);
        reviveVisualization.SetActive(toggle);
    }

    //Method to enable or disable to revive icons
    private void ToggleReviveIcon(bool toggle)
    {
        if (reviveIcon != null) 
        { 
            reviveIcon.SetActive(toggle);

        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && other.gameObject != this.gameObject)
    //    {
    //        isPlayerInRange = true;
    //        Debug.Log(other.gameObject.name + " has entered " + this.gameObject.name);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && other.gameObject != this.gameObject)
    //    {
    //        isPlayerInRange = false;
    //        Debug.Log(other.gameObject.name + " has left " + this.gameObject.name);
    //    }
    //}
}
