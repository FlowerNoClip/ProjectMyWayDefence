using EpicToonFX;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Hualand_PenetrateTower : Hualand
{
    [SerializeField] private MonsterManager[] monsterManager = null;
    private bool isAttack2 = false;
    protected override void Awake()
    {
        base.Awake();
        towername = "HuaLand_PenetrateTower";
    }
    protected override void Start()
    {
        base.Start();
        base.GetTowerInfo(towername, ranked);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        for (int i = 0; i < monsterManager.Length; i++)
        {
            if (monsterManager[i] != null && !monsterManager[i].gameObject.activeSelf)
            {
                monsterManager[i] = null;
            }
        }
        if (other.gameObject.CompareTag("Monster"))
        {
            if (monsterManager[0] == null)
            {
                monsterManager[0] = other.gameObject.GetComponent<MonsterManager>();
            }
            else if (monsterManager[1] == null)
            {
                monsterManager[1] = other.gameObject.GetComponent<MonsterManager>();
            }
        }
        if (!isAttack && monsterManager[0] != null)
        {
            StartCoroutine(Attack(monsterManager[0].transform.position));
            isAttack = true;
        }
        if (!isAttack2 && monsterManager[1] != null)
        {
            StartCoroutine(Attack2(monsterManager[1].transform.position));
            isAttack2 = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (monsterManager[0] != null && monsterManager[0].gameObject == other.gameObject)
        {
            monsterManager[0] = null;
        }
        else if (monsterManager[1] != null && monsterManager[1].gameObject == other.gameObject)
        {
            monsterManager[1] = null;
        }
    }
    private IEnumerator Attack(Vector3 targetPosition)
    {
        while (monsterManager[0] != null)
        {
            audiomanager.PlaySFX(0);
            GameObject projectile = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(monsterManager[0].transform.position);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * bulletSpeed);
            Effect_Penetrate projectileScript = projectile.GetComponent<Effect_Penetrate>();
            if (projectileScript != null)
            {
                projectileScript.damage = AttackDamage;
                DotDamage(monsterManager[0].gameObject); // 도트 데미지 추가
            }
            yield return new WaitForSeconds(AttackSpeed);
        }
        audiomanager.StopSFX(0);
        isAttack = false;
    }
    private IEnumerator Attack2(Vector3 targetPosition)
    {
        while (monsterManager[1] != null)
        {
            audiomanager.PlaySFX(0);
            GameObject projectile = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(monsterManager[1].transform.position);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * bulletSpeed);
            Effect_Penetrate projectileScript = projectile.GetComponent<Effect_Penetrate>();
            if (projectileScript != null)
            {
                projectileScript.damage = AttackDamage;
                DotDamage(monsterManager[1].gameObject); // 도트 데미지 추가
            }
            yield return new WaitForSeconds(AttackSpeed);
        }
        audiomanager.StopSFX(0);
        isAttack2 = false;
    }
}