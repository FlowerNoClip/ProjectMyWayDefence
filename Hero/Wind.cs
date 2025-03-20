using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public delegate void OnAttackEnemyDelegate(GameObject _attackedenemy);
    private OnAttackEnemyDelegate enemyattackonclick = null;
    public OnAttackEnemyDelegate EnemyAttackOnClick
    {
        set { enemyattackonclick = value; }
    }

    private float AttackSpeed = 0;
    private Vector3 direction = Vector3.zero;
    private Boolean isLaunched = false;

    public bool IsLaunched
    {
        get { return isLaunched; }
    }

    public void Shoot(Vector3 _dir, float _AttackSpeed)
    {
        direction = _dir;
        AttackSpeed = _AttackSpeed;
        isLaunched = true;
        StartCoroutine(ShootCoroutine()); 
        if(!gameObject.name.Contains("Hurricane"))
        {
            Invoke("DestroyEffect", 5f);
        }
    }

    private void DestroyEffect()
    {
        isLaunched = false;
        WindManager.ReturnObject(this);
    }

    //에러: 일단 해결했음(MobManager -> MonsterManager) 
    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Monster"))
        {
            Debug.Log("Mob Damage");
            if (_collider.GetComponent<MonsterManager>() != null)
            {
                if(!gameObject.name.Contains("WindRing"))
                {
                    enemyattackonclick?.Invoke(_collider.gameObject);
                    if(!gameObject.name.Contains("Hurricane") && !gameObject.name.Contains("HeroWindField"))
                    {
                        DestroyEffect();
                    }
                    if(gameObject.name.Contains("HeroWindField"))
                    {
                        Expand(); 
                    }
                }
                else
                {
                    StartCoroutine(DotRingDamage(_collider.gameObject)); 
                }
            }
            else
            {
                Debug.Log("Monster is Null!");
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while(isLaunched)
        {
            transform.Translate(direction * Time.deltaTime * AttackSpeed * -5f); 
            yield return new WaitForEndOfFrame(); 
        }
        yield break; 
    }


    private IEnumerator DotRingDamage(GameObject _object)
    {
        while(_object.activeSelf)
        {
            enemyattackonclick?.Invoke(_object);
            Debug.Log("Dot Ring Damage"); 
            yield return new WaitForSeconds(1f); 
        }
        yield break; 
    }

    private void Expand()
    {
        transform.localScale += new Vector3(0.2f, 0f, 0.2f); 
    }
}
