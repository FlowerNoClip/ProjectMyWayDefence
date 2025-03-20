#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using Photon.Realtime;

public class ChooseHero : MonoBehaviourPunCallbacks
{
    private enum Champions
    { Hualand, Hikaru, Caribbean }

    [SerializeField] private Button[] buttons = null;
    [SerializeField] private GameObject popUP = null;
    [SerializeField] private string nextScene = string.Empty;
    [SerializeField] private string prevScene = string.Empty;
    private Hashtable userProperty = null;
    private string chooseChampion = string.Empty;


    private void Awake()
    {

        // Debug.Log("NickName in Hero Scene: " + PhotonNetwork.NickName); 
        userProperty = new Hashtable();
        buttons[0].onClick.AddListener(() =>
        {
            Champion(Champions.Hualand);
            buttons[1].interactable = false;
            buttons[2].interactable = false;
        });
        buttons[1].onClick.AddListener(() =>
        {
            Champion(Champions.Hikaru);
            buttons[0].interactable = false;
            buttons[2].interactable = false;
        });
        buttons[2].onClick.AddListener(() =>
        {
            Champion(Champions.Caribbean);
            buttons[0].interactable = false;
            buttons[1].interactable = false;
        });

        buttons[3].onClick.AddListener(ConfirmAction);
        buttons[4].onClick.AddListener(CancelAction);       

    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected && prevScene != string.Empty) SceneManager.LoadScene(prevScene);
    }

    private void Champion(Champions _num)
    {
        chooseChampion = _num.ToString();

        if(popUP.activeSelf)       
            popUP.SetActive(false);
        
        else
            popUP.SetActive(true);      
       
    }

    public void ConfirmAction()
    {
        userProperty["Champion"] = chooseChampion;
        PhotonNetwork.LocalPlayer.SetCustomProperties(userProperty); 
        if (nextScene != string.Empty) SceneManager.LoadScene(nextScene);
    }

    public void CancelAction()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
        popUP.SetActive(false); 
    }

}