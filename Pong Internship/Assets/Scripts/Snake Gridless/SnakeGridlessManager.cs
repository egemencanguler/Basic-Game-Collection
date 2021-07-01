using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeGridlessManager : MonoBehaviour
{
    public GameObject snakeHeadObject;
    public GameObject snakeTile;
    public List<GameObject> snakeTiles = new List<GameObject>();
    //public SnakePlayerGridless snakeHead;
    private List<Vector3> prevPos = new List<Vector3>();
    private List<float> timeHolder = new List<float>();
    public Vector3 snakeDirection = Vector3.zero;
    public float cameraYBorder,cameraXBorder,snakeSpeed;
    public Vector3 spawnPos = Vector3.zero;
    private List<int> prevPosId = new List<int>();
    public int prevPosLim = 2000;

    private void Start() 
    {
        snakeTiles.Add(snakeHeadObject);
        spawnPos = snakeTiles[0].transform.position;    
    }

    private void FixedUpdate() 
    {
        Move();    
    }

    void Move()
    {
        if(prevPos.Count <= prevPosLim /*&& snakeTiles.Count > 1*/)
        {
            prevPos.Add(snakeTiles[0].transform.position);
            timeHolder.Add(Time.fixedDeltaTime);
        }
        
        if(prevPos.Count > prevPosLim)
        {
            prevPos.RemoveAt(0);
            timeHolder.Remove(0);
            //prevPos.Add(snakeTiles[0].transform.position);
            for(int a = 0; a < prevPosId.Count; a++)
            {
                prevPosId[a]--;
                prevPosId[a] = Mathf.Clamp(prevPosId[a],0,prevPos.Count);
            }
        }
        //Mouse position to direct the snake
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        if((mousePosition - snakeTiles[0].transform.position).magnitude > transform.localScale.x/2)
        {
            snakeDirection = (mousePosition- snakeTiles[0].transform.position).normalized;
        }

        //Change the direction of the head towards the mouse
        snakeTiles[0].transform.right = snakeDirection;
        //Check if the snake is in boundaries and move the snake head
        //if(Mathf.Abs(snakeTiles[0].transform.position.y) < cameraYBorder && Mathf.Abs(snakeTiles[0].transform.position.x) < cameraXBorder)
        //{
        snakeTiles[0].transform.position += snakeDirection * snakeSpeed * Time.fixedDeltaTime;
        //snakeTiles[0].transform.position += snakeDirection * snakeSpeed * Time.deltaTime;
        //}
        if(snakeTiles.Count > 1)
        {
            for(int a = 1; a < snakeTiles.Count;a++)
            {
                snakeTiles[a].transform.position = Vector3.Lerp(prevPos[prevPosId[a-1]],prevPos[prevPosId[a-1] + 1],timeHolder[prevPosId[a-1]]);
                //snakeTiles[a].transform.position = Vector3.Lerp(prevPos[prevPosId[a-1]],prevPos[prevPosId[a-1] + 1],Time.deltaTime);
                prevPosId[a-1]++;

            }
        }
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        for(int a = 1; a < prevPos.Count; a++)
        {
            Gizmos.DrawLine(prevPos[a-1],prevPos[a]);
        }
    }
    public void IncreaseSize()
    {
        //If its the tile after the head the program behaves differently since the head is already a part of the program, spawn the new tile after the first one behind the tale tile
        //Adjust parents and ID's of tiles
        GameObject newTile;
        if(snakeTiles.Count == 1)
        {
            for(int a = prevPos.Count - 1,i = 1; a >= 0; a--,i++)
            {
                if((prevPos[a] - prevPos[a-i]).magnitude >= transform.localScale.x/4)
                {
                    spawnPos = prevPos[a-1];
                    prevPosId.Add(a-1);
                    break;
                }
            }
            newTile = Instantiate(snakeTile,spawnPos,Quaternion.identity);
        }
        else
        {
            for(int a = prevPosId[snakeTiles.Count - 2],i = 1; a >= 0; a--,i++)
            {
                if((prevPos[a] - prevPos[a-i]).magnitude >= transform.localScale.x/4)
                {
                    spawnPos = prevPos[a-1];
                    prevPosId.Add(a-1);
                    break;
                }
            }
            newTile = Instantiate(snakeTile,spawnPos,Quaternion.identity);
        }
        snakeTiles.Add(newTile);
        newTile.transform.parent = transform.parent;
        prevPosLim+= 2;
        newTile.GetComponent<SpriteRenderer>().sortingOrder -= snakeTiles.Count;
        newTile.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
    }
}
