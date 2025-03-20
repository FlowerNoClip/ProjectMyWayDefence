using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public static WaterManager instance = null;
    [SerializeField] private GameObject rain = null; //ºø¹æ¿ï 
    [SerializeField] private GameObject waterfall = null; //ÆøÆ÷
    [SerializeField] private GameObject bermuda = null; //¹ö¹Â´Ù
    [SerializeField] private GameObject tide = null; //¹Ð¹°
    [SerializeField] private GameObject wave = null; //ÆÄµµ 

    [SerializeField, Range(1, 100)] private int num = 20;

    private Queue<Water> waterqueue = new Queue<Water>();
    private Queue<Water> waterfallqueue = new Queue<Water>();
    private Queue<Water> bermudaqueue = new Queue<Water>();
    private Queue<Water> tidequeue = new Queue<Water>();
    private Queue<Water> wavequeue = new Queue<Water>();

    protected virtual void Awake()
    {
        instance = this;

        initialize(num);
    }

    protected void initialize(int initcount)
    {
        for (int i = 0; i < initcount; ++i)
        {
            waterqueue.Enqueue(CreateRain());
            bermudaqueue.Enqueue(CreateBermuda());
            waterfallqueue.Enqueue(CreateWaterFall());
            tidequeue.Enqueue(CreateTide());
            wavequeue.Enqueue(CreateWave()); 
        }
    }

    private Water CreateRain()
    {
        Water water = Instantiate(rain).GetComponent<Water>();
        water.gameObject.SetActive(false);
        water.transform.SetParent(transform);
        return water;
    }

    private Water CreateWaterFall()
    {
        Water water = Instantiate(waterfall).GetComponent<Water>();
        water.gameObject.SetActive(false);
        water.transform.SetParent(transform);
        return water;
    }

    private Water CreateBermuda()
    {
        Water water = Instantiate(bermuda).GetComponent<Water>();
        water.gameObject.SetActive(false);
        water.transform.SetParent(transform);
        return water;
    }
    
    private Water CreateTide()
    {
        Water water = Instantiate(tide).GetComponent<Water>();
        water.gameObject.SetActive(false);
        water.transform.SetParent(transform);
        return water;
    }

    private Water CreateWave()
    {
        Water water = Instantiate(wave).GetComponent<Water>();
        water.gameObject.SetActive(false);
        water.transform.SetParent(transform);
        return water;
    }

    public static Water GetRain()
    {
        if (instance.waterqueue.Count > 0)
        {
            Water water = instance.waterqueue.Dequeue();
            water.gameObject.SetActive(true);
            return water;
        }
        else
        {
            Water newwater = instance.CreateRain();
            newwater.gameObject.SetActive(true);
            return newwater;
        }
    }

    public static Water GetBermuda()
    {
        if (instance.bermudaqueue.Count > 0)
        {
            Water water = instance.bermudaqueue.Dequeue();
            water.gameObject.SetActive(true);
            return water;
        }
        else
        {
            Water newwater = instance.CreateBermuda();
            newwater.gameObject.SetActive(true);
            return newwater;
        }
    }

    public static Water GetWaterFall()
    {
        if (instance.waterfallqueue.Count > 0)
        {
            Water water = instance.waterfallqueue.Dequeue();
            water.gameObject.SetActive(true);
            return water;
        }
        else
        {
            Water newwater = instance.CreateWaterFall();
            newwater.gameObject.SetActive(true);
            return newwater;
        }
    }

    public static Water GetTide()
    {
        if (instance.tidequeue.Count > 0)
        {
            Water water = instance.tidequeue.Dequeue();
            water.gameObject.SetActive(true);
            return water;
        }
        else
        {
            Water newwater = instance.CreateTide();
            newwater.gameObject.SetActive(true);
            return newwater;
        }
    }

    public static Water GetWave()
    {
        if (instance.wavequeue.Count > 0)
        {
            Water water = instance.wavequeue.Dequeue();
            water.gameObject.SetActive(true);
            return water;
        }
        else
        {
            Water newwater = instance.CreateWave();
            newwater.gameObject.SetActive(true);
            return newwater;
        }
    }

    public static void ReturnRain(Water water)
    {
        water.gameObject.SetActive(false);
        instance.waterqueue.Enqueue(water);
        water.transform.position = Vector3.zero;
    }

    public static void ReturnBermuda(Water water)
    {
        water.gameObject.SetActive(false);
        instance.bermudaqueue.Enqueue(water);
        water.transform.position = Vector3.zero;
    }

    public static void ReturnWaterFall(Water water)
    {
        water.gameObject.SetActive(false);
        instance.waterfallqueue.Enqueue(water);
        water.transform.position = Vector3.zero;
    }

    public static void ReturnTide(Water water)
    {
        water.gameObject.SetActive(false);
        instance.tidequeue.Enqueue(water);
        water.transform.position = Vector3.zero;
    }

    public static void ReturnWave(Water water)
    {
        water.gameObject.SetActive(false);
        instance.wavequeue.Enqueue(water);
        water.transform.position = Vector3.zero;
    }
}
