using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caribbean_BermudaTower : Caribbean_Lin 
{
    private float cooltime_bermuda = 0f;

    [SerializeField] private string towername = "Caribbean Lin_BermudaTower"; 
    protected override void Start()
    {
        base.Start(); 
        base.GetTowerInfo(towername, ranked); 
    }

    private void Bermuda(Vector3 _dir)
    { 
        if(cooltime_bermuda == 0f)
        {
            base.Active(_dir);
            cooltime_bermuda = 5f;
            StartCoroutine(WaitCoolTime_Bermuda()); 
        }
    }

    private IEnumerator WaitCoolTime_Bermuda()
    {
        while(cooltime_bermuda >= 0f)
        {
            cooltime_bermuda -= Time.deltaTime;
            yield return new WaitForEndOfFrame(); 
        }
        cooltime_bermuda = 0f;
        yield break; 
    }


}
