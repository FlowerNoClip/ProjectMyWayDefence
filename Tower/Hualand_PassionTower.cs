using EpicToonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hualand_PassionTower : Hualand
{
    private int cooltime_PassionTower = 0;
    private float tempAttackSpeed;

    protected override void Awake()
    {
        tempAttackSpeed = AttackSpeed;
        base.Awake();
        towername = "HuaLand_PassionTower";
    }
    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isAttack)
        {
            StartCoroutine(Attack(nextMonsterManager.transform.position));
            isAttack = true;
        }
    }

    private IEnumerator Attack(Vector3 targetPosition)
    {
        while (nextMonsterManager != null)
        {
            audiomanager.PlaySFX(0);
            GameObject projectile = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity) as GameObject; // 선택한 총알을 생성
            MonsterMovement monsterMovement = nextMonsterManager.GetComponent<MonsterMovement>();
            projectile.transform.LookAt(nextMonsterManager.transform.position); // 총알의 회전을 몬스터를 향하도록 설정
           
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * bulletSpeed); // 리지드바디에 힘을 가하여 총알 속도 설정
            ETFXProjectileScript projectileScript = projectile.GetComponent<ETFXProjectileScript>();
            
            if (projectileScript != null)
            {                
                projectileScript.damage = AttackDamage; // 현재 스크립트의 attackDamage 값으로 설정
                DotDamage(nextMonsterManager.gameObject); 
                AttackSpeed *= 0.99f;
            }
            yield return new WaitForSeconds(AttackSpeed); // 공격 속도만큼 대기
        }
        audiomanager.StopSFX(0); 
        isAttack = false;
        AttackSpeed = tempAttackSpeed;
        yield break;
    }



}
