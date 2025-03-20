using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class GameController_RoomPage : MonoBehaviourPunCallbacks
{
    private string heroname = string.Empty;
    private ExitGames.Client.Photon.Hashtable pht = null; 
    //. . . . . 
    [SerializeField] private string prevScene = "3-2_Input_NickName";
    [SerializeField] private string nextScene = "7-1_Scene_MainScene";
    [SerializeField] private GameObject joinbtn = null;
    [SerializeField] private GameObject welcomeui = null;
    // [SerializeField] private GameObject inputroomwindow = null;
    [SerializeField] private GameObject loadingSprite = null;
    [SerializeField] private Canvas mainCanvas = null;

    private bool matching = false;

    public override void OnJoinedRoom()
    {        
        Debug.LogError("hello world");
        JoinRoomBtn();        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError($"Joined Rooom has Failed By {message}");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList); 

        foreach(RoomInfo room in roomList)
        {
            Debug.LogError("RoomName: " + room.Name); 
        }
    }

   

    public void CloseWelComePopUp()
    {
        welcomeui.SetActive(false);
    }


    public void CreateRoomBtn()
    {

        CreateRoom(); 
    }

    public void CreateRoom()
    {        

        joinbtn = Instantiate(joinbtn) as GameObject;
        joinbtn.transform.SetParent(mainCanvas.gameObject.transform);
        // joinbtn.GetComponentInChildren<Button>().onClick.AddListener(JoinRoomBtn);


        string roomName = PhotonNetwork.LocalPlayer.NickName + "¥‘¿« πÊ";


        RoomOptions roomOptions = new RoomOptions();
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        PhotonNetwork.CreateRoom(roomName, roomOptions);

        joinbtn.GetComponent<RoomInfoBar>().SetRoom(roomName);

        Debug.Log("Successfully Room Created!");
    }


    public void JoinRoomBtn()
    {
        if (PhotonNetwork.InRoom)
        {          
            Debug.Log("Current Room Name: " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Joinning Room..."); 
            LoadingUp();
        }

        
    }

    private void Awake()
    {
        if(!PhotonNetwork.IsConnected) 
            SceneManager.LoadScene(prevScene);

        PhotonNetwork.AutomaticallySyncScene = true;

        if(XRSettings.isDeviceActive) 
        {
            Debug.Log($"InLobby : {PhotonNetwork.InLobby} ");
           
            welcomeui.SetActive(false);
            // StartCoroutine(WaitingOtherPlayer());
            PhotonNetwork.JoinRandomRoom();   

        }else
        {
            welcomeui.SetActive(true);
            loadingSprite.SetActive(false);
        }


        
        
    }


    private void LoadingUp()
    {        
        if(!matching)
        {
            loadingSprite.SetActive(true);
            StartCoroutine(WaitingOtherPlayer());
        }
    }

    private IEnumerator WaitingOtherPlayer()
    {

        matching = true;
        Debug.Log("Checking Player...");
        while(matching)
        {
       
            yield return new WaitForEndOfFrame();

            if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                matching = false;
                PhotonNetwork.LoadLevel(nextScene);
            }
        }
        yield break;

    }

}
