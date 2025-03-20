using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hualand : TowerManager
{
    private Dictionary<GameObject, Coroutine> dotDamageCoroutines = new Dictionary<GameObject, Coroutine>();
    private bool isMeteorActive = false; // 메테오 효과 활성화 상태
    #region["Hualand 패시브: 3초 간 도트 데미지"]
    public void DotDamage(GameObject _attackedenemy)
    {
        if (dotDamageCoroutines.ContainsKey(_attackedenemy))
        {
            StopCoroutine(dotDamageCoroutines[_attackedenemy]);
        }
        Coroutine coroutine = StartCoroutine(DotDamageCoroutine(_attackedenemy));
        dotDamageCoroutines[_attackedenemy] = coroutine;
    }
    #endregion
    #region["도트 데미지 코루틴"]
    private IEnumerator DotDamageCoroutine(GameObject _attackedenemy)
    {
        float continuetime = 0f;
        while (continuetime < 3f)
        {
            float damage = isMeteorActive ? 2f : 1f; // 메테오 활성화 시 2의 데미지, 그렇지 않으면 1의 데미지
            _attackedenemy.GetComponent<MonsterManager>().DamageByTower(damage);
            yield return new WaitForSeconds(1f);
            continuetime += 1f;
        }
        dotDamageCoroutines.Remove(_attackedenemy);
    }
    #endregion
    #region["메테오 효과 설정"]
    public void SetMeteorActive(bool isActive)
    {
        isMeteorActive = isActive;
    }
    #endregion
    public void UpgradeAttackSpeed()
    {
        Debug.Log("공격속도2배");
        // 공격 속도를 2배로 업그레이드합니다.
        AttackSpeed *= 0.5f;
    }
    public void ResetAttackSpeed()
    {
        Debug.Log("공격속도 초기화");
        // 공격 속도를 원래대로 초기화합니다.
        AttackSpeed = 2f;
    }
    #region["Active Skill -> Switch/Case 문 사용"]
    protected virtual void Active(Vector3 _dir)
    {
        switch (name)
        {
            case "Hualand_PassionTower":
                // 화랜드 열정타워
                break;
            case "Hualand_HeroTower":
                // 화랜드 영웅타워
                break;
            case "Hualand_PillaroffireTower":
                // 화랜드 불기둥 타워
                break;
            case "Hualand_GoblinTower":
                // 화랜드 도깨비불 타워
                break;
            case "Hualand_PenetrateTower":
                // 화랜드 관통 불타워
                break;
            default:
                break;
        }
    }
    #endregion
    // 공격 (화랜드 타워)
    protected override void Update()
    {
        base.Update();
    }
}
