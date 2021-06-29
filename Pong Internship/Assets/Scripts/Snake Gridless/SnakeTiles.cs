using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTiles : MonoBehaviour
{

    public SnakeManagerGridless snakeManager;
    public Vector3 prevPos = Vector3.zero;
    public Vector3 prevDir = Vector3.zero;
    public int tileId = 0;

    private void Start() 
    {
        snakeManager = transform.parent.GetComponent<SnakeManagerGridless>();
    }
    void Update()
    {
        //Store direction for the tiles behind this one
        prevDir = transform.right;
        //For spawning the next tile
        if((prevPos - transform.position).magnitude > transform.localScale.x/2)
        {
            prevPos = transform.position;
        }

        //The first tile of the tale will behave differently, it depends on the snake head while others depend on each other.
        if(tileId == 0)
        {
            //Use the previous direrction if the snake head reached the screen boundary by comparing the distance
            if((snakeManager.snakeHead.transform.position - transform.position).magnitude < 2)
            {
                Debug.DrawLine(snakeManager.snakeHead.transform.position ,transform.position);
                transform.position += (snakeManager.snakeHead.prevSpawnPos - transform.position).normalized * snakeManager.snakeHead.speed * Time.deltaTime;
                transform.right = snakeManager.snakeHead.prevDir;
            }
            else
            {
                //Move it towards the snake head direction
                transform.position += transform.right * snakeManager.snakeHead.speed * Time.deltaTime;
            }
        }
        else
        {
            //Check the boundary of the tale tile infront of this one by comparing distance
            if((snakeManager.snakeTiles[tileId - 1].transform.position - transform.position).magnitude < 2)
            {
                //If the previous snake tile moved move this tile towards the previous one
                if(snakeManager.snakeTiles[tileId - 1].prevPos != snakeManager.snakeTiles[tileId - 1].transform.position)
                {
                    transform.position += (snakeManager.snakeTiles[tileId - 1].prevPos- transform.position).normalized * snakeManager.snakeHead.speed * Time.deltaTime;
                    transform.right = snakeManager.snakeTiles[tileId-1].prevDir;
                }
            }
            else
            {
                //Continue if the previous one reached screen border
                transform.position += transform.right * snakeManager.snakeHead.speed * Time.deltaTime;
            }
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
}
