using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeManagerGridless : MonoBehaviour
{
    // TODO delete unused ...
    public GameObject snakeTile;
    public List<SnakeTiles> snakeTiles = new List<SnakeTiles>();
    public Text endGame;
    public SnakePlayerGridless snakeHead;
    public bool spawnNew = false;
    public int snakeSize = 1;
    public int score = 0;

    public void IncreaseSize()
    {
        //If its the tile after the head the program behaves differently since the head is already a part of the program, spawn the new tile after the first one behind the tale tile
        //Adjust parents and ID's of tiles
        GameObject newTile;
        if(snakeTiles.Count == 0)
            newTile = Instantiate(snakeTile,snakeHead.spawnPos,Quaternion.identity);
        else
        {
            newTile = Instantiate(snakeTile,snakeTiles[snakeTiles.Count - 1].spawnPos,Quaternion.identity);
            newTile.GetComponent<SnakeTiles>().tileId = snakeTiles.Count;
        }
        snakeTiles.Add(newTile.GetComponent<SnakeTiles>());
        newTile.transform.parent = transform;
    }
}
