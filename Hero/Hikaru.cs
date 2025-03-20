using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Hikaru : TowerManager_Temp
{
    private Wind wind = null;
    private GameObject attackedenemy = null; 

    protected void AddLife()
    {
        if(life < 25)
        {
            Debug.Log("Hikaru Passive: Add Life"); 
            ++life; 
        }
    }

    protected virtual void Active(Vector3 _dir)
    {
        switch(name)
        {
            case "Hikaru_WindTower":
                Debug.Log("Hikaru WindTower Passive");
                WindTower(_dir); 
                break;
            case "Hikaru_RainstormTower":
                Debug.Log("Hikaru RainstormTower Passive");
                RainstormTower(_dir); 
                break;
            case "Hikaru_SquallTower":
                Debug.Log("Hikaru SquallTower Passive");
                SquallTower(_dir);
                break; 
            case "Hikaru_WhirlWindTower":
                Debug.Log("Hikaru WhirlWindTower Passive");
                WhirlWindTower(_dir); 
                break;
            case "Hikaru_HurricaneTower":
                Debug.Log("Hikaru HurricaneTower Passive");
                HurricaneTower(_dir); 
                break;
            case "Hikaru_HeroTower":
                Debug.Log("Hikaru HeroTower Passive");
                HeroTower(_dir); 
                break;
            case "Hikaru":
                break; 
            default:
                Debug.Log("Default"); 
                break; 
        }
    }

    #region["Hikaru WindTower"]
    private void WindTower(Vector3 _dir)
    {
        SpawnWindOneMoreTime(_dir); 
    }
    #endregion

    #region["Hikaru SquallTower"]
    private void SquallTower(Vector3 _dir)
    {
        GetComponentInChildren<SupportRange>().ExpandRange();
        GetComponentInChildren<SupportRange>().SupportTowerOnClick = AddTowerStat; 
    }

    private void AddTowerStat(GameObject _tower) 
    {
        _tower.GetComponent<TowerManager>().AddTowerStat("Squall", AttackDamage); 
    }
    #endregion


    #region["Hikaru MoonsoonTower"]
    private void RainstormTower(Vector3 _dir)
    {
        if(attackedenemy != null)
        {
            attackedenemy.GetComponent<Rigidbody>().AddForceAtPosition(_dir * -2f, attackedenemy.transform.position); 
        }
    }
    #endregion



    #region["Hikaru WhirlWindTower"]
    private void WhirlWindTower(Vector3 _dir)
    {
        GameObject windring = Instantiate(Resources.Load("Prefabs\\Effect\\WindRing")) as GameObject;
        Vector3 tower_pos = transform.position;
        tower_pos.z -= 2f;
        tower_pos.y = -0.2f; 
        windring.transform.position = tower_pos;
        windring.GetComponent<Wind>().EnemyAttackOnClick = EnemyAttackOnClick; 
    }
    #endregion

    #region["Hikaru HurricaneTower"]
    private void HurricaneTower(Vector3 _dir)
    {
        GameObject hurricane = Instantiate(Resources.Load("Prefabs\\Effect\\Hurricane")) as GameObject;
        Vector3 tower_pos = transform.position;
        tower_pos.y = 2.5f; 
        hurricane.transform.position = tower_pos;
        hurricane.GetComponent<Wind>().EnemyAttackOnClick = EnemyAttackOnClick;
        hurricane.GetComponent<Wind>().Shoot(_dir, AttackSpeed); 
    }
    #endregion

    #region["Hikaru HeroTower"]
    private void HeroTower(Vector3 _dir)
    {
        GameObject herowindfield = Instantiate(Resources.Load("Prefabs\\Effect\\HeroWindField")) as GameObject;
        herowindfield.GetComponent<Wind>().EnemyAttackOnClick = EnemyAttackOnClick;
        herowindfield.transform.position = new Vector3(0f, 0.3f, 0f); 
    }
    #endregion

    protected override void Attack(Vector3 _dir)
    {
        SpawnWind(_dir);
    }

    private void SpawnWind(Vector3 _dir)
    {
        wind = WindManager.GetObject();
        wind.transform.position = transform.GetComponentsInChildren<Transform>()[1].position;
        wind.EnemyAttackOnClick = EnemyAttackOnClick;
        wind.Shoot(_dir, AttackSpeed);
    }

    private void SpawnWindOneMoreTime(Vector3 _dir)
    {
        wind = WindManager.GetObject();
        Vector3 spawn_pos = GetComponentsInChildren<Transform>()[1].position;
        spawn_pos.x += 0.75f; 
        wind.transform.position = spawn_pos;
        wind.Shoot(_dir, AttackSpeed); 
    }

    protected override void Update()
    {
        base.Update(); 
    }

    public void EnemyAttackOnClick(GameObject _attackedenemy)
    {
        attackedenemy = _attackedenemy;
        _attackedenemy.GetComponent<MonsterManager>().DamageByTower(AttackDamage); 
    }

}
