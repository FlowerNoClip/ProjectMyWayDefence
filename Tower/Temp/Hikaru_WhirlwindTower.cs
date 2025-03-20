using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hikaru_WhirlwindTower : Hikaru 
{
    [SerializeField] private string towername = "Hikaru_WhirlwindTower"; 

    private bool isRingSpawned = false;
    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo("Hikaru_WhirlwindTower", ranked);
    }


    protected void WhirlWind(Vector3 _dir)
    {
        base.Active(_dir); 
    }




}
