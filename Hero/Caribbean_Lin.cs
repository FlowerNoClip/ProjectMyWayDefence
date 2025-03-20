using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Caribbean_Lin : TowerManager
{
    private GameObject attackedenemy = null;
    private Water water = null;

    protected void Slow()
    {
        //attackedenemy º”µµ ∞®º“ 
        if(attackedenemy != null)
        {

        }
    }

    protected void Active(Vector3 _dir)
    {
        switch (name)
        {
            case "Caribbean_EbbtideTower":
                Debug.Log("Caribbean_Ebbtide Active Skill");
                Ebbtide(); 
                break;
            case "Caribbean_RaindropTower":
                Debug.Log("Caribbean_Raindrop Active Skill");
                Raindrop(); 
                break; 
            case "Caribbean_BermudaTower":
                Debug.Log("Caribbean_Bermuda Active Skill");
                Caribbean_BermudaTower(); 
                break;
            case "Caribbean_WaterFallTower":
                Debug.Log("Caribbean_Waterfall Active Skill");
                Caribbean_WaterFallTower(_dir); 
                break;
            case "Caribbean_SwirlTower":
                Debug.Log("Caribbean_SwirlTower Active Skill");
                Swirl(_dir); 
                break;
            case "Caribbean_HeroTower":
                Debug.Log("Caribbean_HeroTower Active Skill");
                SpawnWater(_dir, "BigWave");
                break; 
            default:
                Debug.Log("Default: " + gameObject.transform.parent.name); 
                break; 
        }
    }




    #region["Bermuda Tower"] 
    private void Caribbean_BermudaTower()
    {
        Water bermuda = WaterManager.GetBermuda(); 
        //bermuda.transform.position = Targetedenemy.transform.position;
        bermuda.EnemyAttackOnClick = EnemyAttackOnClick; 
    }
    #endregion

    #region["WaterFall Tower"]
    private void Caribbean_WaterFallTower(Vector3 _dir)
    {
        Water waterfall = WaterManager.GetWaterFall();
        waterfall.transform.position = transform.GetComponentsInChildren<Transform>()[1].position;
        waterfall.EnemyAttackOnClick = EnemyAttackOnClick;
        waterfall.Shoot(_dir, AttackSpeed); 
    }
    #endregion

    #region["Swirl Tower"]
    private void Swirl(Vector3 _dir)
    {
        GameObject swirl = Instantiate(Resources.Load("Prefabs\\Effect\\WaterRing")) as GameObject;
        Vector3 pos = transform.position;
        pos.y = 0.5f; 
        pos.z += 2f; 
        swirl.transform.position = pos;
        swirl.GetComponent<Water>().EnemyAttackOnClick = EnemyAttackOnClick; 
    }
    #endregion


    #region["Spawn Rain"]
    private void SpawnWater(Vector3 _dir, string _attacktype)
    {
        Water water = null;
        if(_attacktype.Equals("Rain"))
        {
            water = WaterManager.GetRain();
        }
        if(_attacktype.Equals("Tide"))
        {
            water = WaterManager.GetTide();
        }
        if(_attacktype.Equals("BigWave"))
        {
            water = WaterManager.GetWave();
        }
        water.transform.position = transform.GetComponentsInChildren<Transform>()[1].position;
        water.EnemyAttackOnClick = EnemyAttackOnClick;
        water.Shoot(_dir, AttackSpeed);
    }
    #endregion

    #region["Tower Support: Ebbtide"]
    private void Ebbtide()
    {
        GetComponentInChildren<SupportRange>().ExpandRange();
        GetComponentInChildren<SupportRange>().SupportTowerOnClick = AddTowerStat;
    }

    private void AddTowerStat(GameObject _tower)
    {
        if(gameObject.name.Contains("Ebbtide"))
        {
            if(_tower.GetComponent<TowerManager>() != null)
            {
                _tower.GetComponent<TowerManager>().AddTowerStat("Ebbtide", AttackDamage);
            }
            
        }
        if(gameObject.name.Contains("Raindrop"))
        {
            if(_tower.GetComponent<TowerManager>() != null)
            {
                _tower.GetComponent<TowerManager>().AddTowerStat("Raindrop", AttackDamage);
            }
        }
    }
    #endregion

    #region["Tower Support: Raindrop"]
    private void Raindrop()
    {
        GetComponentInChildren<SupportRange>().ExpandRange();
        GetComponentInChildren<SupportRange>().SupportTowerOnClick = AddTowerStat; 
    }
    #endregion  


    public void EnemyAttackOnClick(GameObject _attackedenemy)
    {
        attackedenemy = _attackedenemy;
        _attackedenemy.GetComponent<MonsterManager>().DamageByTower(AttackDamage); 
        if(gameObject.transform.parent.name.Contains("Caribbean_BermudaTower"))
        {
            //∞Ê¡˜ »ø∞˙: ¿·±Ò ¿˚¿Ã ∏ÿ√·¥Ÿ. 
            _attackedenemy.GetComponent<MonsterManager>().Interrupt(); 
        }
    }

}
