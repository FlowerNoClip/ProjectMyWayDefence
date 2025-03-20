using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour
{
    [SerializeField] private Transform camPos = null;



    public void SetCamera(Camera _targetCam)
    {
        _targetCam.transform.SetParent(camPos);
        _targetCam.transform.position = camPos.position;
        _targetCam.transform.rotation = camPos.rotation;
    }
}
