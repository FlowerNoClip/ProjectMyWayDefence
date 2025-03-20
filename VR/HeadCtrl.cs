using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadCtrl : MonoBehaviour
{

    // [SerializeField] private InputActionProperty headPos ;
    // [SerializeField] private InputActionProperty headRot ; 
    [SerializeField] private GameObject camObj = null;

    private void Update()
    {
        UpdateHead();
    }

    private void UpdateHead()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.Head).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _position);
        InputDevices.GetDeviceAtXRNode(XRNode.Head).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion _rotation);


        this.transform.position = _position;
        this.transform.rotation = _rotation;
    }
    
}
