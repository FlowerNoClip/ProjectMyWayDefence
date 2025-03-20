using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

//2024-05-22: CUSTOM UNITY TEMPLATE 

//Aura: 대충 오브젝트의 바닥 색깔이라고 생각하면 됨. (사전적인 의미: 분위기, 대충 게임 용어) 
public class Aura : MonoBehaviour
{
    [SerializeField] private GameObject[] aurora = null;
    [SerializeField] private Vector3 vector3 = Vector3.zero;
    [SerializeField] private TowerManager towerManager = null;
    [SerializeField] private GameObject aura;
    private bool firstTime = true;

    //Basic Unique Epic Hero Myth Legend

    private int ranked_global = 1; 

    private void Awake()
    {
        
    }

    private void Start()
    {
        UpgradeTowerColor(ranked_global);

    }

    public void UpgradeTowerColor(int ranked)
    {

            Debug.Log("Time to Upgrade!");
            if(towerManager.name == "Hualand_FireTower")
            {
                vector3 = new Vector3(towerManager.transform.position.x, towerManager.transform.position.y - 0.5f, towerManager.transform.position.z);
            }
            else
            {
            vector3 = new Vector3(towerManager.transform.position.x, towerManager.transform.position.y - 0.3f, towerManager.transform.position.z);
        
            }
            
            if (ranked >= 1 && ranked <= 6)
            {
                if (firstTime)
                {
                aura = Instantiate(aurora[ranked - 1], vector3, Quaternion.identity, transform);
                ranked_global = ranked;
                firstTime = false;
            }
                else
                {
                Destroy(aura);
                aura = Instantiate(aurora[ranked - 1], vector3, Quaternion.identity, transform);
                    ranked_global = ranked;
                }
            }
            else
            {
                Debug.LogWarning("Ranked value is out of range. It must be between 1 and 6.");
            }
        

    }

    public int GetRanked()
    {
        return ranked_global; 
    }

}
