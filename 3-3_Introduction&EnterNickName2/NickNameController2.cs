using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class NickNameController2 : MonoBehaviourPunCallbacks
{
    private string heroname = string.Empty;
    [SerializeField] private TextMeshProUGUI nicknametext = null;

    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        if(PhotonNetwork.IsConnected)
        {
            Debug.Log("NickName: " + PhotonNetwork.NickName);
            nicknametext.text = PhotonNetwork.NickName + "님이군요. 멋진 이름입니다!";
        }
        else
        {
            //이전 씬으로 이동해야함. 
        }
    }
    #endregion

    public void ChooseHeroBtn()
    {
        SceneManager.LoadScene("4-1_ChooseHero"); 
    }

}
