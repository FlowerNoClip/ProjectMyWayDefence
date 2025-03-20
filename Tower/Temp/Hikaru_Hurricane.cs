using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hikaru_Hurricane : Hikaru 
{
    private bool isHurricaneSpawned = false;
    [SerializeField] private string towername = "Hikaru_Hurricane";

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }

    protected void Hurricane(Vector3 _dir)
    {
        base.Active(_dir); 
    }
}
