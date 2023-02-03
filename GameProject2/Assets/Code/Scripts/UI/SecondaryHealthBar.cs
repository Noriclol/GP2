using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        SetUpHealthBar(canvas);
    }

    private void SetUpHealthBar(Canvas canvas)
    {
        healthBar.transform.SetParent(canvas.transform);
    }
}
