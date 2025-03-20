using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hikaru_HeroTower : Hikaru 
{
    private Boolean isFieldSpawned = false;
    [SerializeField] private string towername = "Hikaru_HeroTower"; 

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked); 
    }

    protected void Hikaru_Hikaru(Vector3 _dir)
    {
        base.Active(_dir); 
    }

}
