using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngineInternal;

public class HandCtrl : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerActionProperty ;
    [SerializeField] private InputActionProperty gripActionProperty;
    private Animator handAnim = null;
    [SerializeField] private ParticleSystem[] effectSystem;
    [SerializeField] private ParticleManager particleManager;
    public XRController controller;


    private void Awake()
    {
        handAnim = GetComponentInChildren<Animator>();
        particleManager = FindObjectOfType<ParticleManager>();

        effectSystem = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < effectSystem.Length; i++)
        {
            particleManager.waterParticleSystem[i] = effectSystem[i];
        }

    }



    private void Update()
    {
       // GripButton();
        TriggerAnim();
        GripAnim();
    }
    
  /*  private void GripButton()
    {
        float value = gripActionProperty.action.ReadValue<float>();
        if(value > 0.8f)
        {
            Debug.Log("그립버튼");
            particleManager.WaterStart();            

        }

    }*/

    private void TriggerAnim()
    {
        float value = triggerActionProperty.action.ReadValue<float>();
        handAnim.SetFloat("Trigger",value);
    }

    private void GripAnim()
    {
        float value = gripActionProperty.action.ReadValue<float>();
        handAnim.SetFloat("Grip",value);
    }

}
