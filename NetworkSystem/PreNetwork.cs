using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PreNetwork : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.GameVersion = "0.01";
        PhotonNetwork.NickName = "DDuDDaDuDDa";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.JoinRandomOrCreateRoom(); 
    }

     # region NetworkHostingScripts

    public override void OnConnected()
    {
        Debug.LogError("Connecting....");
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        Debug.LogError($"Disconnect server by {_cause}");
        Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
    //    Debug.LogError($"Player Connected to Master..");
    }
    public override void OnJoinedRoom()
    {
        Debug.LogError($"Player Connected to Room..");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom("test", new RoomOptions{MaxPlayers = 2, BroadcastPropsChangeToAll = true});
    }
 

    #endregion
}
