using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caribbean_SwirlTower : Caribbean_Lin
{
    [SerializeField] private string towername = "Caribbean Lin_SwirlTower";

    private bool isSpawnedRing = false;

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }


    private void Swirl(Vector3 _dir)
    {
        if (!isSpawnedRing)
        {
            base.Active(_dir);
            isSpawnedRing = true;
        }
    }
}

