using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveCircle : MonoBehaviour
{
    [SerializeField] private Image reviveBorder;

    private float countDown;
    private float downedTime;
    private float scaledValue;


    private void OnEnable()
    {
        downedTime = 25;
        countDown = downedTime;
    }

    private void Update()
    {

        countDown -= Time.deltaTime;

        scaledValue = countDown / downedTime;
        reviveBorder.fillAmount = scaledValue;
    }

}


