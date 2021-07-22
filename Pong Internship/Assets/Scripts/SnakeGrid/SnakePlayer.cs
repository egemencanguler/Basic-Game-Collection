using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePlayer : MonoBehaviour
{
    // TODO herkesin birbirine eristigi 3 class var :(
    
    
    public float speed = 1f;
    public float frequency = 1f;
    public Vector3 direction = Vector3.right;
    public int moveSize = 0;
    public SnakeManager snakeManager;
    public int row = 10;
    public int column = 20;
    public int cameraYBorder = 5;
    public int cameraXBorder = 10;


    private float timer = 0f;

    void Update()
    {
        PlayerInput();
        Move();
    }

    void PlayerInput()
    {
        if(Input.GetKeyDown(KeyCode.W) && direction != -Vector3.up)
        {
            direction = Vector3.up;
        }
        else if(Input.GetKeyDown(KeyCode.S) && direction != Vector3.up)
        {
            direction = -Vector3.up;
        }

        if(Input.GetKeyDown(KeyCode.D) && direction != -Vector3.right)
        {
            direction = Vector3.right;
        }
        else if(Input.GetKeyDown(KeyCode.A) && direction != Vector3.right)
        {
            direction = -Vector3.right;
        }
    }
    void Move()
    {
        //(Border Limit)
        if(Time.time - timer >= frequency)
        {
            timer = Time.time;
            if(Mathf.Abs(transform.position.y) < cameraYBorder && Mathf.Abs(transform.position.x) < cameraXBorder)
            {
                transform.position += direction * speed;
            }
            else if(Mathf.Abs(transform.position.y) > cameraYBorder)
            {
                if(transform.position.y < 0f)
                    transform.position = new Vector3(transform.position.x, cameraYBorder - 0.5f,0f);
                else
                {
                    transform.position = new Vector3(transform.position.x, -cameraYBorder + 0.5f,0f);
                }
            }
            else if(Mathf.Abs(transform.position.x) > cameraXBorder)
            {
                if(transform.position.x < 0f)
                {
                    transform.position = new Vector3(cameraXBorder - 0.5f, transform.position.y,0f);
                }
                else
                {
                    transform.position = new Vector3(-cameraXBorder + 0.5f, transform.position.y,0f);
                }
            }

            moveSize++;
            snakeManager.ScaleSnake();
            snakeManager.DeleteExtraTiles(ref moveSize);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        for(int a = 0; a < row; a++)
        {
            for (int i = 0; i < column; i++)
            {
                if(a < row/2)
                {
                    if(i < column/2)
                    {
                        Gizmos.DrawWireCube(new Vector3(0.5f + i,0.5f + a,0),new Vector3(1,1,0));
                    }
                    else
                    {
                        Gizmos.DrawWireCube(new Vector3((column/2 - 0.5f) - i,0.5f + a,0),new Vector3(1,1,0));
                    }
                }
                else
                {
                    if(i < column/2)
                    {
                        Gizmos.DrawWireCube(new Vector3(0.5f + i,(row/2 - 0.5f) - a,0),new Vector3(1,1,0));
                    }
                    else
                    {
                        Gizmos.DrawWireCube(new Vector3((column/2 - 0.5f) - i,(row/2 - 0.5f) - a,0),new Vector3(1,1,0));
                    }
                }
            }
        }

    }
}
