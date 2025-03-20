using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caribbean_RaindropTower : Caribbean_Lin 
{
    [SerializeField] private string towername = "Caribbean Lin_RaindropTower";

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }

    private void Raindrop(Vector3 _dir)
    {
        base.Active(_dir); 
    }


}
