using EpicToonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoblinTowerBullet : MonoBehaviour
{
    [SerializeField] private List<MonsterManager> monsterManager = new List<MonsterManager>();
    [SerializeField] private List<MonsterMovement> monsterMovement = new List<MonsterMovement>();
    [SerializeField] private Hualand_GoblinTower Hualand_GoblinTower;
    [SerializeField] private ParticleSystem[] newParticleSystem;
    [SerializeField] private AudioManager audiomanager = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterManager targetManager = other.GetComponent<MonsterManager>();
            if (!monsterManager.Contains(targetManager))
            {
                monsterManager.Add(targetManager);
                monsterMovement.Add(other.GetComponent<MonsterMovement>());
                int index = monsterManager.Count - 1;
                StartCoroutine(Attack(targetManager, index));
            }
            audiomanager.PlaySFX(0); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MonsterManager targetManager = other.GetComponent<MonsterManager>();
        if (monsterManager.Contains(targetManager))
        {
            int index = monsterManager.IndexOf(targetManager);
            monsterManager.RemoveAt(index);
            monsterMovement.RemoveAt(index);
        }
    }
    private IEnumerator Attack(MonsterManager targetManager, int index)
    {
        while (targetManager != null)
        {
            targetManager.DamageByTower(Hualand_GoblinTower.AttackDamage);
            yield return new WaitForSeconds(Hualand_GoblinTower.AttackSpeed);
        }
    }
}