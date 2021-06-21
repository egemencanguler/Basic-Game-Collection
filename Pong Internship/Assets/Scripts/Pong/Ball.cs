using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private float ballSpeed;

    [SerializeField]
    private Transform[] playerTransforms;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameManager gameManager;
    #endregion

    #region Variables
    private Vector3 movementVector;
    private float ballInitialDirection = 1;

    private Vector2 yDistancePlayerOneLimit;
    private Vector2 yDistancePlayerTwoLimit;

    private Vector3 borderPositionVector;

    private float cameraBorder;
    #endregion

    #region Awake - Start - Update
    private void Awake() 
    {
        playerTransforms[0] = GameObject.Find("Player 1").gameObject.transform;
        playerTransforms[1] = GameObject.Find("Player 2").gameObject.transform;


        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraBorder = mainCamera.orthographicSize;

        transform.parent = GameObject.Find("Game Manager").transform;
        gameManager = transform.parent.GetComponent<GameManager>();

    }

    private void Start() 
    {
        movementVector = (Vector3.right * gameManager.GetSide()).normalized;
    }

    void Update()
    {
        CollisionDetection();
        transform.position += (movementVector * Time.deltaTime) * ballSpeed; 
        DestroyOnScreenLimit();
    }
    #endregion


    void CollisionDetection()
    {
        //Hit detetection
        Vector3 vectorPlayerOne = playerTransforms[0].position - transform.position;
        Vector3 vectorPlayerTwo = playerTransforms[1].position - transform.position;

        //Determine the limits
        float xDistancePlayerLimit = (transform.localScale.x+ playerTransforms[0].localScale.x)/2;
        yDistancePlayerOneLimit.x = playerTransforms[0].position.y + playerTransforms[0].localScale.y/2;
        yDistancePlayerOneLimit.y = playerTransforms[0].position.y - playerTransforms[0].localScale.y/2;
        yDistancePlayerTwoLimit.x = playerTransforms[1].position.y + playerTransforms[1].localScale.y/2;
        yDistancePlayerTwoLimit.y = playerTransforms[1].position.y - playerTransforms[1].localScale.y/2;

        //Collision detection and change direction
        if(Mathf.Abs(vectorPlayerOne.x) < xDistancePlayerLimit)
        {
            if(transform.position.y <= yDistancePlayerOneLimit.x && transform.position.y >= yDistancePlayerOneLimit.y)
            {
                movementVector = -vectorPlayerOne.normalized;
            }
        }

        if(Mathf.Abs(vectorPlayerTwo.x) < xDistancePlayerLimit)
        {
            if(transform.position.y <= yDistancePlayerTwoLimit.x && transform.position.y >= yDistancePlayerTwoLimit.y )
            {
                Debug.Log(movementVector);
                movementVector = -vectorPlayerTwo.normalized;
            }
        }

        if((Vector3.up * cameraBorder).y - transform.localScale.y/2 <= Mathf.Abs(transform.position.y))
        {
            movementVector = Vector3.Reflect(movementVector,Vector3.up);
            //Increase the speed of the ball when the bounces off the wall
            ballSpeed++;
        }
    }

    void DestroyOnScreenLimit()
    {
        //Check boundaries and score
        if(transform.position.x + 1.25f < playerTransforms[0].transform.position.x || transform.position.x - 1.25f > playerTransforms[1].transform.position.x)
        {
            if(transform.position.x > 0f)
            {
                gameManager.Goal(-1);
            }
            else
            {
                gameManager.Goal(1);
            }
            
            Destroy(gameObject);
        }
    }
}
