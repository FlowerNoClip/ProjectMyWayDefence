using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Vector2Int dimensions = Vector2Int.one;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private GameObject tile = null;
    [SerializeField] private PathFinding pathFinding = null;

    private Node[,] m_Tile = null;
    private Vector2Int m_GridSize = Vector2Int.zero;
    private Vector2Int prevPos = Vector2Int.zero;
    private placementStatus[,] prevNode = null;
    private List<Vector2Int> preNodeset = new List<Vector2Int>();
    private List<Node> path = null;
    private float m_InvGridSize = 1f;
    private int i = 0;


    public List<Node> Path
    {
        get { return path; }
        set { path = value; }
    }
    public void TestLog()
    {
        Debug.LogError("Hello" + i++);

    }
    public Vector3 FollowObjbyGrid(Vector3 _target)
    {

        Vector2Int gridPos = WorldToGrid(_target, m_GridSize);
        return GridToWorld(gridPos, m_GridSize);
    }

    public void placeTile(Vector3 _targetPos, placementStatus[,] _obj, out bool _placeSuccess)
    {
        Vector2Int gridPos = WorldToGrid(_targetPos, m_GridSize);

        for (int x = 0; x < _obj.GetLength(0); ++x)
        {

            for (int y = 0; y < _obj.GetLength(1); ++y)
            {

                if (_obj[x, y] == placementStatus.Empty) continue;

                if (prevNode[x + gridPos.x, y + gridPos.y] != placementStatus.Empty || (x + gridPos.x) <= 0 || (x + gridPos.x) >= (dimensions.x - 2))
                {
                    Debug.Log("Cant Build Tile Here!!");
                    _placeSuccess = false;
                    return;
                }
            }

        }

        if (!pathFinding.StartPathFind())
        {
            _placeSuccess = false;
            return;
        }
        path = pathFinding.GetPathNode;

        //Debug.Log("Place Compelete!!!");
        SetTile(gridPos, _obj);
        SetPrevNode();
        _placeSuccess = true;
        return;
    }

    #region CannotBePlace

    public bool CannotbePlace(Vector3 _targetPos)
    {
        Vector2Int gridPos = WorldToGrid(_targetPos, m_GridSize);
        if (m_Tile[gridPos.x, gridPos.y].GetStatusInNode == placementStatus.Empty)
            return true;
        return false;
    }

    #endregion
    public bool CanbePlace(Vector3 _targetPos)
    {
        Vector2Int gridPos = WorldToGrid(_targetPos, m_GridSize);

        if (m_Tile[gridPos.x, gridPos.y].GetStatusInNode == placementStatus.Filled)
            return true;


        return false;
    }
    public bool CanbePlace(Vector3 _targetPos, placementStatus[,] _obj)
    {
        Vector2Int gridPos = WorldToGrid(_targetPos, m_GridSize);

        placementStatus[,] statuses = GetTile(gridPos, _obj);

        for (int x = 0; x < statuses.GetLength(0); ++x)
        {
            for (int y = 0; y < statuses.GetLength(1); ++y)
            {
                if (m_Tile[x + gridPos.x, y + gridPos.y].GetStatusInNode != placementStatus.Empty)
                    return false;
            }
        }

        return true;
    }

    public void ClearTileOnMove()
    {
        ClearTile();
    }

    public void UpdateGridOnMove(Vector3 _targetPos, placementStatus[,] _obj)
    {

        if (_targetPos == null || _obj == null) return;

        Vector2Int curPos = WorldToGrid(_targetPos, m_GridSize);
        if (curPos != prevPos)
        {
            ClearTile();
            for (int x = 0; x < _obj.GetLength(0); ++x)
            {
                for (int y = 0; y < _obj.GetLength(1); ++y)
                {
                    if (_obj[x, y] == placementStatus.Empty || x + curPos.x >= dimensions.x || y + curPos.y >= dimensions.y) continue;
                    if (_obj[x, y] == placementStatus.Filled && m_Tile[x + curPos.x, y + curPos.y].GetStatusInNode != placementStatus.Empty)
                    {
                        m_Tile[x + curPos.x, y + curPos.y].SetTileStatus(placementStatus.Unable);
                        preNodeset.Add(new Vector2Int(x + curPos.x, y + curPos.y));
                        continue;
                    }

                    m_Tile[x + curPos.x, y + curPos.y].SetTileStatus(_obj[x, y]);
                    preNodeset.Add(new Vector2Int(x + curPos.x, y + curPos.y));
                }
            }

            prevPos = curPos;

        }
        else return;

    }


    private void Awake()
    {
        m_InvGridSize = 1 / gridSize;
        int x = Mathf.RoundToInt(gridSize);
        m_GridSize = new Vector2Int(x, x);
        InitGrid();
    }

    public void StartPath()
    {
        pathFinding.StartPathFind();
        path = pathFinding.GetPathNode;
    }


    private void InitGrid()
    {

        if (m_Tile == null)
        {
            m_Tile = new Node[dimensions.x, dimensions.y];

            //Debug.Log("Tiles have Init");


            for (int x = 0; x < dimensions.x; ++x)
            {
                for (int y = 0; y < dimensions.y; ++y)
                {
                    Vector3 targetPos = GridToWorld(new Vector2Int(x, y), new Vector2Int(1, 1));
                    GameObject newTile = Instantiate(tile, targetPos, Quaternion.identity, this.transform);


                    newTile.transform.localScale = Vector3.one * gridSize;

                    GridTile mTile = newTile.GetComponent<GridTile>();
                    m_Tile[x, y] = new Node(new Vector2Int(x, y), targetPos, mTile, placementStatus.Empty);

                }
            }
        }
        else
        {
            Node[,] tempTile = new Node[dimensions.x, dimensions.y];

            for (int x = 0; x < dimensions.x; ++x)
            {
                for (int y = 0; y < dimensions.y; ++y)
                {

                    if (prevNode.GetLength(0) <= x)
                    {
                        Vector3 targetPos = GridToWorld(new Vector2Int(x, y), new Vector2Int(1, 1));
                        GameObject newTile = Instantiate(tile, targetPos, Quaternion.identity, this.transform);

                        newTile.transform.localScale = Vector3.one * gridSize;
                        GridTile mTile = newTile.GetComponent<GridTile>();
                        tempTile[x, y] = new Node(new Vector2Int(x, y), targetPos, mTile, placementStatus.Empty);
                    }
                    else
                    {
                        tempTile[x, y] = m_Tile[x, y];
                        tempTile[x, y].SetTileStatus(prevNode[x, y]);
                    }


                }
            }
            m_Tile = tempTile;
            tempTile = null;
        }

        if (prevNode == null)
        {
            prevNode = new placementStatus[dimensions.x, dimensions.y];
            SetPrevNode();
            //Debug.Log("prevNode has Init");
        }
        else
        {

            for (int x = 0; x < prevNode.GetLength(0); ++x)
            {
                for (int y = 0; y < prevNode.GetLength(1); ++y)
                {
                    if (prevNode[x, y] != placementStatus.Empty) m_Tile[x, y].SetTileStatus(prevNode[x, y]);

                }
            }

            prevNode = new placementStatus[dimensions.x, dimensions.y];
            SetPrevNode();
            Debug.Log("Reload Compelete");
        }
    }
    private void SetPrevNode()
    {
        for (int x = 0; x < dimensions.x; ++x)
        {
            for (int y = 0; y < dimensions.y; ++y)
            {
                prevNode[x, y] = m_Tile[x, y].GetStatusInNode;
            }
        }
    }

    public void UpgradeGride()
    {

        if (dimensions.x >= 100) return;

        dimensions.x += 10;
        InitGrid();
    }

    private placementStatus[,] GetTile(Vector2Int _gridPos, placementStatus[,] _obj)
    {
        if (_gridPos.x < 0 || _gridPos.y < 0) return null;

        placementStatus[,] tmp = new placementStatus[_obj.GetLength(0), _obj.GetLength(1)];

        for (int x = 0; x < _obj.GetLength(0); ++x)
        {
            for (int y = 0; y < _obj.GetLength(1); ++y)
            {
                tmp[x, y] = m_Tile[x + _gridPos.x, y + _gridPos.y].GetStatusInNode;
            }
        }
        return tmp;
    }
    public void SetTile(Vector3 _gridPos, placementStatus _status)
    {
        Vector2Int curpos = WorldToGrid(_gridPos, Vector2Int.one);

        m_Tile[curpos.x, curpos.y].SetTileStatus(_status);
        prevNode[curpos.x, curpos.y] = _status;
    }
    private void SetTile(Vector2Int _gridPos, placementStatus[,] _status)
    {

        for (int x = 0; x < _status.GetLength(0); ++x)
        {
            for (int y = 0; y < _status.GetLength(1); ++y)
            {
                if (_status[x, y] == placementStatus.Empty) continue;
                m_Tile[x + _gridPos.x, y + _gridPos.y].SetTileStatus(_status[x, y]);

            }
        }
    }

    private void ClearTile()
    {
        foreach (Vector2Int n in preNodeset)
        {
            m_Tile[n.x, n.y].SetTileStatus(prevNode[n.x, n.y]);
        }

        preNodeset.Clear();
    }

    private Vector3 GridToWorld(Vector2Int gridPosition, Vector2Int _sizeOffset)
    {
        Vector3 localPos = new Vector3(gridPosition.x + (_sizeOffset.x * 0.5f), 0, gridPosition.y + (_sizeOffset.y * 0.5f)) * gridSize;

        return transform.TransformPoint(localPos);
    }

    public Vector2Int WorldToGrid(Vector3 worldLocation, Vector2Int sizeOffset)
    {
        Vector3 localLocation = transform.InverseTransformPoint(worldLocation);
        localLocation *= m_InvGridSize;

        var offset = new Vector3(sizeOffset.x * 0.5f, 0.0f, sizeOffset.y * 0.5f);
        localLocation -= offset;

        int xPos = Mathf.RoundToInt(localLocation.x);
        int yPos = Mathf.RoundToInt(localLocation.z);

        return new Vector2Int(xPos, yPos);
    }
    public Node WorldToGridNode(Vector3 _worldPosition)
    {
        Vector2Int curPos = WorldToGrid(_worldPosition, m_GridSize);
        return m_Tile[curPos.x, curPos.y];
    }
    public Vector3[] GetWorldPosWithArray()
    {
        if (path != null)
        {
            Vector3[] nodePos = new Vector3[path.Count];
            for (int i = 0; i < path.Count; ++i)
            {
                nodePos[i] = path[i].GetNodePosInWorld;
            }
            return nodePos;
        }
        else return null;
    }

    public List<Node> GetNeighbours(Node _node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && Mathf.Abs(y) == 1 || Mathf.Abs(x) == 1 && y == 0)
                {
                    int CheckX = _node.GetNodePosInGrid.x + x;
                    int CheckY = _node.GetNodePosInGrid.y + y;

                    if (CheckX >= 0 && CheckX < dimensions.x && CheckY >= 0 && CheckY < dimensions.y)
                    {
                        neighbours.Add(m_Tile[CheckX, CheckY]);
                    }
                }

            }
        }

        return neighbours;
    }



#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.worldToLocalMatrix;

        for (int y = 0; y < dimensions.y; y++)
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                Vector3 position = new Vector3((x + 0.5f) * gridSize, 0, (y + 0.5f) * gridSize);


                if (path != null && m_Tile != null)
                {
                    if (path.Contains(m_Tile[x, y]))
                    {
                        // Gizmos.color = Color.red;
                        // Gizmos.DrawCube(position, new Vector3(gridSize, 0, gridSize));
                    }
                    else
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireCube(position, new Vector3(gridSize, 0, gridSize));

                    }


                }

            }
        }
    }


# endif

}