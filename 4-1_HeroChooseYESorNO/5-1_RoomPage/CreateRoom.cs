using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    [SerializeField] private Button createRoomBtn = null;


    private void Awake()
    {

        createRoomBtn.onClick.AddListener(() => 
        {
            CreateRoomBtn();
        });

        this.gameObject.SetActive(false);
    }

    private void CreateRoomBtn()
    {
        
        
    }


    
}
