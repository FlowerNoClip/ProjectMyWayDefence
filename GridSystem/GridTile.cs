using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridTile : MonoBehaviour
{
    [SerializeField] private Material emptyMat = null;
    [SerializeField] private Material filledMat = null;
    [SerializeField] private Material unableMat = null;
    [SerializeField] private Vector2Int number = Vector2Int.zero;
    [SerializeField] private Mesh[] meshes = null;
    private Renderer rend = null;
    private MeshFilter mesh = null;
    private BoxCollider tileColider = null;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        tileColider = GetComponentInChildren<BoxCollider>();
        mesh = GetComponentInChildren<MeshFilter>();
        
        rend.material = emptyMat;
        tileColider.enabled = false;
        mesh.mesh = meshes[0];
    }

    private void Start()
    {
        transform.localScale = Vector3.one * 0.1f;
        Vector3 planePos = transform.localPosition;
        planePos.y = -0.5f ;
        transform.localPosition = planePos;
        
    }
    public void SetStatus(placementStatus _status)
    {
        switch (_status)
        {
            case placementStatus.Empty:
                ChangeTileStatusInPlane();
                break;
            case placementStatus.Filled:
                ChangeTileStatusInCube();
                break;
            case placementStatus.Unable:
                rend.material = unableMat;
                break;
            default:
                rend.material = null;
                break;
        }
    }
    public void SetNumber(Vector2Int _in)
    {
        number = _in;
    }

    private void ChangeTileStatusInPlane()
    {
        rend.material = emptyMat;
        mesh.mesh = meshes[0];

        Vector3 planePos = transform.localPosition;
        tileColider.enabled = false;
        planePos.y = -0.5f ;
        transform.localPosition = planePos;
        transform.localScale = Vector3.one * 0.1f;
    }

    private void ChangeTileStatusInCube()
    {
        rend.material = filledMat;
        mesh.mesh = meshes[1];
        tileColider.enabled = true;

        Vector3 curPos = transform.localPosition;
        curPos.y = 0f ;
        transform.localPosition = curPos;
        transform.localScale = Vector3.one ;
    }

    
}