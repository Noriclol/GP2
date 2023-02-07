using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiNewParent : MonoBehaviour
{
    //The gameObject we want to move
    [SerializeField] private GameObject uiGameObject;
    //The new canvas aka the new parent
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        SetUpHealthBar(canvas);
    }

    private void SetUpHealthBar(Canvas canvas)
    {
        uiGameObject.transform.SetParent(canvas.transform);
    }
}
