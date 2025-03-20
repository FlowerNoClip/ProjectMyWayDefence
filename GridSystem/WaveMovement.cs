using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    [SerializeField] private Transform StartPos = null;
    [SerializeField] private float mvSpeed = 0f;

    private ParticleSystem particle = null;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
        transform.position = StartPos.position;
    }


    private IEnumerator Movement(Vector3[] _movePoints)
    {
        particle.Play();
        for(int i= 0; i < _movePoints.GetLength(0);)
        {   
            if(Vector3.Distance(transform.position, _movePoints[i]) <= 0.03f)  ++i;
            if(i >= _movePoints.GetLength(0)) break;

            transform.position = Vector3.MoveTowards(transform.position, _movePoints[i], Time.deltaTime * mvSpeed);
            yield return null;
            
        }
        particle.Stop();
        yield break;
    }

    public void SetMove(Vector3[] _movePoints)
    {
        Debug.Log(_movePoints.GetLength(0));
        transform.position = StartPos.position;
        StartCoroutine(Movement(_movePoints));
    }

}
