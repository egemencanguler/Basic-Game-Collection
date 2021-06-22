using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerInputController : MonoBehaviour
{
    public float playerSpeed = 25f;
    public bool playerOne = true;
    public int fieldMoveLimit = 5;
    private float horizontal;

    void Update()
    {
        Vector3 vector = ((Vector3.forward * fieldMoveLimit) - transform.position);

        // -1 or 0 or 1 for vertical direction for input
        switch(playerOne)
        {
            case true:
                if(Input.GetKey(KeyCode.W))
                {
                    horizontal = 1;
                }
                
                if(Input.GetKey(KeyCode.S))
                {
                    horizontal = -1;
                }

                if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    horizontal = 0;
                }
                break;
            case false:
                if(Input.GetKey(KeyCode.UpArrow))
                {
                    horizontal = 1;
                }
                
                if(Input.GetKey(KeyCode.DownArrow))
                {
                    horizontal = -1;
                }
                if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    horizontal = 0;
                }
                break;
        }

        //Stop at borders
        if(horizontal == 1 && vector.z < transform.localScale.z/2 || horizontal == -1 && vector.z > fieldMoveLimit * 2 -transform.localScale.z/2)
        {
            horizontal = 0;
        }

        gameObject.transform.position += Vector3.forward * playerSpeed * horizontal * Time.deltaTime;
        
    }
}
