using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectT : MonoBehaviour
{
    public placementStatus[] objstate;
    public Vector2Int gridSize;
    public placementStatus[,] obj;

    private void Awake()
    {   
        obj = new placementStatus[gridSize.x, gridSize.y];

        int i = 0;
        for(int x = 0; x < gridSize.x ; ++x)
        {
            for(int y = 0; y < gridSize.y ; ++y)
            {
                obj[x,y] = objstate[i++];
            }
        }
    }

}
