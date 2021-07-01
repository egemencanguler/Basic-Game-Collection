using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTiles : MonoBehaviour
{

    public SnakeManagerGridless snakeManager;
    public Vector3 spawnPos = Vector3.zero;
    public List<Vector3> prevPos = new List<Vector3>();
    public Vector3 prevDir = Vector3.zero;
    public int tileId = 0;

    private void Awake() 
    {
        prevPos.Add(transform.position);  
    }

    private void Start() 
    {
        snakeManager = transform.parent.GetComponent<SnakeManagerGridless>();
        spawnPos = snakeManager.snakeHead.spawnPos;
        this.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);  
    }
    void Update()
    {
        //Store direction for the tiles behind this one
        prevDir = transform.right;
        //For spawning the next tile
        if((spawnPos - transform.position).magnitude > transform.localScale.x/2)
        {
            spawnPos = transform.position;
        }
        if(prevPos.Count < 5000 && snakeManager.snakeTiles.Count > tileId + 1)
        {
            prevPos.Add(transform.position);
        }
        //The first tile of the tale will behave differently, it depends on the snake head while others depend on each other.
        if(tileId == 0)
        {
            //Use the previous direrction if the snake head reached the screen boundary by comparing the distance
            //if((snakeManager.snakeHead.prevPos[0] - snakeManager.snakeHead.transform.position).magnitude > transform.localScale.x/2)
            //{
                transform.position += (snakeManager.snakeHead.prevPos[0] - transform.position).normalized * snakeManager.snakeHead.speed * Time.deltaTime;
                //transform.right = snakeManager.snakeHead.prevDir;
                snakeManager.snakeHead.prevPos.RemoveAt(0);
            //}
            /*else
            {
                //Continue if the snake head reached screen border
                transform.position += transform.right * snakeManager.snakeHead.speed * Time.deltaTime;
            }*/
        }
        else
        {
            //Check the boundary of the tale tile infront of this one by comparing distance
            /*if((snakeManager.snakeTiles[tileId - 1].transform.position - transform.position).magnitude < 2)
            {
                //If the previous snake tile moved move this tile towards the previous one
                if(snakeManager.snakeTiles[tileId - 1].spawnPos != snakeManager.snakeTiles[tileId - 1].transform.position)
                {*/
                    transform.position += (snakeManager.snakeTiles[tileId - 1].prevPos[0]- transform.position).normalized * snakeManager.snakeHead.speed * Time.deltaTime;
                    snakeManager.snakeTiles[tileId - 1].prevPos.RemoveAt(0);
                    //transform.right = snakeManager.snakeTiles[tileId-1].prevDir;
                //}
            //}
            /*else
            {
                //Continue if the previous one reached screen border
                transform.position += transform.right * snakeManager.snakeHead.speed * Time.deltaTime;
            }*/
        }
        //If the tile reached the screen border move it to the other border
        if(Mathf.Abs(transform.position.y) > snakeManager.snakeHead.cameraYBorder)
        {
            if(transform.position.y < 0f)
            {
                transform.position = new Vector3(transform.position.x, snakeManager.snakeHead.cameraYBorder - 0.5f,0f);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, -snakeManager.snakeHead.cameraYBorder + 0.5f,0f);
            }
        }
        else if(Mathf.Abs(transform.position.x) > snakeManager.snakeHead.cameraXBorder)
        {
            if(transform.position.x < 0f)
            {
                transform.position = new Vector3(snakeManager.snakeHead.cameraXBorder - 0.5f, transform.position.y,0f);
            }
            else
            {
                transform.position = new Vector3(-snakeManager.snakeHead.cameraXBorder + 0.5f, transform.position.y,0f);
            }
        }
    }

    private void OnDrawGizmos() 
    {
        switch (tileId)
        {
            case 0:
                Gizmos.color = Color.blue;
                break;
            case 1:
                Gizmos.color = Color.green;
                break;
            case 2:
                Gizmos.color = Color.magenta;
                break;
            case 3:
                Gizmos.color = Color.white;
                break;
            case 4:
                Gizmos.color = Color.yellow;
                break;
            default:
                Gizmos.color = Color.cyan;
                break;
        }
        for(int a = 1; a < prevPos.Count; a++)
        {
            Gizmos.DrawLine(prevPos[a-1],prevPos[a]);
        }    
    }
}
