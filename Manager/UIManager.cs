using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : GameManager
{
    [SerializeField] private WaveManager monsterwave = null;
    [SerializeField] private placementManagerT placemanager = null;
    [SerializeField] private WaveManager wavemanager = null;
    [SerializeField] private Button[] towerbtn = null;
    [SerializeField] private Button[] tilebtn = null;
    [SerializeField] private Button[] upgrade = null;
    [SerializeField] private Sprite[] hualand_sprites = null;
    [SerializeField] private Sprite[] hikaru_sprites = null;
    [SerializeField] private Sprite[] caribbean_sprites = null;
    [SerializeField] private TMP_Text lifeText = null;
    [SerializeField] private UnitManager unitManager = null;
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private Button exitButton = null;
    [SerializeField] private GameObject blockHolder = null;
    [SerializeField] private GameObject tileHolder = null;
    [SerializeField] private GameObject canvas = null;
    [SerializeField] private TMP_Text wallpapaer_Toggletext = null;
    [SerializeField] private GameObject ui_upgrade = null;
    [SerializeField] private GameObject ui_detailbtn = null;
    [SerializeField] private GameObject menuui = null;
    [SerializeField] private GameObject settingui = null;
    [SerializeField] private AudioManager audiomanager = null;

    [SerializeField, TextArea] private string[] tower_detail_hualand = null;
    [SerializeField, TextArea] private string[] tower_detail_hikaru = null;
    [SerializeField, TextArea] private string[] tower_detail_caribbean = null;
    public TowerUpgradeRangeManager turangemanager = null;
    public bool Upg = false;
    public float speed = 0.5f; // 스무스한 움직임의 속도

    [SerializeField] private PunObj pun = null;


    public GameObject curGameObj = null;
    public Vector2Int curtower = Vector2Int.zero;




    private bool toggle = false;
    private bool TurnoffRay = false;
    private int[] random; // Random 배열

    [SerializeField] private Button[] cheattowerbtn = null; //Cheat
    public enum CheatTower
    {
        Unique,
        Epic,
        Hero,
        Myth,
        Legend
    }

    //화랜드 타워
    public enum HualandTower
    {
        Hualand_FireTower,
        Hualand_PenetrateTower,
        Hualand_GoblinTower,
        Hualand_PillaroffireTower,
        Hualand_PassionTower
        //Hualand_HeroTower
    }
    //히카루 타워
    public enum HikaruTower
    {
        Hikaru_WindTower,
        Hikaru_RainstormTower,
        Hikaru_SquallTower,
        Hikaru_WhirlWindTower,
        Hikaru_HurricaneTower,
        Hikaru_HeroTower
    }
    //캐리비안 타워
    public enum CaribbeanTower
    {
        Caribbean_EbbtideTower,
        Caribbean_RaindropTower,
        Caribbean_BermudaTower,
        Caribbean_SwirlTower,
        Caribbean_WaterfallTower,
        Caribbean_HeroTower
    }

    protected override void Awake()
    {
        base.Awake();

        SetRandomTowerButton();

        random = new int[tilebtn.Length];

        SetRandomTileButton();

        lifeText.text = gameManager.life.ToString();
        exitButton.onClick.AddListener(() => { Application.Quit(); });
        ui_upgrade.SetActive(false);
        ui_detailbtn.SetActive(false);
        menuui.SetActive(false);
        settingui.SetActive(false);
        audiomanager.PlayBGM();
        pun.SetOnPunCallBackMethod(test);
        if (cheattowerbtn != null)
        {
            SetRandomTowerCheatButton();
        }
    }

    protected override void Update()
    {
        base.Update();
        lifeText.text = gameManager.life.ToString();
        if (!TurnoffRay)
        {
            ClickandUpgrade();
        }
    }

    public void WallPaperToggle()
    {
        string text1 = "▶";
        string text2 = "◀";
        if (toggle)
        {
            canvas.transform.position = new Vector3(canvas.transform.position.x + 280, canvas.transform.position.y, canvas.transform.position.z);
            wallpapaer_Toggletext.text = text2.ToString();
            toggle = false;

        }
        else
        {
            canvas.transform.position = new Vector3(canvas.transform.position.x - 280, canvas.transform.position.y, canvas.transform.position.z);
            wallpapaer_Toggletext.text = text1.ToString();
            toggle = true;
        }
    }

    public void SetUIActive()
    {
        for (int i = 0; i < tilebtn.Length; i++)
        {
            tilebtn[i].interactable = false;
            tilebtn[i].interactable = false;
        }
    }

    public void SetInteractable()
    {
        if (tileHolder.activeSelf == false)
        {
            tileHolder.SetActive(true);
            blockHolder.SetActive(false);
        }
        else
        {
            tileHolder.SetActive(false);
            blockHolder.SetActive(true);
        }
    }

    public void SetRandomTowerButton()
    {
        int[] towernum_arr = new int[3];
        for (int i = 0; i < towernum_arr.Length; ++i)
        {
            towernum_arr[i] = UnityEngine.Random.Range(0, 5);
            //towernum_arr[i] = UnityEngine.Random.Range(0, 6);
        }
        switch (unitManager.heroname)
        {
            case "Hualand":
                SetTowerNameBtns(typeof(HualandTower), towernum_arr);
                break;
            case "Hikaru":
                SetTowerNameBtns(typeof(HikaruTower), towernum_arr);
                break;
            case "Caribbean":
                SetTowerNameBtns(typeof(CaribbeanTower), towernum_arr);
                break;
        }

    }

    public void OnPointerEnter_TowerBtn(PointerEventData eventdata, int _towerindex, Button _towerbtn)
    {
        if (!ui_detailbtn.activeSelf)
        {
            Vector2 uipos = _towerbtn.GetComponentInChildren<RectTransform>().position;
            uipos.x += 500f;
            ui_detailbtn.GetComponentInChildren<Image>().transform.position = uipos;
        }
        ui_detailbtn.SetActive(true);
        switch (unitManager.heroname)
        {
            case "Hualand":
                ui_detailbtn.GetComponentInChildren<TextMeshProUGUI>().text = tower_detail_hualand[_towerindex];
                break;
            case "Hikaru":
                ui_detailbtn.GetComponentInChildren<TextMeshProUGUI>().text = tower_detail_hikaru[_towerindex];
                break;
            case "Caribbean":
                ui_detailbtn.GetComponentInChildren<TextMeshProUGUI>().text = tower_detail_caribbean[_towerindex];
                break;
            default:
                break;
        }
    }

    public void OnPointerExit_TowerBtn(PointerEventData eventdata)
    {
        ui_detailbtn.SetActive(false);
    }

    public void SetRandomTileButton()
    {
        // 가능한 값들을 담을 리스트 생성
        List<int> availableValues = new List<int>();
        for (int i = 0; i < 5; ++i)
        {
            availableValues.Add(i);
        }

        // Random 배열에 중복 없이 값 할당
        for (int i = 0; i < random.Length; ++i)
        {
            if (tilebtn[i].interactable == false)
            {
                tilebtn[i].interactable = true;
            }
            // 랜덤으로 인덱스 선택
            int randomIndex = UnityEngine.Random.Range(0, availableValues.Count);
            // 선택된 값 할당
            random[i] = availableValues[randomIndex];

            // Tile 버튼의 텍스트 설정
            tilebtn[i].GetComponentInChildren<TextMeshProUGUI>().text = placemanager.objects[random[i]].name;

            // 이벤트 핸들러에서 클로저 문제 해결을 위해 지역 변수 사용
            int currentIndex = random[i];
            int tilebtxindex = i; // 현재 i 값을 캡처

            tilebtn[i].onClick.AddListener(() =>
            {

                if (wavemanager.stage == 0 && wavemanager.round == 1)
                {
                    SpawnTileBtn(currentIndex);
                    tilebtn[tilebtxindex].interactable = false;
                }

                else
                {
                    SpawnTileBtn(currentIndex);
                    tilebtn[0].interactable = false;
                    tilebtn[1].interactable = false;
                    tilebtn[2].interactable = false;

                }
            });
        }

    }
    public void SpawnTileBtn(int index)
    {
        //Debug.Log("_towername_btn: " + _towername);

        placemanager.ChangeTile(index);
    }


    public void SpawnTowerBtn(String _towername)
    {
        //Debug.Log("_towername_btn: " + _towername);
        placemanager.SpawnTowerBtn(_towername);
    }


    public void RerollBtn()
    {
        SetRandomTowerButton();
    }
    private void SetTowerNameBtns(Type _enumtype, int[] _towernum)
    {
        string[] towerbtnname_arr = new string[3];
        for (int i = 0; i < towerbtn.Length; ++i)
        {
            if (towerbtn[i].interactable == false)
            {
                towerbtn[i].interactable = true;
            }

            towerbtn[i].onClick.RemoveAllListeners();
        }
        for (int i = 0; i < towerbtn.Length; ++i)
        {

            int index = i;
            towerbtnname_arr[i] = Enum.GetName(_enumtype, _towernum[i]);
            towerbtn[i].GetComponentInChildren<TextMeshProUGUI>().text = towerbtnname_arr[i];
            towerbtn[i].onClick.AddListener(
                () => {
                    if (placemanager.testBox == null)
                    {
                        SpawnTowerBtn(towerbtnname_arr[index]);
                        towerbtn[index].interactable = false;
                    }
                    else
                    {
                        return;
                    }
                }
            );

            //출처: ChatGPT (시리나 빅스비 같은 인공지능 채팅인데 간단한(?) 프로그래밍도 할줄 암.)
            //https://chatgpt.com/
            EventTrigger trigger = towerbtn[i].gameObject.AddComponent<EventTrigger>();
            // PointerEnter 이벤트 추가 및 설정
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => { OnPointerEnter_TowerBtn((PointerEventData)eventData, _towernum[index], towerbtn[index]); });
            trigger.triggers.Add(pointerEnterEntry);
            // PointerExit 이벤트 추가 및 설정
            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => { OnPointerExit_TowerBtn((PointerEventData)eventData); });
            trigger.triggers.Add(pointerExitEntry);
            //SetTowerBtnSprites(towerbtn[i], _enumtype, i);
        }
    }

    private void SetTowerBtnSprites(Button _button, Type _enumtype, int _num)
    {
        String _enum_str = _enumtype.ToString();
        String towername = _enum_str.Split('+')[1];
        switch (towername)
        {
            case "Hualand":
                _button.GetComponent<Image>().sprite = hualand_sprites[_num];
                break;
            case "Hikaru":
                _button.GetComponent<Image>().sprite = hikaru_sprites[_num];
                break;
            case "Caribbean":
                _button.GetComponent<Image>().sprite = caribbean_sprites[_num];
                break;
        }
    }

    private void ClickandUpgrade()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼이 클릭되었는지 확인
        {
            // 마우스 위치를 기준으로 레이를 쏘아 맞은 대상을 가져옴
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 레이캐스트를 통해 충돌한 대상을 가져옴
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Upgrade")))
            {
                Debug.Log("Upgrade");
                Debug.Log("hit.collider.name: " + hit.collider.name + hit.collider.gameObject.GetComponentInParent<TowerManager>().ranked);
                // 충돌한 대상이 Tower 태그를 가진 오브젝트인지 확인
                if (hit.collider.CompareTag("Tower"))
                {
                    Debug.Log("Here?");
                    // 충돌한 오브젝트의 TowerManager 컴포넌트를 가져와 tempTower에 저장

                    curGameObj = hit.collider.gameObject;
                    curtower = curGameObj.GetComponentInParent<TowerManager>().towerNum;

                    ui_upgrade.SetActive(true);
                    upgrade[0].onClick.RemoveAllListeners();
                    upgrade[1].onClick.RemoveAllListeners();
                    upgrade[0].onClick.AddListener(
                        () => UpgradeTowerSet(curtower)
                    ) ;
                    upgrade[1].onClick.AddListener(
                        () => ExitBtn(hit.collider.gameObject)
                    );
                    TurnoffRay = true;
                }
            }
        }
    }


    public void UpgradeTowerSet(Vector2Int _curNum)
    {
        
        pun.CallBackPun(_curNum);
    }


     public void test(Vector2Int _curObj)
    {
        Debug.LogError(curGameObj ? "True" : "False");
        if (!curGameObj)
        {
            TowerManager[] games = GameObject.FindObjectsOfType<TowerManager>();
            foreach (TowerManager t in games)
            {
                if (t.towerNum == _curObj) curGameObj = t.gameObject;
            }
        }
        UpgradeTowerBtn(curGameObj);
    }
 


    public void UpgradeTowerBtn(GameObject _hittedtower)
    {
        StartCoroutine(Upgrade(_hittedtower));
    }

    IEnumerator Upgrade(GameObject _hittedtower)
    {
        Debug.Log("test");
        if (_hittedtower != null && _hittedtower.GetComponentInParent<TowerManager>() != null)
        {
            _hittedtower.GetComponentInParent<TowerManager>().upgradeRange.enabled = true;
            ui_upgrade.SetActive(false);
            TurnoffRay = false;
            yield return new WaitForSeconds(0.3f);
            if (_hittedtower.transform.parent != null)
            {
                _hittedtower.GetComponentInParent<TowerManager>().upgradeRange.enabled = false;
            }

        }

        yield break;
    }


    public void ExitBtn(GameObject _hittedtower)
    {
        ui_upgrade.SetActive(false);
        _hittedtower.GetComponentInParent<TowerManager>().upgradeRange.enabled = false;
        TurnoffRay = false;
    }

    public void MenuButton()
    {
        menuui.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoSettingBtn()
    {
        settingui.SetActive(true);
        settingui.GetComponentsInChildren<Button>()[2].onClick.AddListener(ExitSettingBtn);
        menuui.SetActive(false);
    }

    public void ExitSettingBtn()
    {
        settingui.SetActive(false);
        menuui.SetActive(true);
        Debug.Log("Custom Setting Button Here");
        audiomanager.StopBGM();
        audiomanager.PlayBGM();
    }
    public void playGame()
    {
        menuui.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        menuui.SetActive(false);
        //초기화
    }

    public void GoHome()
    {
        menuui.SetActive(false);
    }
    #region["Cheat: 강제로 라운드 올리기"]
    public void CheatRoundBtn()
    {
        monsterwave.SpawnMonsterCheat();
    }
    #endregion
    #region["Cheat: 타워 버튼"]
    public void SetRandomTowerCheatButton()
    {
        string[] towerbtnname_arr2 = { "Unique", "Epic", "Hero", "Myth", "Legend" };
        for (int i = 0; i < 5; ++i)
        {
            int index = i;
            cheattowerbtn[i].onClick.AddListener(
                () => SpawnTowerBtn(towerbtnname_arr2[index])
            );
        }
    }
    #endregion

    public void CheatVersion()
    {
        SceneManager.LoadScene("7-2_Scene_MainScene 1");
    }
}