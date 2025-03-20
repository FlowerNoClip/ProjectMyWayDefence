using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private bool camSet = false;
    [SerializeField] private float cameraMvSpeed = 1f;
    [SerializeField] private Transform camObj = null;
    [SerializeField] private float rotSpeed = 120f;

    [SerializeField] private float minZoomLevel = 5f;
    [SerializeField] private float maxZoomLevel = 20f;
  

    //  private void Awake()
    // {
    //     CameraCtrlInit();
    // }
    public void CameraCtrlInit(Camera _setCam)
    {
        //Debug.Log("Camera Init");
        camSet = true;
        if(!XRSettings.isDeviceActive) 
        {
            camObj = _setCam.transform;
            camObj.SetParent(this.transform);
            camObj.localPosition = new Vector3(0f,0f,-15f);
        }
        transform.eulerAngles = new Vector3(60f, 0f, 0f);
    }

    public void MoveCamera(Vector3 _targetPos)
    {
        Vector3 curPos = transform.position;
        curPos += Curmove(_targetPos) * cameraMvSpeed * Time.deltaTime;

        if(curPos.x > 16f)     curPos.x = 16f;
        else if(curPos.x < -5f)  curPos.x = -5f;

        transform.position = curPos;
    }    

    public void RotateCamPos()
    {   
        if(camSet)
        {
            StartCoroutine(RotateCamUP());
            camSet = false;
        }else
        {
            StartCoroutine(RotateCamDown());
            camSet = true;
        }
    }

    public void ZoomCamera(float _level)
    {
        ZoomLevel(_level);
    }

   
    public bool CameraToggle
    {
        set { camSet = value;}
        get { return camSet; } 
    }



    private void ZoomLevel(float _level)
    {


        float distance = Vector3.Distance(this.transform.position, camObj.position);

        if(distance >= minZoomLevel && distance <= maxZoomLevel)
            camObj.localPosition += Vector3.forward * Time.deltaTime * _level * rotSpeed;

        else if(distance < Mathf.Abs(minZoomLevel))
            camObj.localPosition = Vector3.forward * -minZoomLevel;
            
        else if(distance > Mathf.Abs(maxZoomLevel))
            camObj.localPosition = Vector3.forward * -maxZoomLevel;
            
      
    }


    private IEnumerator RotateCamUP()
    {
        float x = transform.eulerAngles.x;


        while(x < 90f)
        {
            x = Mathf.Clamp(x,60f,90f);

            x += rotSpeed * Time.deltaTime ;

            if(x >= 90f) x = 90f;

            transform.eulerAngles = new Vector3(x,0f,0f);
            yield return null;
        }

        yield break;
    }
    
    private Vector3 Curmove(Vector3 _targetPos)
    {
        if(_targetPos.x >= Screen.width - 1)  return Vector3.right;
        else if(_targetPos.x <= 1)            return Vector3.left;

        return Vector3.zero;
    }

    private IEnumerator RotateCamDown()
    {
        float x = transform.eulerAngles.x;

        while(x > 60f)
        {
            x = Mathf.Clamp(x,60f,90f);

            x -= rotSpeed * Time.deltaTime ;

            if(x <= 60f) x = 60f;

            transform.eulerAngles = new Vector3(x,0f,0f);
            yield return null;
        }

        yield break;
    }
   

}
