using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBallManager : MonoBehaviour
{
    public float ballSpeed;
    public Transform[] playerTransforms;
    public GameManager gameManager;
    public int fieldBorder = 5;

    private Vector3 movementDirection;
    private static int ballInitialDirection = -1;
    private bool hitWall = false;

    private void Awake() 
    {
        playerTransforms[0] = GameObject.FindGameObjectWithTag("Player 1").transform;
        playerTransforms[1] = GameObject.FindGameObjectWithTag("Player 2").transform;

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
        movementDirection.y = 0;
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
        float playerOneYLimitMax = playerOnePosition.z + playerOneScale.z/2;
        float playerOneYLimitMin = playerOnePosition.z - playerOneScale.z/2;
        float playerTwoYLimitMax = playerTwoPosition.z + playerTwoScale.z/2;
        float playerTwoYLimitMin = playerTwoPosition.z - playerTwoScale.z/2;

        //Collision detection and change direction
        
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

        //Collision
        if(Mathf.Abs(vectorPlayerOne.x) <= xDistancePlayerLimit || initialToFinal.magnitude >= (playerOnePosition - finalPosition).magnitude)
        {
            if(transform.position.z <= playerOneYLimitMax && transform.position.z >= playerOneYLimitMin)
            {
                movementDirection = -vectorPlayerOne.normalized;
                hitWall = false;
            }
        }

        if(Mathf.Abs(vectorPlayerTwo.x) <= xDistancePlayerLimit || initialToFinal.magnitude >= (playerTwoPosition - finalPosition).magnitude)
        {
            if(transform.position.z <= playerTwoYLimitMax && transform.position.z >= playerTwoYLimitMin)
            {
                movementDirection = -vectorPlayerTwo.normalized;
                hitWall = false;
            }
        }

        if((Vector3.forward * fieldBorder).z - transform.localScale.z/2 <= Mathf.Abs(transform.position.z))
        {
            //Check if the ball hit the wall in a row, if not then increase the speed otherwise the game gets hard very fast
            if(!hitWall)
            {
                ballSpeed++;
            }
            hitWall = true;
            movementDirection = Vector3.Reflect(movementDirection,Vector3.forward);
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
