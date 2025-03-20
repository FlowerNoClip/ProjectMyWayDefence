using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class TowerUpgradeRangeManager : MonoBehaviour
{
    private List<TowerManager> towerManagers = new List<TowerManager>(); // TowerManager 오브젝트를 저장할 리스트
    public GridSystem gridSystem;
    [SerializeField] private string towername = string.Empty;
    Collider colliders;
    private void Awake()
    {
        colliders = GetComponent<Collider>();
        GameObject Grid = GameObject.FindGameObjectWithTag("GridSystem");
        if (Grid != null)
        {
            gridSystem = Grid.GetComponent<GridSystem>();
        }       
    }
    // 콜라이더에 무언가 들어올 때 호출됨
    private void OnTriggerEnter(Collider other)
    {
        if(transform.parent.gameObject.GetComponent<TowerManager>().GetRanked() == 6)
        {
            Debug.Log("이미 최고등급 입니다.");
            return; 
        }
        if(other.transform.parent != null && other.transform.parent.name.Contains(transform.parent.name) && other.transform.parent.CompareTag("Tower"))
        {
            Debug.Log("Maybe..."); 
            if(other.transform.parent.gameObject.GetComponent<TowerManager>().GetRanked() == transform.parent.gameObject.GetComponent<TowerManager>().GetRanked())
            {
                Debug.Log("Here!"); 
                TowerManager towerManager = other.transform.parent.gameObject.GetComponent<TowerManager>(); // 들어온 객체에서 TowerManager 컴포넌트를 가져옴
                if (towerManager != null && !towerManagers.Contains(towerManager)) // 가져온 TowerManager가 유효하고 리스트에 없으면 추가
                {
                    // 리스트에서 null이 있는 경우, 해당 인덱스에 새로운 요소를 추가
                    int index = towerManagers.FindIndex(item => item == null);
                    if (index != -1)
                    {
                        towerManagers[index] = towerManager;
                        StartCoroutine(UpgradeTower(towerManager));

                    }
                    else
                    {
                        towerManagers.Add(towerManager);
                        
                        StartCoroutine(UpgradeTower(towerManager));
                    }
                }
            }
        }
    }

    // 콜라이더에서 무언가 나갈 때 호출됨
    private void OnTriggerExit(Collider other)
    {
        TowerManager towerManager = other.GetComponent<TowerManager>(); // 나간 객체에서 TowerManager 컴포넌트를 가져옴
        if (towerManager != null && towerManagers.Contains(towerManager)) // 가져온 TowerManager가 유효하고 리스트에 있으면 삭제
        {
            towerManagers.Remove(towerManager); // 리스트에서 제거
        }
    }


    private IEnumerator UpgradeTower(TowerManager _tower)
    {
        if (_tower != null)
        {
            /*
            _tower.TowerUpgrade(towername);
            Destroy(transform.parent.transform.parent.gameObject); 
            */

            //자기 자신은 업그레이드 

            transform.parent.gameObject.GetComponent<TowerManager>().TowerUpgrade(towername);
            colliders.enabled = false;
            //가져온 타워는 삭제 
            /* Node aa = gridSystem.WorldToGridNode(_tower.transform.parent.gameObject.transform.position);
             * 
             aa.SetTileStatus(placementStatus.Filled);*/
            gridSystem.SetTile(_tower.transform.parent.gameObject.transform.position, placementStatus.Filled);


            yield return new WaitForSeconds(0.2f);
            Destroy(_tower.transform.parent.gameObject);
            yield return null;
        }
        yield break;
    }
}

