using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTitleStartButton : MonoBehaviour
{
    public void GameTitleStartBtn()
    {
        SceneManager.LoadScene("3-1_Introduction&EnterNickName1");
    }
}
