using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitUiFunction : MonoBehaviour
{
    [SerializeField] private Button confirmBtn = null;
    [SerializeField] private Button cancelBtn = null;


    private void Awake()
    {
        confirmBtn.onClick.AddListener( Application.Quit );
        cancelBtn.onClick.AddListener( () =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
