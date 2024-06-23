using Photon.Pun;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveManager : GameManager
{
    [System.Serializable]
    public class MonsterData
    {
        public GameObject prefab;
        public int count;
    }

    [SerializeField] private MonsterData[] monsters;
    [SerializeField] private Transform[] go;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private GameObject[] monsterPool;
    [SerializeField] private GridSystem gridSystem = null;
    [SerializeField] private Transform endPosBox = null;
    [SerializeField] private Slider spawnTimeSlider = null;
    [SerializeField] private TMP_Text stageText = null;
    [SerializeField] private TMP_Text roundText = null;
    [SerializeField] private GameObject plane = null;
    [SerializeField] private TMP_Text monsterCountUI = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private placementManagerT placeManager = null;
    public int monsterCount = 0;
    public int spawnMobCounter = 40;

    [SerializeField] private float summonInterval = 0.2f;
    private Coroutine spawnCoroutine;
    private int currentMonsterIndex;
    private static int monsterIndexTemp;
    //private float randomZ;

    public bool isSpawn = false;
    public int stage = 0;
    public int round = 1;

    private bool isstartBuild = false;

    private float spawnTimer = 0f;
    private float spawnInterval = 5f;

    private Vector2Int gridSize = new Vector2Int(23, 20);

    private bool meteorUsed = false;
    private bool tornadoUsed = false;
    private bool waterUsed = false;

    protected override void Awake()
    {
        base.Awake();
        spawnTimeSlider.maxValue = spawnInterval;
        stageText.text = StageText().ToString();
        roundText.text = round.ToString();
    }


    public void StartBuild()
    {
        if (isstartBuild == false)
        {
            isstartBuild = true;
            StartCoroutine(StartBuildTime());
        }

    }

    private IEnumerator StartBuildTime()
    {
        while (spawnTimeSlider.value >= 0)
        {
            UpdateSpawnTimer();
            spawnTimeSlider.value = spawnTimer;
            monsterCountUI.text = monsterCount.ToString();

            yield return null;
        }
        isstartBuild = false;
        yield break;
    }

    private void UpdateSpawnTimer()
    {
        if (!isSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                Spawn();
                spawnTimer = 0f;
            }
        }

    }

    public void Spawn()
    {
        if (isSpawn)
        {
            return;
        }
        else
        {
            gridSystem.StartPath();
            SpawnMonster(stage, round);
            isSpawn = true;
            spawnTimer = 0f;
        }
    }

    private void InitializeMonsterPool(int monsterIndex, int count)
    {
        if (monsterIndexTemp != monsterIndex)
        {
            ClearMonsterPool();
        }

        monsterIndexTemp = monsterIndex;

        // 이미 폴링된 몬스터의 수를 가져옵니다.
        int pooledMonsterCount = (monsterPool != null) ? monsterPool.Length : 0;

        // 새로운 몬스터 풀 배열을 생성합니다.
        GameObject[] newMonsterPool = new GameObject[count + pooledMonsterCount];

        // 기존에 폴링된 몬스터를 새로운 배열로 복사하고 비활성화합니다.
        for (int i = 0; i < pooledMonsterCount; i++)
        {
            newMonsterPool[i] = monsterPool[i];
            newMonsterPool[i].SetActive(false);
        }

        // 새로운 몬스터만 폴링합니다.
        for (int j = pooledMonsterCount; j < newMonsterPool.Length; j++)
        {
            GameObject monster = Instantiate(monsters[monsterIndex].prefab, transform.position, Quaternion.identity, transform);
            monster.SetActive(false);
            monster.name += j;
            newMonsterPool[j] = monster;
        }

        // 새로운 몬스터 풀 배열로 교체합니다.
        monsterPool = newMonsterPool;
        currentMonsterIndex = 0;
    }

    public void AddStage()
    {
        if (round < 5)
        {
            if (stage <= 4)
            {
                Debug.Log("Stage " + stage + 1 + "-" + round);
                ++round;

                Debug.Log("Next round");
            }
        }
        else if (round >= 5)
        {
            if (stage >= 4)
            {
                Debug.Log("GameClear");
                Debug.Log("Stage " + stage + "-" + round);
                SceneManager.LoadScene("Victory");
            }
            else if (stage < 5)
            {
                round = 1;
                ++stage;
            }
        }

        meteorUsed = false;
        tornadoUsed = false;
        waterUsed = false;

        stageText.text = StageText().ToString();
        roundText.text = round.ToString();
        uiManager.SetRandomTileButton();
        uiManager.SetRandomTowerButton();
    }


    private IEnumerator UpdateGrid()
    {
        if (AreAllMonstersDeactivated())
        {
/*            // gridSystem.UpgradeGride();
            //placeManager.UpgradeGrid();

            // 현재 localScale을 가져옵니다.
            Vector3 planePosition = plane.transform.position;
            Vector3 newScale = plane.transform.localScale;

            // x 값을 20만큼 증가시킵니다.
            planePosition.x += 5f;
            newScale.x += 10f;

            // 수정된 localScale을 다시 할당합니다.
            plane.transform.position = planePosition;
            plane.transform.localScale = newScale;



            // endPosBox.transform.position = new Vector3(gridSize.x -0.5f, 0f, 10f);
            endPos.position = new Vector3(gridSize.x - 0.5f, 0f, 9.5f);
            Debug.Log("Stage " + stage + "-" + round);*/
            yield break;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateGrid());
    }

    private int StageText()
    {
        return stage + 1;
    }

    private void ClearMonsterPool()
    {
        if (monsterPool != null)
        {
            for (int i = 0; i < monsterPool.Length; i++)
            {
                Destroy(monsterPool[i]);
            }
            monsterPool = null;
        }
    }

    private void DeactivateAllMonsters()
    {
        if (monsterPool != null)
        {
            for (int i = 0; i < monsterPool.Length; i++)
            {
                monsterPool[i].gameObject.transform.position = gameObject.transform.position;
                monsterPool[i].SetActive(false);
            }
        }
    }

    private void SpawnMonster(int _stage, int _round)
    {
        if (_stage == 0)
        {
            StartSpawnCoroutine(_stage, spawnMobCounter);
        }
        else if (_stage == 1)
        {
            StartSpawnCoroutine(_stage, spawnMobCounter);
        }
        else if (_stage == 2)
        {
            StartSpawnCoroutine(_stage, spawnMobCounter);
        }
        else if (_stage == 3)
        {
            StartSpawnCoroutine(_stage, spawnMobCounter);
        }
        else if (_stage == 4)
        {
            StartSpawnCoroutine(_stage, spawnMobCounter);
        }
        else
        {
            Debug.Log("GameClear");
        }

        uiManager.SetUIActive();
    }

    public void SpawnMonsterCheat()
    {
        if (stage >= 4)
        {
            Debug.Log("GameClear");
            Debug.Log("Stage " + stage + "-" + round);
            SceneManager.LoadScene("Victory");
        }
        else if (stage < 5)
        {
            round = 1;
            ++stage;
            StartCoroutine(UpdateGrid());
            StartSpawnCoroutine(stage, spawnMobCounter);
        }

        stageText.text = StageText().ToString();
        roundText.text = round.ToString();
        uiManager.SetRandomTileButton();
        uiManager.SetRandomTowerButton();
    }

    private bool AreAllMonstersDeactivated()
    {
        if (monsterPool != null)
        {
            for (int i = 0; i < monsterPool.Length; i++)
            {
                if (monsterPool[i].activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void StartSpawnCoroutine(int monsterIndex, int count)
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnMonsters(monsterIndex, count));
    }


    private IEnumerator SpawnMonsters(int monsterIndex, int count)
    {
        InitializeMonsterPool(monsterIndex, count);

        yield return StartCoroutine(ActivateAllMonstersSequentially(monsterPool.Length));
    }

    private IEnumerator ActivateAllMonstersSequentially(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //randomZ = Random.Range(0.5f, 19.5f);

            if (!monsterPool[i].activeSelf)
            {
                /*                startPos.transform.position = new Vector3(0.5f, -1f, randomZ);
                                endPos.transform.position = new Vector3(gridSize.x -0.5f, -1f, randomZ);*/
                monsterPool[i].SetActive(true);
                yield return new WaitForSeconds(summonInterval);
            }
        }
        StartCoroutine(AddStages());

    }

    private IEnumerator AddStages()
    {
        if (AreAllMonstersDeactivated())
        {
            AddStage();
            isSpawn = false;
            yield break;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(AddStages());
    }

    public bool CanUseMeteor()
    {
        if (!meteorUsed)
        {
            meteorUsed = true;
            return true;
        }
        return false;
    }
    public bool CanUseTornado()
    {
        if (!tornadoUsed)
        {
            tornadoUsed = true;
            return true;
        }
        return false;
    }
    public bool CanUseWater()
    {
        if (!waterUsed)
        {
            waterUsed = true;
            return true;
        }
        return false;
    }
}
