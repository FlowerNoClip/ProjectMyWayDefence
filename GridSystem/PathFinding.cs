using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private GridSystem grid = null;
    [SerializeField] private Transform start = null;
    [SerializeField] private Transform end = null;
    [SerializeField] private Vector2Int gridSize = Vector2Int.zero;
    private List<Node> pathbyA = null;

    public List<Node> GetPathNode 
    {
        get { return pathbyA; }
    }
    public bool StartPathFind()
    {
        FindPath(start.position, end.position, out bool success);
        //Debug.Log(success);
        return success;
    }
    private void FindPath(Vector3 _startPos, Vector3 _targetPos, out bool _success)
    {
        Node startNode = grid.WorldToGridNode(_startPos);
        Node targetNode = grid.WorldToGridNode(_targetPos);
    
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();


        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node curNode = openSet[0];
            for(int i = 0; i < openSet.Count ; ++i)
            {
                if(openSet[i].fCost < curNode.fCost ||
                     openSet[i].fCost == curNode.fCost && openSet[i].hCost < curNode.hCost)
                {
                    curNode= openSet[i];
                }
            }

            openSet.Remove(curNode);
            closedSet.Add(curNode);

            if(curNode == targetNode) 
            {
                RetracePath(startNode, targetNode);
                _success = true;
                return;
            }

            foreach(Node n in grid.GetNeighbours(curNode))
            {
                if(n.GetStatusInNode != placementStatus.Empty || closedSet.Contains(n))
                {
                    continue;
                }

                int newMovementCostToNeighbour = curNode.gCost + GetDistance(curNode, n);
                if(newMovementCostToNeighbour < n.gCost || !openSet.Contains(n))
                {
                    n.gCost = newMovementCostToNeighbour;
                    n.hCost = GetDistance(n, curNode);
                    n.PreNode = curNode;

                    if(!openSet.Contains(n)) openSet.Add(n);
                  
                }
            }
        }
        _success = false;

    }

    private void RetracePath(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();
        Node curNode = _endNode;


        while(curNode != _startNode)
        {
            path.Add(curNode);
            curNode = curNode.PreNode;
        }
        path.Reverse();

        pathbyA = path;
    }


    private int GetDistance(Node _nodeA, Node _nodeB)
    {
        int dstX = Mathf.Abs(_nodeA.GetNodePosInGrid.x - _nodeB.GetNodePosInGrid.x);
        int dstY = Mathf.Abs(_nodeA.GetNodePosInGrid.y - _nodeB.GetNodePosInGrid.y);

        if(dstX > dstY) return 14 * dstY + 10*(dstX - dstY);

        return 14 * dstX + 10*(dstY - dstX);
    }
}
