using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class TowerManager_Temp : UnitManager
{
    private Boolean cannottowerinstalled = false;
    public Boolean CannotTowerInstalled
    {
        get { return cannottowerinstalled; }
    }
    protected float AttackSpeed = 1f;
    protected float AttackDamage = 1f;
    protected float cooltime = 1f;
    protected int ranked = 1;
    protected Boolean isRotateTower = true;
    public Boolean isTowerInstalled = false;
    protected Boolean isEnemyTargeted = false;
    protected List<TowerDTO> towerlist = new List<TowerDTO>();
    protected GameObject Targetedenemy = null;
    protected Boolean isRayon = false;
    public placementManagerT placementManagerT = null;
    public AttackRange attackRange =null;
    public virtual void TowerInstalled()
    {
        isTowerInstalled = true; //타워 설치 상태 true로 바꾸기
        GetComponentInChildren<AttackRange>().EnableCollider();
        GetComponentInChildren<AttackRange>().AddRigidbody();
        if (GetComponentInChildren<SupportRange>() != null)
        {
            GetComponentInChildren<SupportRange>().EnableCollider();
            GetComponentInChildren<SupportRange>().AddRigidBody();
        }
        isRayon = false;
    }
    protected override void Awake()
    {
        base.Awake();

        GameObject placementManager = GameObject.FindGameObjectWithTag("PlacementManager");

        if (placementManager != null)
        {
            placementManagerT = placementManager.GetComponent<placementManagerT>();
        }

        attackRange = GetComponentInChildren<AttackRange>();
    }
    protected override void Start()
    {
        PrepareDB();
    }
    protected override void Update()
    {
        base.Update();


        if(placementManagerT.testBox != null || placementManagerT.curObj != null)
        {
            attackRange.DisableCollider();
        }
        else
        {
            attackRange.EnableCollider();
        }
    }

    protected virtual void PrepareDB()
    {
        // base.ConnectDB();
        // if (isConnectDB())
        // {
        //     GetTowerData();
        // }
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
    public virtual void TargetEnemy(GameObject _target)
    {
        Targetedenemy = _target;
        isEnemyTargeted = true;
        Vector3 dir =  transform.position -Targetedenemy.transform.position;
        dir.y = 0f;
        Debug.DrawRay(transform.position, -dir, Color.red, 1f);
        StartCoroutine(AttackCoroutine(dir));
    }
    private IEnumerator AttackCoroutine(Vector3 _dir)
    {
        Attack(_dir);
        yield return new WaitForSeconds(1f);
        isEnemyTargeted = false;
        yield break;
    }
    protected virtual void Attack(Vector3 _dir)
    {
        
    }
    protected GameObject GetTargetedEnemy(GameObject _enemy)
    {
        return _enemy;
    }
    protected virtual void GetTowerInfo(String _towername, int _ranked)
    {
        foreach (TowerDTO towerdto in towerlist)
        {
            if (towerdto.Towername1.Equals(_towername) && towerdto.Ranked.Equals(_ranked))
            {
                AttackDamage = towerdto.AttackDamage;
                AttackSpeed = towerdto.AttackSpeed;
/*                Debug.Log("Towername: " + _towername);
                Debug.Log("Ranked: " + _ranked);
                Debug.Log("AttackDamage: " + AttackDamage);
                Debug.Log("AttackSpeed: " + AttackSpeed);*/
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
    protected void CheckCollision(GameObject _object, String _towername)
    {

    }



    protected void TowerUpgrade(string _towername)
    {
        if (ranked < 6)
        {
            ++ranked;
            GetTowerInfo(_towername, ranked);
            GetComponentInChildren<Aura>().UpgradeTowerColor(ranked);
        }
        isRotateTower = true;
    }
}