using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 25f;

    [SerializeField]
    private bool playerOne = true;

    [SerializeField]
    private Camera mainCamera;

    private float vertical;

    private float cameraBorder;


    // Start is called before the first frame update
    private void Awake() 
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Get camera size to limit the movement of the player
        cameraBorder = mainCamera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = ((Vector3.up * cameraBorder) - transform.position);


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
                break;
        }

        //Stop at borders
        if(vertical == 1 && vector.y < transform.localScale.y/2 || vertical == -1 && vector.y > cameraBorder * 2 -transform.localScale.y/2 )
        {
            vertical = 0;
        }

        gameObject.transform.position += Vector3.up * playerSpeed * vertical * Time.deltaTime;
        
    }
}
