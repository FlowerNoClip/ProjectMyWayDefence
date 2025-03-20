using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    private PhotonView photonView = null;
    private Camera cam = null;
    bool triggerValue;
    public ParticleManager networkManager = null;
    [SerializeField] private float upgradeRange = 1f;
    private bool isInRange = false; // 네트워크 플레이어가 범위 안에 있는지 여부

    private Hualand[] allTowers;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        // 네트워크 매니저를 찾아서 할당
        networkManager = FindObjectOfType<ParticleManager>();

        // 씬의 모든 Hualand 타워를 찾습니다.
        allTowers = FindObjectsOfType<Hualand>();
    }

    public void SetCamera(Camera _targetCam)
    {
        cam = _targetCam;
        _targetCam.transform.SetParent(head);
        XROrigin xROrigin = GetComponent<XROrigin>();
        xROrigin.Camera = cam;
        _targetCam.transform.position = head.position;
        _targetCam.transform.rotation = head.rotation;
    }

    private void Update()
    {
        MapPosition(head, XRNode.Head);
    }

    private void MapPosition(Transform _target, XRNode _node)
    {
        if (!photonView.IsMine) return;
        InputDevices.GetDeviceAtXRNode(_node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _position);
        InputDevices.GetDeviceAtXRNode(_node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion _rotation);

        _target.localPosition = _position;
        _target.localRotation = _rotation;
    }

    public void MeteorOn()
    {
        Debug.Log("메테오");
        networkManager.MeteorStart();
    }

    public void TornadoOn()
    {
        Debug.Log("토네이도");
        gameObject.SetActive(true);
        networkManager.TornadoStart();
    }

    public void WaterOn()
    {
        Debug.Log("워터");
        networkManager.WaterStart();
    }

   /* private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        // 네트워크 플레이어가 범위 안에 들어왔을 때 호출됩니다.
        if (other.CompareTag("Tower"))
        {
            Debug.Log("콜라이더온");
            isInRange = true;
            UpgradeTowers();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
        // 네트워크 플레이어가 범위를 벗어났을 때 호출됩니다.
        if (other.CompareTag("Tower"))
        {
            isInRange = false;
            ResetTowers();
        }
    }

    private void UpgradeTowers()
    {
        if (isInRange)
        {
            Debug.Log("타워찾기");
            foreach (var tower in allTowers)
            {
                if (Vector3.Distance(tower.transform.position, transform.position) <= upgradeRange)
                {
                    Debug.Log("타워 업그레이드");
                    tower.UpgradeAttackSpeed();
                }
            }
        }
    }

    private void ResetTowers()
    {
        foreach (var tower in allTowers)
        {
            if (Vector3.Distance(tower.transform.position, transform.position) <= upgradeRange)
            {
                tower.ResetAttackSpeed();
            }
        }
    }

    [PunRPC]
    private void SetInRange(bool inRange)
    {
        isInRange = inRange;
        if (isInRange)
        {
            UpgradeTowers();
        }
        else
        {
            ResetTowers();
        }
    }*/
}
