using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class AttackRange : MonoBehaviour
{
    private Collider rangecollider = null;
    [SerializeField] private MonsterManager monsterManager = null;
    
    private void Awake()
    {
        rangecollider = GetComponent<Collider>();
        rangecollider.enabled = false; 
    }

    private void OnTriggerEnter(Collider _collider) 
    {
        if(_collider.CompareTag("Monster"))
        {
/*            Debug.Log("Mob Targetted");
            Debug.Log("_collider.gameObject: " + _collider.gameObject.name); */
            //GetComponentInParent<TowerManager>().TargetEnemy(_collider.gameObject);
        }
    }

    public void EnableCollider()
    {
        rangecollider.enabled = true;
    }

    public void DisableCollider()
    {
        rangecollider.enabled = false;
    }

    public void AddRigidbody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; 
    }
}
