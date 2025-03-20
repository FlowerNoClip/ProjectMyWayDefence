using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorRayCast : MonoBehaviour
{   
    private RectTransform rect = null;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    
    private void Update()
    {
        Ray ray = new Ray();
        ray.origin = rect.position;
        ray.direction = Vector3.forward;

        RaycastHit hit ;

        if(Physics.Raycast(ray, out hit , 1000f))
        {
            Debug.Log(hit.transform.name);
        }

        Debug.DrawRay(rect.position, Vector3.forward* 1000f, Color.red);
    }
}
