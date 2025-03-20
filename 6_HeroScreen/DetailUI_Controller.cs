using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DetailUI_Controller : MonoBehaviour
{
    public void ExitUI() 
    {
        //Destroy(gameObject.transform.parent.gameObject); 
        transform.parent.gameObject.SetActive(false); 
    }

}
