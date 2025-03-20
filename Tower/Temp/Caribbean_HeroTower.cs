using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caribbean_HeroTower : Caribbean_Lin 
{

    protected override void Awake()
    {
        base.Awake();
        towername = "Caribbean Lin_HeroTower";
    }
    protected override void Start()
    {
        base.Start(); 
        base.GetTowerInfo(towername, ranked); 
    }




}
