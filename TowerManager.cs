using EpicToonFX;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : UnitManager
{
    // 몬스터 매니저를 저장할 변수
    [SerializeField] public MonsterManager nextMonsterManager = null;
    [SerializeField] public placementManagerT placementManagerT = null;
    [SerializeField] private CapsuleCollider attackRange = null; 
    [SerializeField] public CapsuleCollider upgradeRange = null; 
    public List<TowerDTO> towerlist = new List<TowerDTO>();
    [SerializeField] public string towername = "HuaLand_FireTower";
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject spawn;
    [SerializeField] private Aura aura = null; 
    public float AttackSpeed = 1f;
    public float AttackDamage = 1f;
    public float cooltime = 1f;
    public int ranked = 1;
    protected float dotdamage = 1f;
    public Vector2Int towerNum = Vector2Int.zero;

    public bool isAttack = false;   

    public float bulletSpeed =3000f;

    [SerializeField] protected AudioManager audiomanager = null;
    //0: Tower Sound, 1: Upgrade Sound

    protected override void Awake()
    {
        DisableUpgradeRange();
    }

    public void EnableUpgradeRange()
    {
        upgradeRange.enabled = true; 
    }

    public void DisableUpgradeRange()
    {
        upgradeRange.enabled = false;
    }

    protected override void Start()
    {
        GameObject placementManager = GameObject.FindGameObjectWithTag("PlacementManager");
        if (placementManager != null)
        {
            placementManagerT = placementManager.GetComponent<placementManagerT>();
        }
        PrepareDB();
        GetTowerInfo(towername, ranked);
    }

    private void PrepareDB()
    {
        ConnectDB();
        if (isConnectDB())
        {
            GetTowerData();
        }
    }

    // 트리거가 감지되었을 때 호출되는 함수
    protected virtual void OnTriggerEnter(Collider other)
    {
        // 트리거된 객체의 태그가 "Monster"인지 확인
        if (other.gameObject.CompareTag("Monster"))
        {
            if (nextMonsterManager == null)
            {
                nextMonsterManager = other.gameObject.GetComponent<MonsterManager>();

                return;
            }
        }
    }

    // 트리거가 종료되었을 때 호출되는 함수
    private void OnTriggerExit(Collider other)
    {
        if (nextMonsterManager != null && nextMonsterManager.gameObject == other.gameObject)
        {
            nextMonsterManager = null;
        }
    }


    private void GetTowerData()
    {
        string sql = "SELECT * FROM tower_data where Towername Like '%" + heroname + "%'";
        base.ConnectDB();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TowerDTO towerdto = new TowerDTO();
                        towerdto.Towername1 = reader.GetString(1);
                        towerdto.Ranked = reader.GetInt32(2);
                        towerdto.AttackDamage = reader.GetInt32(3);
                        towerdto.AttackSpeed = reader.GetFloat(4);
                        towerdto.Cost = reader.GetInt32(5);
                        towerlist.Add(towerdto);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Query Error: " + e.Message);
        }
    }

    protected virtual void GetTowerInfo(String _towername, int _ranked)
    {
        foreach (TowerDTO towerdto in towerlist)
        {
            if (towerdto.Towername1.Equals(_towername) && towerdto.Ranked.Equals(_ranked))
            {
                AttackDamage = towerdto.AttackDamage;
                AttackSpeed = towerdto.AttackSpeed;
                Debug.Log("Upgraded Ranked: " + towerdto.Ranked); 
                Debug.Log("Upgraded AttackDamage: " + towerdto.AttackDamage);
                Debug.Log("Upgraded AttackSpeed: " + towerdto.AttackSpeed); 
                break;
            }
        }
       
    }

    public void AddTowerStat(string _type, float Attack)
    {
        float originalAttackDamage = AttackDamage;
        float originalAttackSpeed = AttackSpeed;
        switch (_type)
        {
            case "Squall":
                AttackDamage += Attack;
                break;
            case "Ebbtide":
                AttackDamage += (AttackDamage * (1f / 10f));
                AttackSpeed += (AttackSpeed * (1f / 10f));
                break;
            case "Raindrop":
                AttackSpeed += (AttackSpeed * (1f / 10f));
                break;
            case "HikaruUlt":
                Debug.Log("Tower AttackSpeed Before Buffered By HikaruUlt: " + AttackSpeed);
                AttackSpeed *= 2f;
                Debug.Log("Tower AttackSpeed After Buffered By HikaruUlt: " + AttackSpeed);
                break;
            default:
                break;
        }
        StartCoroutine(ReturnOriginalStat(originalAttackDamage, originalAttackSpeed));
    }

    private IEnumerator ReturnOriginalStat(float _originalAttackDamage, float _originalAttackSpeed)
    {
        float waittime = 0f;
        while (waittime <= 3f)
        {
            waittime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        AttackDamage = _originalAttackDamage;
        AttackSpeed = _originalAttackSpeed;
        yield break;
    }

    protected override void Update()
    {
        if (placementManagerT.testBox != null || placementManagerT.curObj != null)
        {
            attackRange.enabled = false;
        }
        else
        {
            attackRange.enabled = true;
        }


        // monsterManager가 null이 아닌 경우 몬스터를 바라봄
        if (nextMonsterManager != null)
        {
            if (!nextMonsterManager.gameObject.activeSelf)
            {
                nextMonsterManager = null;
                return;
            }

            // 타워의 현재 위치
            Vector3 towerPosition = transform.position;
            // 몬스터의 현재 위치
            Vector3 monsterPosition = nextMonsterManager.transform.position;

            // 타워의 Y축 회전을 계산
            Vector3 direction = monsterPosition - towerPosition;
            direction.y = 0; // Y축은 무시하고 수평 방향만 고려

            MonsterMovement monsterMovement = nextMonsterManager.GetComponent<MonsterMovement>();

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Vector3 adjustedRotation = new Vector3(-90, targetRotation.eulerAngles.y, 0); // Z값을 항상 +1만큼 더 증가
                transform.rotation = Quaternion.Euler(adjustedRotation);
            }
        }

    }

    public void TowerUpgrade(string _towername)
    {
        if (ranked < 6)
        {
            ++ranked;
            GetTowerInfo(_towername, ranked);
            aura.UpgradeTowerColor(ranked);
            upgradeRange.enabled = false;
            audiomanager.PlaySFX(1); 
        }
    }

    public int GetRanked()
    {
        return ranked; 
    }
}
