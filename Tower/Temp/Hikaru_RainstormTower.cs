using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hikaru_RainstormTower : Hikaru 
{
    [SerializeField] private string towername = "Hikaru_RainstormTower"; 
    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }

    protected void Moonsoon(Vector3 _dir)
    {
        base.Active(_dir); 
    }

    protected override void Attack(Vector3 _dir)
    {
        base.Attack(_dir);
        Moonsoon(_dir);
    }
}
