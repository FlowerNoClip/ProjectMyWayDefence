using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PcPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!XRSettings.isDeviceActive)
        {
            float xValue = Input.GetAxisRaw("Horizontal");
            float yValue = Input.GetAxisRaw("Vertical");


            transform.position += new Vector3(xValue,0f,yValue) * 5f *Time.deltaTime;

            if(Input.GetKey(KeyCode.Q)) transform.rotation *= Quaternion.Euler(Vector3.up * 40f *Time.deltaTime);
            if(Input.GetKey(KeyCode.E)) transform.rotation *= Quaternion.Euler(Vector3.up * -40f *Time.deltaTime);
        }
    }
}
