using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveScript : MonoBehaviour
{

    [Header("Main Player")]
    //The reason theres two icons is because one is supposed to on their own canvas 
    //while the other is supposed to be on the other player canvas
    [SerializeField] private GameObject mainReviveIcon;
    [SerializeField] private Image mainReviveBorder;

    [Header("Second Player")]
    [SerializeField] private GameObject secondaryReviveIcon;
    [SerializeField] private Image secondaryReviveBorder;



    [Header("Revive Settings")]
    //A bool so desginers can disable timed revived or not
    [SerializeField] private bool isReviveTimed;
    [SerializeField] private float downedTime;
    //Bool to check if player is downed
    [NonSerialized] public bool isPlayerDowned;
    private float countDown;
    private float scaledValue;

    private void Start()
    {
        isPlayerDowned = false;
        isReviveTimed = true;
        downedTime = 25;
        countDown = downedTime;
    }

    private void Update()
    {
        if (isReviveTimed && isPlayerDowned)
        {
            ReviveCountdown();
        }
    }

    //Gets a scaled value and changes the images fill based on the scaled value
    private void ReviveCountdown()
    {
        countDown -= Time.deltaTime;
        scaledValue = countDown / downedTime;

        if (mainReviveBorder != null)
        {
            mainReviveBorder.fillAmount = scaledValue;

        }
        if (secondaryReviveBorder != null)
        {
            secondaryReviveBorder.fillAmount = scaledValue;

        }

    }

    //Method to enable or disable to revive icons
    public void ToggleReviveIcon(bool toggle)
    {
        if (mainReviveIcon != null) 
        { 
            mainReviveIcon.SetActive(toggle);

        }

        if (secondaryReviveIcon != null)
        {
            secondaryReviveIcon.SetActive(toggle);

        }
    }
}
