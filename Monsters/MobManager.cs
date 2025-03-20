using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MobManager : UnitManager 
{
    [HideInInspector] public float hp = 10f;

    /*private TextMeshProUGUI hptext = null;

    private void Start()
    {
        hptext = GetComponentInChildren<TextMeshProUGUI>();
        hptext.text = "HP: " + hp;
    }*/

/*    protected override void Update()
    {
        base.Update();
        hptext.text = "HP: " + hp;
        HPBarToWorld(); 
    }*/

/*    public void HPBarToWorld()
    {
        hptext.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
    }*/
}
