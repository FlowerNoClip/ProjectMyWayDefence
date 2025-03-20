using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class InitMainGame : MonoBehaviourPunCallbacks
{
   [SerializeField] private string prevScene = string.Empty;
   [SerializeField] private GameObject spawnedPlayerPrefab = null;
   [SerializeField] private Transform[] spawnPos = null;
   [SerializeField] private CameraCtrl cameraCtrl = null;

    // public override void OnJoinedRoom()
    // {
    //     // Debug.LogError($"Player {PhotonNetwork.LocalPlayer.NickName} has joined Room..");
    //     InitPlayerSpawn();
    // }
    // public override void OnCreatedRoom()
    // {
    //     Debug.LogError($"Creating Room");
    //     base.OnCreatedRoom();
    // }
    // public override void OnJoinRandomFailed(short returnCode, string message)
    // {
    //     Debug.LogError($"Join Room has Failed by {message} : {returnCode}");
    //     PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 2, BroadcastPropsChangeToAll = true});

    // }
    private void Awake()
    {
        // if(!PhotonNetwork.IsConnected)
        // SceneManager.LoadScene(prevScene);
        // else 
        // PhotonNetwork.JoinRandomRoom();
        InitPlayerSpawn();
    }

    private void InitPlayerSpawn()
    {
         if(XRSettings.isDeviceActive) {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("VR/XRPlayer", spawnPos[0].position, Quaternion.identity);
            XROrigin xROrigin = spawnedPlayerPrefab.GetComponent<XROrigin>();
            xROrigin.Camera = Camera.main;
            Debug.Log($"Xr is {XRSettings.isDeviceActive}");

        }
        else 
        {
            Debug.Log($"Xr is {XRSettings.isDeviceActive}");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("VR/PcPlayer", spawnPos[1].position, Quaternion.identity);
            // cameraCtrl.Init();
        }

        PhotonView phtView = spawnedPlayerPrefab.GetComponent<PhotonView>();

        if(phtView.IsMine&& XRSettings.isDeviceActive) 
        {
            spawnedPlayerPrefab.GetComponent<UserCamera>().SetCamera(Camera.main);
        }
    }


}


