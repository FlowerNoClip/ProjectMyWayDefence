using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHolder : MonoBehaviour
{
    [SerializeField] private List<GameObject>tower = new List<GameObject>();


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            GameObject towers = collision.gameObject.GetComponent<GameObject>();
            if (towers != null && !tower.Contains(towers))
            {
                tower.Add(towers);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            GameObject towers = collision.gameObject.GetComponent<GameObject>();
            if (towers != null && !tower.Contains(towers))
            {
                tower.Remove(towers);
            }
        }
    }
}
