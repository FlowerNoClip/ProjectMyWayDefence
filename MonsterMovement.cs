using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private Transform startPos = null;
    [SerializeField] private GameObject endPos = null;
    [SerializeField] private GridSystem gridManager = null;
    [SerializeField] private WaveManager waveManager = null;
    [SerializeField] private GameManager gameManager = null;
    public float mvSpeed = 5f;

    public Vector3 previousPosition;
    public Vector3 currentPosition;
    public Vector3 moveDirection;
    private void Awake()
    {
        // "WaveManager" 태그를 가진 게임 오브젝트를 찾습니다.
        GameObject StartPoint = GameObject.FindGameObjectWithTag("Startpoint");
        GameObject EndPoint = GameObject.FindGameObjectWithTag("Destination");
        GameObject Grid = GameObject.FindGameObjectWithTag("GridSystem");
        GameObject Wave = GameObject.FindGameObjectWithTag("WaveManager");
        GameObject Game = GameObject.FindGameObjectWithTag("GameManager");

        if (StartPoint != null)
        {
            startPos = StartPoint.GetComponent<Transform>();
        }
        if (EndPoint != null)
        {
            endPos = EndPoint.GetComponent<GameObject>();
        }
        if (Grid != null)
        {
            gridManager = Grid.GetComponent<GridSystem>();
        }
        if (Wave != null)
        {
            waveManager = Wave.GetComponent<WaveManager>();
        }
        if (Game != null)
        {
            gameManager = Game.GetComponent<GameManager>();
        }
    }


    private void OnEnable()
    {
        transform.position = startPos.position;
        previousPosition = transform.position;
        Vector3[] array = gridManager.GetWorldPosWithArray();
        waveManager.monsterCount++;
        SetMove(array);
    }

    private void OnDisable()
    {
        waveManager.monsterCount--;
    }



    private IEnumerator Movement(Vector3[] _movePoints)
    {

        for (int i = 0; i < _movePoints.GetLength(0);)
        {
            if (Vector3.Distance(transform.position, _movePoints[i]) <= 0.03f) ++i;
            if (i >= _movePoints.GetLength(0)) break;

            transform.LookAt(_movePoints[i]);
            transform.position = Vector3.MoveTowards(transform.position, _movePoints[i], Time.deltaTime * mvSpeed);
            yield return null;

        }

        yield break;
    }

    public void SetMove(Vector3[] _movePoints)
    {
        StartCoroutine(Movement(_movePoints));
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestinationBox"))
        {
            gameObject.SetActive(false);
            gameManager.life--;

        }

        if (other.gameObject.CompareTag("GoblinTower"))
        {
            mvSpeed = 2.1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GoblinTower"))
        {
            mvSpeed = 3.0f;
        }

    }
}





