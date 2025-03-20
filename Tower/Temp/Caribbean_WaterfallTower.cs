using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caribbean_WaterfallTower : Caribbean_Lin 
{
    private float cooltime_waterfall = 0f;

    [SerializeField] private string towername = "Caribbean Lin_WaterfallTower";

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }

    
    private void Caribbean_WaterFall(Vector3 _dir)
    {
        if(cooltime_waterfall == 0f)
        {
            base.Active(_dir);
            cooltime_waterfall = 10f;
            StartCoroutine(WaitCoolTime_WaterFall()); 
        }
    }

    private IEnumerator WaitCoolTime_WaterFall()
    {
        while(cooltime_waterfall >= 0f)
        {
            cooltime_waterfall -= Time.deltaTime;
            yield return new WaitForEndOfFrame(); 
        }
        cooltime_waterfall = 0f; 
        yield break; 
    }


}
