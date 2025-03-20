using EpicToonFX;
using System.Collections;
using UnityEngine;

public class Hualand_FireTower : Hualand
{
    protected override void Awake()
    {
        towername = "HuaLand_FireTower";
    }
    
    protected override void Start()
    {
        base.Start(); 
        base.GetTowerInfo(towername, ranked);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if( other.gameObject.CompareTag("Monster") && towername == "HuaLand_FireTower") { 
            if (!isAttack)
            {
                StartCoroutine(Attack(nextMonsterManager.transform.position));
                isAttack = true;
            }
        }
    }

    private IEnumerator Attack(Vector3 targetPosition)
    {
        while (nextMonsterManager != null)
        {
            audiomanager.PlaySFX(0); 
            GameObject projectile = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity) as GameObject; // 선택한 총알을 생성
            projectile.transform.LookAt(nextMonsterManager.transform.position); // 총알의 회전을 몬스터를 향하도록 설정
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * bulletSpeed); // 리지드바디에 힘을 가하여 총알 속도 설정
            ETFXProjectileScript projectileScript = projectile.GetComponent<ETFXProjectileScript>();
            if (projectileScript != null)
            {
                projectileScript.damage = AttackDamage; // 현재 스크립트의 attackDamage 값으로 설정
                DotDamage(nextMonsterManager.gameObject);
            }
            yield return new WaitForSeconds(AttackSpeed); // 공격 속도만큼 대기
        }
        audiomanager.StopSFX(0); 
        isAttack = false;
        yield break;
    }

}
