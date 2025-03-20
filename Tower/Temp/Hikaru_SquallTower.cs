using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hikaru_SquallTower : Hikaru 
{
    [SerializeField] private string towername = "HiKaru_SquallTower";

    private float cooltime_squall = 0f;

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo("Hikaru_SquallTower", ranked);
    }

    protected void Squall(Vector3 _dir)
    {
        if(cooltime_squall == 0f)
        {
            base.Active(_dir);
            cooltime_squall = 3f;
            StartCoroutine(WaitCoolTime_SquallTower()); 
        }
    }

    private IEnumerator WaitCoolTime_SquallTower()
    {
        while(cooltime_squall >= 0)
        {
            cooltime_squall -= Time.deltaTime; 
            yield return new WaitForEndOfFrame(); 
        }
        cooltime_squall = 0f;
        yield break; 
    }


}
