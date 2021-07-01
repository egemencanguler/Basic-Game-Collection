using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePlayerGridless : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction = Vector3.right;
    public int cameraYBorder = 5;
    public int cameraXBorder = 10;
    public Vector3 spawnPos = Vector3.zero;
    public List<Vector3> prevPos = new List<Vector3>();
    public Vector3 prevDir = Vector3.zero;
    private float timer = 0f;

    private void Start() 
    {
        spawnPos = transform.position;    
    }

    void Update()
    {
        Move();
    }
    void Move()
    {
        if(prevPos.Count < 5000 && transform.parent.GetComponent<SnakeManagerGridless>().snakeTiles.Count > 0)
        {
            prevPos.Add(transform.position);
        }
        //Take the direction for the tale tiles
        prevDir =  direction;
        //Mouse position to direct the snake
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        if((mousePosition - transform.position).magnitude > transform.localScale.x/2)
        {
            direction = (mousePosition- transform.position).normalized;
        }
        //Change the direction of the head towards the mouse
        transform.right = direction;

        //Check if the snake is in boundaries and move the snake head
        if(Mathf.Abs(transform.position.y) < cameraYBorder && Mathf.Abs(transform.position.x) < cameraXBorder)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        //This section makes the snake head move boundary to boundary
        else if(Mathf.Abs(transform.position.y) > cameraYBorder)
        {
            if(transform.position.y < 0f)
            {
                transform.position = new Vector3(transform.position.x, cameraYBorder - 0.5f,0f);
            }
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
        //Where the first tale will be spawned
        if((spawnPos - transform.position).magnitude > transform.localScale.x/2)
        {
            spawnPos = transform.position;
        }

    }

    private void OnDrawGizmos() 
    {
        Vector3 mousePositionSnake = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,spawnPos);
        Gizmos.color = Color.red;
        for(int a = 1; a < prevPos.Count; a++)
        {
            Gizmos.DrawLine(prevPos[a-1],prevPos[a]);
        }
    }
}
