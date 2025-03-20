using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager instance = null;
    [SerializeField] private GameObject prefab = null;
    [SerializeField, Range(1, 100)] private int num = 5;

    private Queue<Wind> windqueue = new Queue<Wind>();

    protected virtual void Awake()
    {
        instance = this;

        initialize(num);
    }

    protected void initialize(int initcount)
    {
        for (int i = 0; i < initcount; ++i)
        {
            windqueue.Enqueue(CreateNewObject());
        }
    }

    private Wind CreateNewObject()
    {
        Wind wind = Instantiate(prefab).GetComponent<Wind>();
        wind.gameObject.SetActive(false);
        wind.transform.SetParent(transform);
        return wind;
    }

    public static Wind GetObject()
    {
        if (instance.windqueue.Count > 0)
        {
            Wind wind = instance.windqueue.Dequeue();
            wind.gameObject.SetActive(true);
            return wind;
        }
        else
        {
            Wind newwind = instance.CreateNewObject();
            newwind.gameObject.SetActive(true);
            return newwind;
        }
    }


    public static void ReturnObject(Wind wind)
    {
        wind.gameObject.SetActive(false);
        instance.windqueue.Enqueue(wind);
        wind.transform.position = Vector3.zero;
    }
}
