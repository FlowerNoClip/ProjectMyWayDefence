using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//작성자: 고영석
//수정자: 
//최초작성: 2024-04-22
//최종작성: 2024-06-03 

public class GameController_Setting : MonoBehaviour
{

    //Inspector 
    [SerializeField] private TMP_InputField volume_master_text = null;
    [SerializeField] private Slider volumemasterscroll = null;
    [SerializeField] private TMP_InputField volume_effect_text = null;
    [SerializeField] private Slider volumeeffectscroll = null;
    [SerializeField] private TMP_InputField volume_bgm_text = null;
    [SerializeField] private Slider volumebgmscroll = null;
    [SerializeField, Range(0f, 10f)] private float volume_master = 2f; 
    [SerializeField, Range(0f, 10f)] private float volume_effect = 2f;
    [SerializeField, Range(0f, 10f)] private float volume_bgm = 2f;

    private float originalvolume_master = 0f;
    private float originalvolume_effect = 0f;
    private float originalvolume_bgm = 0f;
    

    private AudioSource audiosource = null;


    #region["유니티 맨처음에 실행되는 메소드"] 
    private void Awake()
    {
        // audiosource = GetComponent<AudioSource>(); 
        audiosource = GetComponent<AudioSource>();

        originalvolume_master = volume_master; 
        originalvolume_effect = volume_effect;
        originalvolume_bgm = volume_bgm;


        if (PlayerPrefs.HasKey("Volume_Master"))
        {
            volume_master_text.text = (int)PlayerPrefs.GetFloat("Volume_Master") + "";
            volumemasterscroll.value = PlayerPrefs.GetFloat("Volume_Master");
            volume_master = PlayerPrefs.GetFloat("Volume_Master"); 
        }
        if (PlayerPrefs.HasKey("Volume_Effect"))
        {
            volume_effect_text.text = (int)PlayerPrefs.GetFloat("Volume_Effect") + "";
            volumeeffectscroll.value = PlayerPrefs.GetFloat("Volume_Effect");
            volume_effect = PlayerPrefs.GetFloat("Volume_Effect");
        }
        if(PlayerPrefs.HasKey("Volume_BGM"))
        {
            volume_bgm_text.text = (int)PlayerPrefs.GetFloat("Volume_BGM") + "";
            volumebgmscroll.value = PlayerPrefs.GetFloat("Volume_BGM");
            volume_bgm = PlayerPrefs.GetFloat("Volume_BGM");
        }
        else
        {
            volume_master_text.text = volume_master + "";
            volumemasterscroll.value = volume_master; 
            volume_effect_text.text = volume_effect + "";
            volumeeffectscroll.value = volume_effect;
            volume_bgm_text.text = volume_bgm + "";
            volumebgmscroll.value = volume_bgm;
        }
    }
    #endregion



    #region["효과음 샘플 재생"] 
    private IEnumerator PlayTestEffect()
    {
        AudioClip clip = Resources.Load("Audio\\Player\\맞았을 때\\Play_Scream") as AudioClip;
        audiosource.clip = clip;
        audiosource.volume = volume_bgm / 10f;
        audiosource.Play();
        yield return new WaitForSeconds(3f);
        audiosource.Stop();
        yield break;
    }
    #endregion

    #region["배경음악 샘플 재생"]
    private IEnumerator PlayTestBGM()
    {
        AudioClip clip = Resources.Load("Audio\\Startscreen_bgm") as AudioClip;
        audiosource.clip = clip;
        audiosource.volume = volume_bgm / 10f;
        audiosource.Play();
        yield return new WaitForSeconds(3f);
        audiosource.Stop();
        yield break; 
    }
    #endregion


    #region["뒤로가기"] 
    public void BackButton()
    {
        // Destroy(transform.parent.gameObject); 
       transform.parent.gameObject.SetActive(false);
    }
    #endregion

    #region["적용하기"]
    public void ApplyButton()
    {
        //마스터 볼륨
        PlayerPrefs.SetFloat("Volume_Master", volume_master);
        PlayerPrefs.Save();

        //효과음 
        PlayerPrefs.SetFloat("Volume_Effect", volume_effect);
        PlayerPrefs.Save();

        //배경음악 
        PlayerPrefs.SetFloat("Volume_BGM", volume_bgm);
        PlayerPrefs.Save();

        //transform.parent.gameObject.SetActive(false);
    }
    #endregion

    #region["설정값 초기화"]
    public void ClearButton()
    {
        //볼륨 설정값 초기화 
        PlayerPrefs.DeleteKey("Volume_Master");
        PlayerPrefs.DeleteKey("Volume_Effect");
        PlayerPrefs.DeleteKey("Volume_BGM");
        volume_master = originalvolume_master; 
        volume_effect = originalvolume_effect;
        volume_bgm = originalvolume_bgm;

        volume_master_text.text = (int)volume_master + "";
        volumemasterscroll.value = volume_master;

        volume_effect_text.text = (int)volume_effect + "";
        volumeeffectscroll.value = volume_effect;

        volume_bgm_text.text = (int)volume_bgm + "";
        volumebgmscroll.value = volume_bgm;
    }
    #endregion

    #region["마스터 볼륨: 효과음"]
    public void OnValueChange_Volume_Master()
    {
        volumeeffectscroll.value = volumemasterscroll.value;  
        volumebgmscroll.value = volumemasterscroll.value;
        volume_master_text.text = (int)volumemasterscroll.value + "";
        volume_master = (int)volumemasterscroll.value;
       
    }
    #endregion

   #region["마스터 볼륨: 텍스트"]
    public void OnValueChange_Volume_Master_Text()
    {
        volumemasterscroll.value = (float)Convert.ToDouble(volume_master_text.text); 
        volumeeffectscroll.value = (float)Convert.ToDouble(volume_master_text.text); 
        volumebgmscroll.value = (float)Convert.ToDouble(volume_master_text.text);
        volume_master = (int)volumemasterscroll.value; 
    }
    #endregion

    #region["볼륨조절: 효과음"] 
    public void OnValueChange_Volume_Effect()
    {
        volume_effect = volumeeffectscroll.value;  
        volume_effect_text.text = (int)volume_effect + "";
        StartCoroutine("PlayTestEffect"); 
    }
    #endregion

    #region["볼륨조절: 효과음: 텍스트"]
    public void OnValueChange_Volume_Effect_Text()
    {
        volumeeffectscroll.value = (float)Convert.ToDouble(volume_effect_text.text);
        volume_effect = volumeeffectscroll.value;
    }
    #endregion

    #region["볼륨조절: 배경음악"] 
    public void OnValueChange_Volume_BGM()
    {
        volume_bgm = volumebgmscroll.value;
        volume_bgm_text.text = (int)volume_bgm + "";
        StopCoroutine("PlayTestBGM"); 
        StartCoroutine("PlayTestBGM");
    }
    #endregion

    #region["볼륨조절: 배경음악: 텍스트"]
    public void OnValueChange_Volume_BGM_Text()
    {
        volumebgmscroll.value = (float)Convert.ToDouble(volume_bgm_text.text);
        volume_bgm = volumebgmscroll.value;
    }
    #endregion

}

