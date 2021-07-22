using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManagerSR : MonoBehaviour
{
    // TODO No one using ball manager - 2d version uses it but it is not ball manager but ball
    
   [SerializeField]
    private float ballSpeed;
    [SerializeField]
    private Transform ballsParents;

    [SerializeField]
    private Transform[] playerTransforms;

    private Vector3 movementDirection;

    private void Awake() 
    {
        Debug.Break();
        gameObject.name = "BallManager";
        // TODO NEVER use find with object name
        ballsParents = GameObject.Find("Balls").transform;

        playerTransforms[0] = GameObject.Find("Player 1").transform;
        playerTransforms[1] = GameObject.Find("Player 2").transform;
    }
    void Start()
    {
        //Making the balls move towards the center line
        transform.parent = ballsParents;
        if(transform.position.x < 0)
            movementDirection = Vector3.right;
        else
            movementDirection = -Vector3.right;
    }

    void Update()
    {
        transform.position += (movementDirection*Time.deltaTime * ballSpeed);
        DestroyBall();
    }

    void DestroyBall()
    {
        //Square lenght/2 * sqr(2) + circle radius is the max distance of collision. SO anything under it is %100 collision
        if(Mathf.Abs(transform.position.x) - transform.localScale.x <= 0f)
            Destroy(gameObject);
        if((playerTransforms[0].position - transform.position).magnitude <= (playerTransforms[0].localScale.x*Mathf.Sqrt(2) + transform.localScale.x)/2)
        {
            playerTransforms[0].gameObject.GetComponent<PlayerSR>().PlayerReset();
            Destroy(gameObject);
        }
        if((playerTransforms[1].position - transform.position).magnitude <= (playerTransforms[0].localScale.x*Mathf.Sqrt(2) + transform.localScale.x)/2)
        {
            playerTransforms[1].gameObject.GetComponent<PlayerSR>().PlayerReset();
            Destroy(gameObject);
        }
    }
}
