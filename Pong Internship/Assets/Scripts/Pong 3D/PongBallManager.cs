using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBallManager : MonoBehaviour
{
    public float ballSpeed;
    public Transform[] playerTransforms;
    public GameManager gameManager;
    public int fieldBorder = 5;
    public float paddleColliderZOffset = 0.25f;
    public float xScreenLimit = 9f;

    private Vector3 movementDirection;
    private static int ballInitialDirection = -1;
    private bool hitWall = false;
    private Vector3 pointOfIntersection = Vector3.zero;
    private bool playerOneEndlessGoal = false;
    private bool playerTwoEndlessGoal = false;

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
        if(!gameManager.isEndlessMode)
            DestroyOnScreenLimit();
    }

    private void FixedUpdate() 
    {
        CollisionDetection();    
    }

  void CollisionDetection()
    {
        float speed = Mathf.Abs(Time.fixedDeltaTime * ballSpeed);
        Vector3 playerOnePosition = playerTransforms[0].position;
        Vector3 playerTwoPosition = playerTransforms[1].position;

        Vector3 playerOneScale = playerTransforms[0].localScale;
        Vector3 playerTwoScale = playerTransforms[1].localScale;

        Vector3 vectorPlayerOne = (playerOnePosition - transform.position).normalized;
        Vector3 vectorPlayerTwo = (playerTwoPosition - transform.position).normalized;

        //Determine the limits
        float xDistancePlayerLimit = (transform.localScale.x + playerOneScale.x)/2;
        float playerOneYLimitMax = playerOnePosition.z + playerOneScale.z/2;
        float playerOneYLimitMin = playerOnePosition.z - playerOneScale.z/2;
        float playerTwoYLimitMax = playerTwoPosition.z + playerTwoScale.z/2;
        float playerTwoYLimitMin = playerTwoPosition.z - playerTwoScale.z/2;

        float playerOneXComponent = playerOnePosition.x + playerOneScale.x/2;
        float playerOneZComponentMax = playerOnePosition.z + playerOneScale.z/2 + paddleColliderZOffset;
        float playerOneZComponentMin = playerOnePosition.z - playerOneScale.z/2 - paddleColliderZOffset;

        float playerTwoXComponent = playerTwoPosition.x - playerTwoScale.x/2;
        float playerTwoZComponentMax = playerTwoPosition.z + playerTwoScale.z/2 +  paddleColliderZOffset;
        float playerTwoZComponentMin = playerTwoPosition.z - playerTwoScale.z/2 -  paddleColliderZOffset;

        Vector3 previousPoistion = transform.position;
        transform.position += movementDirection * speed;
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = currentPosition + movementDirection * speed;
        bool playerOneLineIntersection =  LineIntersection(previousPoistion,currentPosition,new Vector3(playerOneXComponent,0f,playerOneZComponentMax),new Vector3(playerOneXComponent,0f,playerOneZComponentMin), ref pointOfIntersection);
        bool playerTwoLineIntersection = LineIntersection(previousPoistion,currentPosition,new Vector3(playerTwoXComponent,0f,playerTwoZComponentMax),new Vector3(playerTwoXComponent,0f,playerTwoZComponentMin), ref pointOfIntersection);

        //Debug.DrawLine(currentPosition,previousPoistion,Color.green);

        //If a line (previous position - current position) intersects the line of the paddle collision happens
        if(playerOneLineIntersection)
        {
            transform.position = pointOfIntersection;
            movementDirection = -vectorPlayerOne;
        }

        if(playerTwoLineIntersection)
        {
            transform.position = pointOfIntersection;
            movementDirection = -vectorPlayerTwo;
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

        Vector3 wallIntersection = Vector3.zero;
        bool playerOneWall = LineIntersection(previousPoistion,currentPosition,new Vector3(-xScreenLimit,0f,fieldBorder),new Vector3(-xScreenLimit,0f,-fieldBorder), ref wallIntersection);
        bool playerTwoWall = LineIntersection(previousPoistion,currentPosition,new Vector3(xScreenLimit,0f,fieldBorder),new Vector3(xScreenLimit,0f,-fieldBorder), ref wallIntersection);

        if(gameManager.isEndlessMode)
        {
            if(!playerTwoLineIntersection && playerTwoWall)
            {
                transform.position = wallIntersection;
                playerOneEndlessGoal = true;
                movementDirection = Vector3.Reflect(movementDirection,Vector3.right);
            }
            else if(!playerOneLineIntersection && playerOneWall)
            {
                transform.position = wallIntersection;
                playerTwoEndlessGoal = true;
                movementDirection = Vector3.Reflect(movementDirection,Vector3.right);
            }
        }

        if( transform.position == new Vector3(-xScreenLimit,transform.position.y,transform.position.z))
        {
            ballInitialDirection = -1;
            gameManager.Goal(ballInitialDirection);
            playerOneEndlessGoal = false;
        }

        if(transform.position == new Vector3(xScreenLimit,transform.position.y,transform.position.z))
        {
            ballInitialDirection = 1;
            gameManager.Goal(ballInitialDirection);
            playerTwoEndlessGoal = false;
        }
        Debug.DrawLine(new Vector3(playerOneXComponent,0f,playerTwoZComponentMax),new Vector3(playerOneXComponent,0f,playerOneZComponentMin),Color.red);
        Debug.DrawLine(new Vector3(playerTwoXComponent,0f,playerTwoZComponentMax),new Vector3(playerTwoXComponent,0f,playerOneZComponentMin),Color.red);
    }
    //Line Segment - Line Segment Intersection
    bool LineIntersection(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, ref Vector3 pointOfIntersection)
    {
        Vector3 rLine = point2 - point1;
        Vector3 sLine = point4 - point3;
        
        //r = b - a, s = d - c
        // b = a + t*r, d = c + u*s such t and u that there are pointing to an equal intersection point -> a + t*r = c + u*s
        //The cross product of a vector with itself is 0 so we can multiply the equation to have two new different equations with only 1 variable (t or u) 
        //t = (c - a) x s / (rxs), u = (c - a) x r/ (rxs) -> sxr = -rxs
        float topFractionT = (point3.x - point1.x) * sLine.z - (point3.z - point1.z) * sLine.x;
        float topFractionU = (point3.x - point1.x) * rLine.z - (point3.z - point1.z) * rLine.x;
        float crossRS = rLine.x * sLine.z - rLine.z*sLine.x;
        float t = topFractionT/crossRS;
        float u = topFractionU/crossRS;

        //For scaling the vectors that intersect properly t and u must be between 0 and 1
        if(t > 0f && t < 1f && u < 1f && u > 0f)
        {
            Debug.Log("Intersection");
            pointOfIntersection = point1 + t * rLine;
            return true; 
        }
        return false;
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
