using System;
using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
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

    public void Shoot(Vector3 _dir, float _AttackSpeed, float _AttackDamage)
    {
        direction = _dir;
        AttackSpeed = _AttackSpeed;
        isLaunched = true;
        
        if (gameObject.name.Equals("P_FirePillar(Clone)"))
        {
            StartCoroutine(ShootFirePillarCoroutine());
        }
        else
        {
            StartCoroutine(ShootCoroutine()); 
        }
        Invoke("DestroyEffect", 3f);
    }

    private void DestroyEffect()
    {
        isLaunched = false;
        StopCoroutine("ShootCoroutine"); 
        ShootManager.ReturnObject(this);
    }

    //에러: 일단 해결했음(MobManager -> MonsterManager) 
    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Monster"))
        {
            Debug.Log("Mob Damage");
            if (_collider.GetComponent<MonsterManager>() != null)
            {
                enemyattackonclick?.Invoke(_collider.gameObject);
            }
            else
            {
                Debug.Log("Monster is Null!");
            }
            DestroyEffect(); 
        }
    }


    private IEnumerator ShootCoroutine()
    {
        direction.y = 0f; 
        while(isLaunched)
        {
            transform.Translate(direction * Time.deltaTime * AttackSpeed * -5f); 
            //transform.position += direction * Time.deltaTime * AttackSpeed * -5f;
            yield return new WaitForEndOfFrame(); 
        }
    }

    private IEnumerator ShootFirePillarCoroutine()
    {
        Debug.Log(".Fire Pillar Coroutine"); 
        float time = 0f;
        while (time < 3f)
        {
           
            transform.position += direction * Time.deltaTime * -1f;
            transform.Translate(Vector3.up * Time.deltaTime * 2.5f); 
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
