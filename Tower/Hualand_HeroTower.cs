using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Hualand_HeroTower : Hualand
{

    protected override void Awake()
    {
        base.Awake();
        towername = "HuaLand_HeroTower";
    }
    private float cooltime_hero = 0f;

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }

    protected override void Update()
    {
        base.Update();
        /*
        if (isRayon)
        {
            GetBottom(towername);
        }
        */ 
    }


}
