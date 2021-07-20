using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //THIS IS FOR GIZMOS ONLY
    public int row = 10;
    public int column = 10;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int a = 0; a < row; a++)
        {
            for (int i = 0; i < column; i++)
            {
                if (a < row / 2)
                {
                    if (i < column / 2)
                    {
                        Gizmos.DrawWireCube(new Vector3(0.5f + i, 0, 0.5f + a), new Vector3(1, 0, 1));
                    }
                    else
                    {
                        Gizmos.DrawWireCube(new Vector3((column / 2 - 0.5f) - i,0, 0.5f + a), new Vector3(1, 0, 1));
                    }
                }
                else
                {
                    if (i < column / 2)
                    {
                        Gizmos.DrawWireCube(new Vector3(0.5f + i, 0, (row / 2 - 0.5f) - a), new Vector3(1, 0, 1));
                    }
                    else
                    {
                        Gizmos.DrawWireCube(new Vector3((column / 2 - 0.5f) - i, 0, (row / 2 - 0.5f) - a), new Vector3(1, 0, 1));
                    }
                }
            }
        }
    }
}
