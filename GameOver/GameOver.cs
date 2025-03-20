using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviourPunCallbacks
{
    public void Home()
    {
        PhotonNetwork.LeaveRoom(); 
        SceneManager.LoadScene("2_GameTitle");
    }

    public void Retry()
    {
        SceneManager.LoadScene("7-1_Play_Haland_Caribbean");
    }
}
