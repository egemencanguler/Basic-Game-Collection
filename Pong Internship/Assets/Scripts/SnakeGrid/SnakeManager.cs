using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeManager : MonoBehaviour
{
    public GameObject snakeTile;
    public List<GameObject> snakeTiles = new List<GameObject>();

    public Text endGame;
    public SnakePlayer snakeHead;
    public bool spawnNew = false;
    public int snakeSize = 1;
    public int score = 0;

    private void Update() 
    {
        DestroySnake();
    }
    public void ScaleSnake()
    {

        Vector3 newSpawnLocation = snakeHead.transform.position - snakeHead.direction;
        GameObject newTile = Instantiate(snakeTile,newSpawnLocation,Quaternion.identity);
        snakeTiles.Add(newTile);
        newTile.transform.parent = transform;
        spawnNew = false;
    }

    public void DeleteExtraTiles(ref int movedSize)
    {
        // TODO her kareket ettigimizde bi parcamizi yok etmek pek iyi bir fikir olmayabilir
        // cok kisa birsuru fonkyon ve class birbirini dongu seklinde cagiriyor cok basit olan bu kodu karmasik hale getiriyor
        
        if(movedSize >= snakeSize)
        {
            Destroy(snakeTiles[0]);
            snakeTiles.RemoveAt(0);
            movedSize = snakeSize - 1;
        }
    }

    public void DestroySnake()
    {
        for(int i = 0; i < snakeTiles.Count;i++)
        {
            if(snakeHead.transform.position == snakeTiles[i].transform.position)
            {
                endGame.text = "Game Over | Score: " + score;
                Destroy(gameObject);
            }
        }
    }

    public void IncreaseSize()
    {
        snakeSize++;
    }
}
