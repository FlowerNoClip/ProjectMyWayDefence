using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Caribbean_EbbtideTower : Caribbean_Lin 
{
    [SerializeField] private string towername = "Caribbean Lin_EbbtideTower"; 
    private float cooltime_ebbtide = 0f; 

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }

    private void Ebbtide(Vector3 _dir)
    {
        if(cooltime_ebbtide == 0f)
        {
            base.Active(_dir);
            cooltime_ebbtide = 1f;
            StartCoroutine(WaitCoolTime_Ebbtide()); 
        }
    }

    private IEnumerator WaitCoolTime_Ebbtide()
    {
        while(cooltime_ebbtide >= 0f)
        {
            cooltime_ebbtide -= Time.deltaTime;
            yield return new WaitForEndOfFrame(); 
        }
        cooltime_ebbtide = 0f; 
        yield break; 
    }

}
