using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class PillaroffireTowerBulletE : MonoBehaviour
    {
        [SerializeField] private MonsterManager[] monsterManager = new MonsterManager[5];
        public float damage = 0f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Monster"))
            {
                for (int i = 0; i < monsterManager.Length; i++)
                {
                    if (monsterManager[i] == null)
                    {
                        monsterManager[i] = other.GetComponent<MonsterManager>(); // 몬스터 매니저를 가져옴
                        if (monsterManager[i] != null)
                        {
                            monsterManager[i].DamageByTower(damage); // 데미지를 줌
                        }
                        break; // 데미지를 주고 나면 반복문을 종료함
                    }
                }
            }
        }
    }
}