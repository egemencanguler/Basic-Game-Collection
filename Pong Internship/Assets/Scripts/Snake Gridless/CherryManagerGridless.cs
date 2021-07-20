using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryManagerGridless : MonoBehaviour
{
    public SnakeGridlessManager snakeManager;
    public int row = 10;
    public int column = 20;
    public int scoreIncrease = 10;
    private Vector3 spawnLocation = Vector3.zero;
    public List<SnakeGridlessManager> snakeHeads = new List<SnakeGridlessManager>();
    
    private void Awake() 
    {
        
        foreach (GameObject snakeHead in GameObject.FindGameObjectsWithTag("Snake Head"))
        {
            snakeHeads.Add(snakeHead.GetComponent<SnakeGridlessManager>());
        }
    }

    private void Update() 
    {
        EatCherry();
    }
    void SpawnCherry()
    {
        int sideRow = Random.Range(0,row);
        int sideColumn =  Random.Range(0,column);

        //Check for borders to spawn
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

        //Do not spawn at the position of any snake tile
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
        //Cherry collision with the snake head
        for(int i = 0; i < snakeHeads.Count; i++)
        {
            if((transform.position - snakeHeads[i].transform.position).magnitude <= transform.localScale.x/2)
            {
                snakeHeads[i].IncreaseSize();
                SpawnCherry();
                Destroy(gameObject);
                break;
            }
        }
    }
}
