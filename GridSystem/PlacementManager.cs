using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

public class placementManagerT : MonoBehaviourPun
{
    [SerializeField] private InputTest inputManager = null;
    [SerializeField] private GridSystem gridManager = null;
    [SerializeField] public TileObject[] objects = null;
    [SerializeField] private CameraCtrl cameraCtrl = null;
    [SerializeField] private PathFinding pathfinding = null;
    [SerializeField] private float placeHeight = 0.5f;
    [SerializeField] private AudioManager audioManager = null;
    [SerializeField] private WaveMovement wave = null;

    public GameObject testBox = null;
    private Vector3? curPos = Vector3.zero;
    private Vector3 placePos = Vector3.zero;
    private TileObject tempObj = null;
    public TileObject curObj = null;


    // Init InputManager CallBackmethod
    private void Awake()
    {
        inputManager.SetonLeftButtonEvent(OnInputLeftClickCallback);
        inputManager.SetonWheelButtonEvent(OnWheelDrawCallback);
        inputManager.SetonButtonRotateEvent(OnRotateBtnCallBack);
        inputManager.SetonButtonPlaceCallback(OnButtonPlaceCallback);
        inputManager.SetonCamRotateEvent(OnCameraRotate);
    }


    /// Update method

    private void Update()
    {

        if (!XRSettings.isDeviceActive)
        {
            curPos = inputManager.GetMousePositionInWorld();
            if (curPos.HasValue) placePos = curPos.Value;
            cameraCtrl.MoveCamera(Input.mousePosition);

            if (curObj && curPos.HasValue)
            {
                curObj.transform.position = gridManager.FollowObjbyGrid(curPos.Value);
                gridManager.UpdateGridOnMove(curObj.transform.position, curObj.obj);
            }
            if (testBox)
            {
                Vector3 vector3 = gridManager.FollowObjbyGrid(placePos);
                vector3.y = 0.5f;
                testBox.transform.position = vector3;
            }
        }
    }


    #region CallBackMethod

    private void OnInputLeftClickCallback()
    {
        if (curObj)
            photonView.RPC("PlaceTile", RpcTarget.All, placePos);

    }

    private void OnWheelDrawCallback(float _num)
    {
        cameraCtrl.ZoomCamera(_num);
    }
    private void OnButtonPlaceCallback()
    {
        if (testBox) photonView.RPC("PlaceTower", RpcTarget.All, placePos);

    }
    private void OnCameraRotate()
    {
        if (XRSettings.isDeviceActive) return;
        cameraCtrl.RotateCamPos();
    }



    private void OnRotateBtnCallBack()
    {
        if (curObj) photonView.RPC("RotateTileStatus", RpcTarget.All);
    }

    public void SpawnTowerBtn(string _towername)
    {
        if (testBox)
        {
            Destroy(testBox.gameObject);
            photonView.RPC("InstateTower", RpcTarget.All, _towername);
        }
        else
            photonView.RPC("InstateTower", RpcTarget.All, _towername);

        // PhotonNetwork.Instantiate("Prefabs/Towers/" + _towername, placePos, Quaternion.identity);
    }

    public void ChangeTile(int _tileNum)
    {
        photonView.RPC("ChangeTileWithPun", RpcTarget.All, _tileNum);
    }


    public void UpgradeGrid()
    {
        photonView.RPC("UpgradeMap", RpcTarget.All);
    }
    #endregion




    [PunRPC]
    private void UpgradeMap()
    {
        gridManager.UpgradeGride();
    }

    [PunRPC]
    private void ChangeTileWithPun(int _num)
    {
        //Debug.Log(_num);
        curObj = objects[_num];
    }

    [PunRPC]
    private void RotateTileStatus()
    {

        placementStatus[,] tmpArr = new placementStatus[curObj.obj.GetLength(1), curObj.obj.GetLength(0)];


        for (int i = 0; i < tmpArr.GetLength(1); i++)
        {
            for (int j = 0; j < tmpArr.GetLength(0); j++)
            {
                tmpArr[j, tmpArr.GetLength(1) - 1 - i] = curObj.obj[i, j];
            }
        }
        curObj.obj = tmpArr;
        gridManager.UpdateGridOnMove(curPos.Value, curObj.obj);
    }


    [PunRPC]
    public void PlaceTile(Vector3 _objPos)
    {
        gridManager.placeTile(_objPos, curObj.obj, out bool placeSuccess);
        if (placeSuccess) curObj = null;
        audioManager.PlaySFX(2);
        Vector3[] path = gridManager.GetWorldPosWithArray();
        wave.SetMove(path);
    }

    [PunRPC]
    private void InstateTower(string _towername)
    {
        GameObject tower_prefab = Resources.Load("Prefabs/Towers/" + _towername) as GameObject;
        testBox = Instantiate(tower_prefab, placePos, Quaternion.identity);
        gridManager.ClearTileOnMove();

    }
    [PunRPC]
    private void PlaceTower(Vector3 _targetPos)
    {

        if (!testBox || gridManager.CannotbePlace(_targetPos)) return;

        Vector3 placePos = Vector3.zero;
        if (testBox.GetComponentInChildren<TowerManager>() != null)
        {
            placePos = gridManager.FollowObjbyGrid(_targetPos);
            placePos += Vector3.up * placeHeight;


            if (gridManager.CanbePlace(_targetPos))
            {
                gridManager.SetTile(placePos, placementStatus.Unable);
                testBox.transform.position = placePos;
                testBox.GetComponentInChildren<TowerManager>().towerNum = gridManager.WorldToGrid(placePos, Vector2Int.one);

                testBox = null;
            }

        }
    }



}