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

    private GameObject reviveIcon;
    private Image reviveBorder;
    [SerializeField] private GameObject reviveVisualization;
    private SphereCollider reviveZone;

    [Header("Revive Settings")]
    //A bool so desginers can disable timed revived or not
    [SerializeField] private bool isReviveTimed;
    [NonSerialized] public bool isPlayerDowned;
    [NonSerialized] public bool isPlayerBeingRevived;
    private bool isPlayerInRange;
    private bool isReveiveButtonPressed;
    [SerializeField] private float downedTime;
    private float countDown;
    private float scaledValue;
    private float countUp;
    private float reviveTime;
    private float reviveRadius;
    private Vector3 reviveVisualizationSize;
    private Vector3 reviveVisualizationLocation;

    private void Awake()
    {
		const string hudTag = "Hud";
		var Hud = GameObject.FindGameObjectWithTag(hudTag);

		GameObject playerProfile;

        if (isLocalPlayer)
        {
            playerProfile = Hud.transform.Find("PlayerProfile").gameObject;

        }
        else
        {
            playerProfile = Hud.transform.Find("SecondPlayerProfile").gameObject;

        }
            

		reviveIcon = playerProfile.transform.Find("ReviveIcon").gameObject;
		reviveBorder = reviveIcon.transform.Find("Border").GetComponent<Image>();

		reviveIcon.SetActive(false);

		reviveZone = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        isPlayerDowned = false;
        isPlayerBeingRevived = false;
        isReviveTimed = true;
        isPlayerInRange = false;
        downedTime = 25;
        countDown = downedTime;
        countUp = 0;
        reviveTime = 5;

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
        if (isReviveTimed && isPlayerDowned && !isPlayerBeingRevived)
        {
            ReviveCountdown();
        }

        if (isPlayerDowned && isPlayerBeingRevived)
        {
            RevivingPlayer();
        }
    }

	public void OnRevive(InputAction.CallbackContext context)
    {
		if (!context.performed) return;

		if (context.ReadValue<float>() > 0.5f)
		{ // Pressed
			if (isPlayerInRange) isPlayerBeingRevived = true;
            else isPlayerBeingRevived = false;
		}
		else
		{ // Released
			isPlayerBeingRevived = false;
			countUp = 0;
		}
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
            //Heal Player
            PlayerDown(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject != this.gameObject)
        {
            isPlayerInRange = true;
            Debug.Log(other.gameObject.name + " has entered " + this.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject != this.gameObject)
        {
            isPlayerInRange = false;
            Debug.Log(other.gameObject.name + " has left " + this.gameObject.name);
        }
    }
}
