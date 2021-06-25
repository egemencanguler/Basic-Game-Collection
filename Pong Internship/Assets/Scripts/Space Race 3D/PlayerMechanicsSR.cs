using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanicsSR : MonoBehaviour
{
    public float playerSpeed = 25f;
    public bool playerOne = true;
    public Camera mainCamera;
    public float xOffSetLimit = 1f;
    public float zBorderOffSetLimit;
    public float cameraZOffSet = 1f;
    public float gameStartDelay = 3f;
    public Vector2 spawnPoint;
    public float playerColliderRadius = 1f;

    public float cameraBordersXMax;
    public float cameraBordersXMin;
    private int vertical;
    private int horizontal;


    private void Awake() 
    {
        spawnPoint = transform.position;
        zBorderOffSetLimit = transform.position.z;
    }

    void Update()
    {
        MovementInput();
        //if(Time.time >= gameStartDelay)
            transform.position += (Vector3.forward * vertical + Vector3.right * horizontal) * Time.deltaTime * playerSpeed;
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,mainCamera.transform.position.y,transform.position.z - cameraZOffSet);
        
    }

    void MovementInput()
    {
        //Vector3 vector = ((Vector3.up * cameraBorder) - transform.position);
        // -1 or 0 or 1 for vertical direction for input
        switch(playerOne)
        {
            case true:
                if(Input.GetKey(KeyCode.W))
                {
                    vertical = 1;
                }
                
                if(Input.GetKey(KeyCode.S))
                {
                    vertical = -1;
                }

                if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    vertical = 0;
                }

                if(Input.GetKey(KeyCode.D))
                {
                    horizontal = 1;
                }
                
                if(Input.GetKey(KeyCode.A))
                {
                    horizontal = -1;
                }
            

                if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    horizontal = 0;
                }
                break;
            case false:
                if(Input.GetKey(KeyCode.UpArrow))
                {
                    vertical = 1;
                }
                
                if(Input.GetKey(KeyCode.DownArrow))
                {
                    vertical = -1;
                }
                if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    vertical = 0;
                }

                if(Input.GetKey(KeyCode.RightArrow))
                {
                    horizontal = 1;
                }
                
                if(Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontal = -1;
                }

                if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    horizontal = 0;
                }
                break;
        }

        if(transform.position.x > cameraBordersXMax - xOffSetLimit)
        {
            horizontal = Mathf.Clamp(horizontal,-1,0);
        }
        else if(transform.position.x < cameraBordersXMin + xOffSetLimit)
        {
            horizontal = Mathf.Clamp(horizontal,0,1);
        }

        if(transform.position.z <= zBorderOffSetLimit)
        {
            vertical = Mathf.Clamp(vertical,0,1);
        }
        
    }

    public void PlayerReset()
    {
        //Reset the position
        transform.position = spawnPoint;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position,playerColliderRadius);    
    }
}
