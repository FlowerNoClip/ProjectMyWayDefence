using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips; // 오디오 클립 배열

    private AudioSource bgmSource = null; // BGM용 오디오 소스
    private List<AudioSource> sfxSources = new List<AudioSource>(); // 효과음용 오디오 소스 목록

    //Audio
    //1: BGM, 2: Block ON, 3: Block OFF
    //4: Tower ON, 5: Tower OFF
    //6: 

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = audioClips[0]; 
        // 효과음용 AudioSource 컴포넌트를 초기화합니다.
        for (int i = 0; i < 10; i++) // 초기에는 10개의 AudioSource를 준비합니다.
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(sfxSource);
        }
    }


    // 사용 가능한 AudioSource를 찾는 함수
    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // 모든 AudioSource가 사용 중일 경우 새로운 AudioSource를 추가합니다.
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        sfxSources.Add(newSource);
        return newSource;
    }

    // 효과음을 재생하는 함수
    public void PlaySFX(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= audioClips.Length)
        {
            Debug.LogWarning("잘못된 클립 인덱스입니다.");
            return;
        }

        AudioSource sfxSource = GetAvailableSFXSource();
        if(PlayerPrefs.HasKey("Volume_Effect")) //효과음 볼륨 설정 
        {
            sfxSource.volume = PlayerPrefs.GetFloat("Volume_Effect") / 10f; 
        }
        sfxSource.PlayOneShot(audioClips[clipIndex]);
    }

    public void StopSFX(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= audioClips.Length)
        {
            Debug.LogWarning("잘못된 클립 인덱스입니다.");
            return;
        }
        AudioSource sfxSource = GetAvailableSFXSource();
        sfxSource.Stop(); 
    }

    public void PlayBGM()
    {
        bgmSource = GetComponentsInChildren<AudioSource>()[0]; 
        if (PlayerPrefs.HasKey("Volume_BGM")) //BGM 볼륨 설정 
        {
            bgmSource.volume = PlayerPrefs.GetFloat("Volume_BGM") / 10f; 
        }
        bgmSource.loop = true; // 반복 재생 설정
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource = GetComponentsInChildren<AudioSource>()[0];
        bgmSource.Stop(); 
    }
}
