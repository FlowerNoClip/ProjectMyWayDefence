using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask ;
    public delegate void OnClickEventDelegate();
    public delegate void OnClickButtonDelegate();
    public delegate void OnInputWheelDelegate();
    public delegate void OnNumDelegate(float _num);
    private OnClickEventDelegate onLeftButtonEvent = null;    
    private OnClickEventDelegate onRightButtonEvent = null;
    private OnNumDelegate onWheelClickCallback = null;
  
  
    private OnClickButtonDelegate onButtonRotateCallback = null;
    private OnClickButtonDelegate onButtonPlaceCallback = null;
    private OnClickButtonDelegate onSpaceButtonCallback = null;
    private OnNumDelegate onAlphaNumCallback = null;

    public void SetonLeftButtonEvent(OnClickEventDelegate _onButtonEvent)
    {
        onLeftButtonEvent = _onButtonEvent;
    }
    public void SetonRightButtonEvent(OnClickEventDelegate _onButtonEvent)
    {
        onRightButtonEvent = _onButtonEvent;
    }
    public void SetonWheelButtonEvent(OnNumDelegate _onWheelButtonCallback)
    {
        onWheelClickCallback = _onWheelButtonCallback;
    }



    public void SetonButtonRotateEvent(OnClickButtonDelegate _onButtonClickCallback)
    {
        onButtonRotateCallback =_onButtonClickCallback;
    }
    public void SetonButtonPlaceCallback(OnClickButtonDelegate _onButtonPlaceCallback)
    {
        onButtonPlaceCallback = _onButtonPlaceCallback;
    }
    public void SetonCamRotateEvent(OnClickButtonDelegate _onButtonClickCallback)
    {
        onSpaceButtonCallback = _onButtonClickCallback;
    }
    public void SetAlphaNumCallback(OnNumDelegate _onAlphaNumCallback)
    {
        onAlphaNumCallback = _onAlphaNumCallback;
    }


    private void Update()
    {

#if UNITY_STANDALONE_WIN 
        // tmp.transform.position = placementGrid.FollowObj(GetMousePosition());
       
        int? alphaKey = GetPressedKey();
        if(Input.GetMouseButtonDown(0)) 
            onLeftButtonEvent?.Invoke();

        else if(Input.GetMouseButtonDown(1)) 
            onRightButtonEvent?.Invoke();

        if(Input.GetKeyDown(KeyCode.Space))
            onSpaceButtonCallback?.Invoke();

        else if(Input.GetKeyDown(KeyCode.B))
            onButtonPlaceCallback?.Invoke();

        else if(Input.GetKeyDown(KeyCode.R))
            onButtonRotateCallback?.Invoke();
       
        else if(Input.GetAxisRaw("Mouse ScrollWheel") > 0 ||Input.GetAxisRaw("Mouse ScrollWheel") <0 )
        {
            float axis = Input.GetAxisRaw("Mouse ScrollWheel");
             onWheelClickCallback?.Invoke(axis);
        }
           

        // else if (alphaKey.HasValue)
        //     onAlphaNumCallback?.Invoke(alphaKey.Value);

#endif

    }

    private int? GetPressedKey()
    {
        for (int i = 0; i <= 9; i++)
        {
            KeyCode keyCode = KeyCode.Alpha0 + i;
            if (Input.GetKeyDown(keyCode)) return i;
        }
        return null;
    }

    

#if UNITY_STANDALONE_WIN
    public Vector3? GetMousePositionInWorld()
    {
        Vector3 mousePos = Input.mousePosition;

        if(mousePos.x <=  0|| mousePos.y <= 0||mousePos.x >= Screen.width || mousePos.y >= Screen.height)
            return null;
        
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;




        if(Physics.Raycast(ray, out hit))
           return new Vector3(hit.point.x, 0f, hit.point.z);
        

        return null;
    }   
 
    public Vector2 GetMousePositionInScreen()
    {
        Vector3 mousePos = Input.mousePosition;

        if(mousePos.x >= Screen.width)      return Vector2.right;
        else if(mousePos.x <= 0)            return Vector2.left;
        else if(mousePos.y > Screen.height) return Vector2.up;
        else if(mousePos.y <= 0)            return Vector2.down; 


        return Vector2.zero;
    }
#endif
}
