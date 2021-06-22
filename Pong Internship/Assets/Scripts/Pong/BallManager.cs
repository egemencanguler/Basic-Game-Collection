using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public float ballSpeed;
    public Transform[] playerTransforms;
    public Camera mainCamera;
    public GameManager gameManager;

    private Vector3 movementDirection;
    private float cameraBorder;
    private static int ballInitialDirection = -1;
    private bool hitWall = false;

    private void Awake() 
    {
        playerTransforms[0] = GameObject.FindGameObjectWithTag("Player 1").transform;
        playerTransforms[1] = GameObject.FindGameObjectWithTag("Player 2").transform;
        mainCamera = Camera.main;
        cameraBorder = mainCamera.orthographicSize;

        transform.parent = GameObject.FindWithTag("GameController").transform;
        gameManager = transform.parent.GetComponent<GameManager>();
    }

    private void Start() 
    {
        movementDirection = (Vector3.right * ballInitialDirection).normalized;
    }

    void Update()
    {

        DestroyOnScreenLimit();
    }

    private void FixedUpdate() 
    {
        CollisionDetection();    
    }

    //Hit detetection
    void CollisionDetection()
    {
        transform.position += (movementDirection * Time.fixedDeltaTime) * ballSpeed;
        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = initialPosition + (movementDirection * Time.fixedDeltaTime) * ballSpeed;
        Vector3 initialToFinal = finalPosition - initialPosition;

        Vector3 playerOnePosition = playerTransforms[0].position;
        Vector3 playerTwoPosition = playerTransforms[1].position;

        Vector3 playerOneScale = playerTransforms[0].localScale;
        Vector3 playerTwoScale = playerTransforms[1].localScale;

        Vector3 vectorPlayerOne = playerOnePosition - transform.position;
        Vector3 vectorPlayerTwo = playerTwoPosition - transform.position;

        //Determine the limits
        float xDistancePlayerLimit = (transform.localScale.x + playerOneScale.x)/2;
        float playerOneYLimitMax = playerOnePosition.y + playerOneScale.y/2;
        float playerOneYLimitMin = playerOnePosition.y - playerOneScale.y/2;
        float playerTwoYLimitMax = playerTwoPosition.y + playerTwoScale.y/2;
        float playerTwoYLimitMin = playerTwoPosition.y - playerTwoScale.y/2;

        //Check if the balls next move reaches and passes the paddle, if it does calculate the distance and make it move to the collision point
        if(initialToFinal.magnitude >= (playerOnePosition - finalPosition).magnitude)
        {
            float distance = (playerOnePosition - initialPosition).magnitude - xDistancePlayerLimit;
            transform.position += movementDirection * distance;
        }

        if(initialToFinal.magnitude >= (playerTwoPosition - finalPosition).magnitude)
        {
            float distance = (playerTwoPosition - initialPosition).magnitude - xDistancePlayerLimit;
            transform.position += movementDirection * distance;
        }

        //Collision detection and change direction
        if(Mathf.Abs(vectorPlayerOne.x) <= xDistancePlayerLimit || initialToFinal.magnitude >= (playerOnePosition - finalPosition).magnitude)
        {
            if(transform.position.y <= playerOneYLimitMax && transform.position.y >= playerOneYLimitMin)
            {
                movementDirection = -vectorPlayerOne.normalized;
                hitWall = false;
            }
        }

        if(Mathf.Abs(vectorPlayerTwo.x) <= xDistancePlayerLimit || initialToFinal.magnitude >= (playerTwoPosition - finalPosition).magnitude)
        {
            if(transform.position.y <= playerTwoYLimitMax && transform.position.y >= playerTwoYLimitMin)
            {
                movementDirection = -vectorPlayerTwo.normalized;
                hitWall = false;
            }
        }

        if((Vector3.up * cameraBorder).y - transform.localScale.y/2 <= Mathf.Abs(transform.position.y))
        {
            //Check if the ball hit the wall in a row, if not then increase the speed otherwise the game gets hard very fast
            if(!hitWall)
            {
                ballSpeed++;
            }
            hitWall = true;
            movementDirection = Vector3.Reflect(movementDirection,Vector3.up);
            //Increase the speed of the ball when the bounces off the wall
        }
    }

    void DestroyOnScreenLimit()
    {
        const float offSet = 1.25f;
        //Check boundaries and score
        if(transform.position.x + offSet < playerTransforms[0].transform.position.x || transform.position.x - offSet > playerTransforms[1].transform.position.x)
        {
            if(transform.position.x > 0f)
            {
                ballInitialDirection = 1;
                Debug.Log("1 Scored!");
                gameManager.Goal(ballInitialDirection);
            }
            else
            {
                ballInitialDirection = -1;
                Debug.Log("2 Scored!");
                gameManager.Goal(ballInitialDirection);
            }
            
            Destroy(gameObject);
        }
    }
}
