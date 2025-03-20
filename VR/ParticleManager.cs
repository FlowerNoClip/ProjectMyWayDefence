using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class ParticleManager : MonoBehaviourPunCallbacks
{
    public ParticleSystem[] meteorParticleSystem;
    public ParticleSystem tornadoParticleSystem;
    public ParticleSystem[] waterParticleSystem; // 추가된 워터 파티클 시스템

    private bool isCasting = false;
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    public void MeteorStart()
    {
        if (waveManager != null && waveManager.CanUseMeteor())
        {
            photonView.RPC("StartParticleSystem", RpcTarget.All, "Meteor");
        }

        // 모든 Hualand 인스턴스에 메테오 이벤트 알림
        Hualand[] hualandTowers = FindObjectsOfType<Hualand>();
        foreach (var tower in hualandTowers)
        {
            tower.SetMeteorActive(true);
        }

        // 일정 시간 후 메테오 효과 리셋
        StartCoroutine(ResetMeteorEffect());
    }

    private IEnumerator ResetMeteorEffect()
    {
        yield return new WaitForSeconds(10f);

        // 메테오 효과 리셋
        Hualand[] hualandTowers = FindObjectsOfType<Hualand>();
        foreach (var tower in hualandTowers)
        {
            tower.SetMeteorActive(false);
        }
    }

    public void TornadoStart()
    {
        if (waveManager != null && waveManager.CanUseTornado())
        {
            photonView.RPC("StartParticleSystem", RpcTarget.All, "Tornado");
        }
    }

    public void WaterStart() // 워터 파티클 시스템을 시작하기 위한 메소드
    {
         if (waveManager != null && waveManager.CanUseWater())
         {
        photonView.RPC("StartParticleSystem", RpcTarget.All, "Water");
         }

    }

/*    public void TriggerStop()
    {
        photonView.RPC("StopAllParticleSystems", RpcTarget.All);
    }
*/
    [PunRPC]
    public void StartParticleSystem(string particleSystemType)
    {
        
        //Debug.Log("Start Particle System: " + particleSystemType);

        if (particleSystemType == "Meteor" && meteorParticleSystem != null)
        {
            for (int i = 0; i < meteorParticleSystem.Length; i++)
            {
                meteorParticleSystem[i].Play();
            }
        }
        else if (particleSystemType == "Tornado" && tornadoParticleSystem != null)
        {
            tornadoParticleSystem.Play();
        }
        else if (particleSystemType == "Water" && waterParticleSystem != null) // 추가된 파티클 시스템 유형
        {
            for (int i = 0; i < waterParticleSystem.Length; i++)
            {
                waterParticleSystem[i].Play();
            }
        }

    }

    

    /*[PunRPC]
    public void StopAllParticleSystems()
    {
        Debug.Log("Stop All Particle Systems");

        if (meteorParticleSystem != null)
        {
            for (int i = 0; i < meteorParticleSystem.Length; i++)
            {
                meteorParticleSystem[i].Stop();
            }
        }

        if (tornadoParticleSystem != null)
        {
            tornadoParticleSystem.Stop();
        }

        if (waterParticleSystem != null) // 워터 파티클 시스템 중지
        {
            for (int i = 0; i < waterParticleSystem.Length; i++)
            {
                waterParticleSystem[i].Stop();
            }
        }
    }*/
}
