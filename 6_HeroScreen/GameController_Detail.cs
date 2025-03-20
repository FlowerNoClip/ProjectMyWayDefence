using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class GameController_Detail : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI nicknametext = null;
    [SerializeField] private TextMeshProUGUI nicknametext2 = null;

    [SerializeField] private String prevScene = string.Empty;
    [SerializeField] private String gameplay_mainScene = string.Empty;
    [SerializeField] private String roomscene = string.Empty;

    [SerializeField] private GameObject passiveui = null;
    [SerializeField] private GameObject ultui = null;

    [SerializeField] private Button passivebtn = null;
    [SerializeField] private Button ultbtn = null;

    private Hashtable pht = null;

    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            nicknametext.text = PhotonNetwork.NickName;
            nicknametext2.text = PhotonNetwork.NickName + "¥‘¿« Room";
            pht = PhotonNetwork.CurrentRoom.CustomProperties;
        }
        else
        {
            SceneManager.LoadScene(prevScene);
        }
    }
    #endregion

    public void PassivePointEnter()
    {
        Vector3 currentsize = passivebtn.gameObject.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("passive size: " + currentsize);
        currentsize.x += 50f;
        currentsize.y += 50f;
        passivebtn.gameObject.GetComponent<RectTransform>().sizeDelta = currentsize;
    }

    public void PassivePointExit()
    {
        Vector3 currentsize = passivebtn.gameObject.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("passive size: " + currentsize);
        currentsize.x -= 50f;
        currentsize.y -= 50f;
        passivebtn.gameObject.GetComponent<RectTransform>().sizeDelta = currentsize;
    }

    public void PassiveDetail()
    {
        if (passiveui.activeSelf)
        {
            passiveui = Instantiate(passiveui);
        }
        else
        {
            passiveui.SetActive(true);
            passiveui = Instantiate(passiveui);
        }
    }

    public void UltPointEnter()
    {
        Vector3 currentsize = ultbtn.gameObject.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("ult size: " + currentsize);
        currentsize.x += 50f;
        currentsize.y += 50f;
        ultbtn.gameObject.GetComponent<RectTransform>().sizeDelta = currentsize;
    }

    public void UltPointExit()
    {
        Vector3 currentsize = ultbtn.gameObject.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("ult size: " + currentsize);
        currentsize.x -= 50f;
        currentsize.y -= 50f;
        ultbtn.gameObject.GetComponent<RectTransform>().sizeDelta = currentsize;
    }

    public void UltDetail()
    {
        if (ultui.activeSelf)
        {
            ultui = Instantiate(ultui);
        }
        else
        {
            ultui.SetActive(true);
            ultui = Instantiate(ultui);
        }
    }


    public void StartGame()
    {
        SceneManager.LoadScene(gameplay_mainScene);
    }

    public void GetOutRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leave Room");
        SceneManager.LoadScene(roomscene);
    }

}
