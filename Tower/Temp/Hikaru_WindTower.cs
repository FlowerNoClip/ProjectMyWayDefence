using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hikaru_WindTower : Hikaru
{
    [SerializeField] private string towername = "Hikaru_WindTower";
    private float cooltime_windtower = 0f;

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }


    protected void Wind(Vector3 _dir)
    {
        if (cooltime_windtower <= 0f)
        {
            base.Active(_dir);
            cooltime_windtower = 1.5f;
            StartCoroutine(towername);
        }
    }

    private IEnumerator WaitCoolTimeWind()
    {
        while (cooltime_windtower >= 0f)
        {
            cooltime_windtower -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cooltime_windtower = 0f;
        yield break;
    }
}

