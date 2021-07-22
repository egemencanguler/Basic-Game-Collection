using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryManager : MonoBehaviour
{
    // TODO SnakeManagerGridlessti duzlettim
    public SnakeManager snakeManager;
    public GameObject cherryObject;
    public GameObject snakeHead;
    public int row = 10;
    public int column = 20;
    public int scoreIncrease = 10;

    private float timer = 0f;
    private Vector3 spawnLocation = Vector3.zero;

    private void Update() 
    {
        EatCherry();
    }
    void SpawnCherry()
    {
        int sideRow = Random.Range(0,row);
        int sideColumn =  Random.Range(0,column);

        // TODO burda neler oluyor
        if(sideRow < row/2)
        {
            if(sideColumn < column/2)
            {
                spawnLocation = new Vector3(0.5f + sideColumn,0.5f + sideRow,0);
            }
            else
            {
                spawnLocation = new Vector3((column/2 - 0.5f) - sideColumn,0.5f + sideRow,0);
            }
        }
        else
        {
            if(sideColumn < column/2)
            {
                spawnLocation = new Vector3(0.5f + sideColumn,(row/2 - 0.5f) - sideRow,0);
            }
            else
            {
                spawnLocation = new Vector3((column/2 - 0.5f) - sideColumn,(row/2 - 0.5f) - sideRow,0);
            }
        }

        GameObject newCherry = Instantiate(this.gameObject,spawnLocation,Quaternion.identity);

        for(int i = 0; i < snakeManager.snakeTiles.Count;i++)
        {
            if(spawnLocation == snakeManager.snakeTiles[i].transform.position)
            {
                SpawnCherry();
                Destroy(newCherry);
            }
        }

    }
    void EatCherry()
    {
        if(snakeHead != null)
        {
            if((transform.position - snakeHead.transform.position).magnitude <= transform.localScale.x/2)
            {
                snakeManager.IncreaseSize();
                snakeManager.score += scoreIncrease;
                SpawnCherry();
                Destroy(gameObject);
            }
        }
    }
}
