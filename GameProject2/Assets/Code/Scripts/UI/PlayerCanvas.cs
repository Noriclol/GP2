using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private Image playerIconSlot;
    [SerializeField] private Image secondPlayerIconSlot;

    [SerializeField] private Sprite playerIcon;
    [SerializeField] private Sprite secondPlayerIcon;

    private void Start()
    {
        playerIconSlot.sprite = playerIcon;
        secondPlayerIconSlot.sprite = secondPlayerIcon;
    }
}
