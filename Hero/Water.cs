using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
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
        Invoke("DestroyEffect", 2.5f);
    }

    private void DestroyEffect()
    {
        isLaunched = false;
        if(gameObject.name.Contains("Bermuda"))
        {
            WaterManager.ReturnBermuda(this);
        }
        if(gameObject.name.Contains("WaterFall"))
        {
            WaterManager.ReturnWaterFall(this); 
        }
        if(gameObject.name.Contains("P_Water"))
        {
            WaterManager.ReturnRain(this); 
        }
        if(gameObject.name.Contains("BigWave"))
        {
            WaterManager.ReturnWave(this); 
        }
        if(gameObject.name.Contains("Tide"))
        {
            WaterManager.ReturnTide(this); 
        }
    }

    //에러: 일단 해결했음(MobManager -> MonsterManager) 
    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Monster"))
        {
            if (_collider.GetComponent<MonsterManager>() != null)
            {
                enemyattackonclick?.Invoke(_collider.gameObject);
                if(gameObject.name.Contains("Bermuda") || gameObject.name.Contains("BigWave"))
                {
                    Invoke("DestroyEffect", 2.5f);
                }
                else
                {
                    DestroyEffect();
                }
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (isLaunched)
        {
            if(gameObject.name.Contains("BigWave") || gameObject.name.Contains("WaterFall"))
            {
                transform.Translate(direction * Time.deltaTime * AttackSpeed * -1f);
            }
            else
            {
                transform.Translate(direction * Time.deltaTime * AttackSpeed * -5f);
            }
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    
}
