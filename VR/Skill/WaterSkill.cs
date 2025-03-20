using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterSkill : MonoBehaviour
{
    [SerializeField] private List<MonsterManager> monsterGetDatas = new List<MonsterManager>();
    [SerializeField] private float damageAmount = 0.5f; // 한번에 줄 HP 양
    [SerializeField] private float damageInterval = 1f; // 데미지를 주는 간격 (초)
    public ParticleSystem[] waterParticleSystem;
    [SerializeField] private Collider waterCollider;

    private PhotonView photonView;
    private bool isParticlePlaying = false;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        waterCollider.enabled = false;
    }

    private void Update()
    {
        CheckParticlePlaying();
    }

    private void CheckParticlePlaying()
    {
        bool ColliderEnabled = false;

        for (int i = 0; i < waterParticleSystem.Length; i++)
        {
            if (waterParticleSystem[i].isPlaying)
            {
                ColliderEnabled = true;
                break;
            }
        }

        waterCollider.enabled = ColliderEnabled;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Monster"))
        {
            collider.GetComponent<MonsterManager>().DamageByTower(damageAmount);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Monster"))
        {
            MonsterManager monsterGetData = collider.gameObject.GetComponent<MonsterManager>();
            if (monsterGetData != null && monsterGetDatas.Contains(monsterGetData))
            {
                monsterGetDatas.Remove(monsterGetData);
            }
        }
    }

    private IEnumerator DamageMonstersOverTime()
    {
        while (true)
        {
            for (int i = 0; i < monsterGetDatas.Count; i++)
            {
                if (monsterGetDatas[i] != null)
                {
                    monsterGetDatas[i].DamageByTower(damageAmount);
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
