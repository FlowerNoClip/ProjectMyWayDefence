using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class SupportRange : MonoBehaviour
{
    private Vector3 originalscale = Vector3.zero;
    private Collider rangecollider = null;

    public delegate void SupportTowerDelegate(GameObject _tower); 
    private SupportTowerDelegate supporttoweronclick = null;
    public SupportTowerDelegate SupportTowerOnClick
    {
        set { supporttoweronclick = value; }
    }

    private void Awake()
    {
        originalscale = transform.localScale; 
        rangecollider =  GetComponent<Collider>();
        rangecollider.enabled = false; 
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if(_collider.transform.parent != null && _collider.transform.parent.name.Contains("Tower"))
        {
            //Tag 방식으로 변경예정
            StartCoroutine(ReturnToOriginalShape_Coroutine()); 
            supporttoweronclick?.Invoke(_collider.gameObject); 
        }
    }
    
    public void ExpandRange()
    {
        //Test
        transform.localScale = new Vector3(20f, 20f, 20f); 
    }

    private IEnumerator ReturnToOriginalShape_Coroutine()
    {
        float waittime = 0f;
        while(waittime <= 0.75f)
        {
            waittime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = originalscale; 
        yield break; 
    }

    public void EnableCollider()
    {
        rangecollider.enabled = true; 
    }

    public void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; 
    }
}
