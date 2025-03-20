using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSkill : MonoBehaviour
{
    [SerializeField] private List<MonsterManager> monsterGetDatas = new List<MonsterManager>();
    [SerializeField] private int damageAmount = 1; // 한번에 줄 HP 양
    [SerializeField] private float damageInterval = 1f; // 데미지를 주는 간격 (초)
    [SerializeField] private Transform startPoint; // 시작 지점
    [SerializeField] private Transform endPoint; // 도착 지점
    [SerializeField] private float speed = 1f; // 이동 속도
    [SerializeField] private float amplitude = 1f; // 위아래 움직임의 진폭
    [SerializeField] private float frequency = 1f; // 위아래 움직임의 주기
    [SerializeField] private ParticleSystem tornadoParticleSystem; // 토네이도 파티클 시스템

    private float journeyLength; // 시작 지점과 도착 지점 간의 거리
    private float startTime; // 이동을 시작한 시간
    private bool isMoving = false; // 토네이도가 이동 중인지 여부

    private void Start()
    {
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
        //StartCoroutine(DamageMonstersOverTime());
    }

    private void Update()
    {
        if (!isMoving && tornadoParticleSystem.isPlaying) // 파티클 시스템이 작동 중일 때만 움직임
        {
            MoveTornado();
        }
    }

    private void MoveTornado()
    {
        // 시작 지점과 도착 지점 사이를 이동하는 루프
        StartCoroutine(MoveBetweenPoints());
    }

    private IEnumerator MoveBetweenPoints()
    {
        isMoving = true;
        float distanceCovered = 0f;
        startTime = Time.time;

        while (distanceCovered < journeyLength)
        {
            float fractionOfJourney = distanceCovered / journeyLength;
            Vector3 linearPosition = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
            float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector3(linearPosition.x, linearPosition.y + yOffset, linearPosition.z);

            // 이동한 거리 갱신
            distanceCovered = (Time.time - startTime) * speed;
            yield return null;
        }

        // 이동이 끝나면 파티클 시스템 정지 및 비활성화
        tornadoParticleSystem.Stop();
        tornadoParticleSystem.Clear();
        isMoving = false;
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Monster"))
        {
            collider.GetComponent<MonsterManager>().GetComponent<MonsterManager>().DamageByTower(damageAmount);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Monster"))
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
