using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSR : MonoBehaviour
{
    public float playerSpeed = 25f;
    public bool playerOne = true;
    public Camera mainCamera;
    public float gameStartDelay = 3f;
    public Vector2 spawnPoint;

    private float vertical;
    private float cameraBorder;


    // Start is called before the first frame update
    private void Awake() 
    {
        mainCamera = Camera.main;
        //Get camera size to limit the movement of the player
        cameraBorder = mainCamera.orthographicSize;

        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        MovementInput();
        if(Time.time >= gameStartDelay)
            gameObject.transform.position += Vector3.up * playerSpeed * vertical * Time.deltaTime;
        
    }

    void MovementInput()
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
        // TODO float == comparison visual studio uyarmiyormu, int yapabilirisn direk zaten int
        if(vertical == 1 && vector.y < transform.localScale.y/2 || vertical == -1 && vector.y > cameraBorder * 2 -transform.localScale.y/2 )
        {
            vertical = 0;
        }
    }

    public void PlayerReset()
    {
        //Reset the position
        transform.position = spawnPoint;
    }
}
