using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillarShootManager : MonoBehaviour
{
    public static FirePillarShootManager instance = null;
    [SerializeField] private GameObject prefab = null;
    [SerializeField, Range(1, 100)] protected int num = 5;

    private Queue<Fire> firequeue = new Queue<Fire>();

    protected virtual void Awake()
    {
        instance = this;

        initialize(num);
    }

    protected void initialize(int initcount)
    {
        for (int i = 0; i < initcount; ++i)
        {
            firequeue.Enqueue(CreateNewObject());
        }
    }

    private Fire CreateNewObject()
    {
        Fire fire = Instantiate(prefab).GetComponent<Fire>();
        fire.gameObject.SetActive(false);
        fire.transform.SetParent(transform);
        return fire;
    }

    public static Fire GetObject()
    {
        if (instance.firequeue.Count > 0)
        {
            Fire fire = instance.firequeue.Dequeue();
            fire.gameObject.SetActive(true);
            return fire;
        }
        else
        {
            Fire newfire = instance.CreateNewObject();
            newfire.gameObject.SetActive(true);
            return newfire;
        }
    }

    public static void ReturnObject(Fire fire)
    {
        fire.gameObject.SetActive(false);
        instance.firequeue.Enqueue(fire);
        fire.transform.position = Vector3.zero;
    }
}
