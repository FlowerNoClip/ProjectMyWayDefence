using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

public class TitleFuction : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject exitui = null;
    [SerializeField] private GameObject settingUI = null;
    [SerializeField] private Button[] buttons = null;

    [SerializeField] private string nextScene = "3-1_Introduction&EnterNickName1";
    [SerializeField] private string roomPageScene = "5-1_RoomPage";

    [SerializeField] private InputActionProperty RightCtrlprimaryBtn;
    [SerializeField] private InputActionProperty RightCtrlSecoundBtn;

    [SerializeField] private Image[] controllerBtn = null;



    private void Awake()
    {
        settingUI.SetActive(false);
        exitui.SetActive(false);

        

        buttons[0].onClick.AddListener( () => {     StarGameBtn();  });
        buttons[1].onClick.AddListener( () => {     SettingBtn();   });
        buttons[2].onClick.AddListener( () => {     QuitGame();     });

        RightCtrlprimaryBtn.action.performed += StarGamectrlBtn;
        RightCtrlSecoundBtn.action.performed += QuitGamectrlBtn;

        if(XRSettings.isDeviceActive) 
        {
            foreach(Image img in controllerBtn)
            {
                img.gameObject.SetActive(true);
            }
        }else
        {
            foreach(Image img in controllerBtn)
            {
                img.gameObject.SetActive(false);
            }
        }
    }


    private void StarGamectrlBtn(InputAction.CallbackContext context)
    {
        Debug.Log("Start Game");
        StarGameBtn();
    }

    private void QuitGamectrlBtn(InputAction.CallbackContext context)
    {
        Debug.Log("End Game");
        QuitGame();
    }
    private void StarGameBtn()
    {
        if(XRSettings.isDeviceActive) PhotonNetwork.ConnectUsingSettings();
        
        else SceneManager.LoadScene(nextScene);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("XR connecting to Server...");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnConnected();
        SceneManager.LoadScene(roomPageScene);
    }
    private void SettingBtn()
    {
        if (!settingUI.activeSelf)
        {
            settingUI.SetActive(true);
            settingUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(ExitSettingBtn); 
        }
        else
        {
            settingUI.SetActive(false);
        }
    }

    public void ExitSettingBtn()
    {
        settingUI.SetActive(false); 
    }

    private void QuitGame()
    {
        if(XRSettings.isDeviceActive) Application.Quit();
        if(exitui.activeSelf) exitui.SetActive(false);
        else exitui.SetActive(true);
    }

    
}
