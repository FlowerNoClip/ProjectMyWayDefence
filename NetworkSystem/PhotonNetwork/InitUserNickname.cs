using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class InitUserNickname : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private string gameVersion = "1.7";
    [SerializeField] private string nickName = string.Empty;
    [SerializeField] private string nextScene = string.Empty;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => 
        {
            ButtonAction();
        });

        inputField.onValueChanged.AddListener((string _nickName) => 
        {
            nickName = _nickName;
        });
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void ButtonAction()
    {
        if(string.IsNullOrEmpty(nickName))
        {
            Debug.LogError("NickName Is Empty...");
            return;
        }
        
        if(!PhotonNetwork.IsConnected)
        {
            
            Debug.LogFormat("Connect : {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.ConnectUsingSettings();
        }else
        {
            Debug.LogError($"Already Connected to Server.. join in Lobby");
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.LogError($"{PhotonNetwork.NickName} has Connected in Lobby..");
        if(nextScene!=string.Empty && PhotonNetwork.IsConnected) SceneManager.LoadScene(nextScene);
    }
    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Player Connected to Master..");
    }


    public override void OnConnected()
    {
        Debug.LogError("Connecting....");
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        Debug.LogError($"Disconnect server by {_cause}");
        Application.Quit();
    }




}
