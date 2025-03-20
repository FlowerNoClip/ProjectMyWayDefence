using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private Vector2Int gridPos;
    private Vector3 worldPos; 
    private Node prevNode;
    private placementStatus placementStatus;
    private GridTile placementTile;
    public int gCost;
    public int hCost;


    public Node(Vector2Int _gridPos, Vector3 _worldPos, GridTile _tile, placementStatus _status)
    {
        this.gridPos = _gridPos;
        this.worldPos = _worldPos;
        this.placementTile = _tile;
        placementTile.SetNumber(gridPos);
        this.placementStatus = _status;
    }


    public placementStatus GetStatusInNode
    {
        get { return placementStatus; }
    }

    public void SetTileStatus(placementStatus _status)
    {
        placementTile.SetStatus(_status);
        placementStatus = _status;
    }
    public Node PreNode
    {
        set { prevNode = value; }
        get { return prevNode; }
    }
    public int fCost
    {
        get { return gCost + hCost; }  
    }
    public Vector2Int GetNodePosInGrid
    {
        get { return gridPos; }
    }
    public Vector3 GetNodePosInWorld
    {
        get { return worldPos; } 
    }

    
}