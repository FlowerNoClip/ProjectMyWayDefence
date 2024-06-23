using EpicToonFX;
using System.Collections;
using UnityEngine;

public class Hualand_GoblinTower : Hualand
{
    [SerializeField] private CapsuleCollider capsuleCollider;
    private WaveManager waveManager;

    protected override void Awake()
    {


    }

    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
        
    }

    protected override void Update()
    {
        base.Update();
        capsuleCollider.enabled = !(placementManagerT.testBox != null || placementManagerT.curObj != null);
    }


}
