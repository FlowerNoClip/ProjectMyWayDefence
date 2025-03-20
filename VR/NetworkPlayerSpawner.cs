using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject spawnedPlayerPrefab = null;
    [SerializeField] private Camera mainCam = null;
    [SerializeField] private Transform spawnPos = null;
    [SerializeField] private CameraCtrl cameraCtrl = null;
    [SerializeField] private placementManagerT placemanager = null;
    [SerializeField] private WaveManager waveManager = null;
    private Hashtable roomProperty = null;

    void Awake()
    {
        ToConnectServer();
        roomProperty = new Hashtable();
        roomProperty["NickName"] = "DDuDDaDuDDa";
        roomProperty["Champions"] = "DDuDDaDuDDa";

    }

    // Update is called once per frame
    private void ToConnectServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying To Connecting Server....");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server...");

        PhotonNetwork.JoinLobby();

    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.LogError("Joined Lobby..");


        InstantiatePlayer();

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError("Creating Room...");



        RoomOptions roo = new RoomOptions();
        roo.MaxPlayers = 5;
        roo.BroadcastPropsChangeToAll = true;
        roo.CustomRoomProperties = roomProperty;
        roo.IsOpen = true;

        PhotonNetwork.CreateRoom("test1", roo, TypedLobby.Default);
    }

    public override void OnPlayerEnteredRoom(Player _newPlayer)
    {
        Debug.Log("A player Joined the room...");
        base.OnPlayerEnteredRoom(_newPlayer);
    }

    public override void OnCreatedRoom()
    {

        Debug.LogError("Creating Room...");



        RoomOptions roo = new RoomOptions();
        roo.MaxPlayers = 5;
        roo.BroadcastPropsChangeToAll = true;
        roo.CustomRoomProperties = roomProperty;
        roo.IsOpen = true;

        PhotonNetwork.CreateRoom("test1", roo, TypedLobby.Default);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed Create Room...by {message}");

    }
    private void InstantiatePlayer()
    {

        if (XRSettings.isDeviceActive)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("VR/XRPlayer", spawnPos.position, Quaternion.identity);
            XROrigin xROrigin = spawnedPlayerPrefab.GetComponent<XROrigin>();
            xROrigin.Camera = mainCam;

            Debug.Log($"Xr is {XRSettings.isDeviceActive}");
            PhotonView phtView = spawnedPlayerPrefab.GetComponent<PhotonView>();


            if(phtView.IsMine)
                spawnedPlayerPrefab.GetComponent<UserCamera>().SetCamera(mainCam);
        }
        else
        {
            // spawnedPlayerPrefab = PhotonNetwork.Instantiate("VR/PcPlayer", spawnPos.position, Quaternion.identity);
           // placemanager.InitPcUserCtrl();\

            cameraCtrl.CameraCtrlInit(mainCam);
            Debug.Log($"Xr is {XRSettings.isDeviceActive}");
        }        
   

        // roomProperty = PhotonNetwork.CurrentRoom.CustomProperties;
        // Debug.Log(roomProperty["Champion"]);

    }

    public override void OnLeftRoom()
    {
        
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.LogError($"{otherPlayer.NickName} has left room....");
    }

    public void StartGame()
    {
        photonView.RPC("StartGamewithTimer", RpcTarget.All);
    }

    [PunRPC]
    public void StartGamewithTimer()
    {
        waveManager.StartBuild();
    }
}