using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManagerSR : MonoBehaviour
{
    public float ballSpeed;
    public Transform ballsParents;
    public Transform[] playerTransforms;

    private Vector3 movementDirection;

    private void Awake() 
    {
        ballsParents = GameObject.Find("Balls").transform;

        playerTransforms[0] = GameObject.FindGameObjectWithTag("Player 1").transform;
        playerTransforms[1] = GameObject.FindGameObjectWithTag("Player 2").transform;
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
        if(Mathf.Abs(transform.position.x) - transform.localScale.x <= 0f)
            Destroy(gameObject);

        for (int i = 0; i < playerTransforms.Length; i++)
        {
            Vector3 playerPosition = playerTransforms[i].position;
            Vector3 playerScale = playerTransforms[i].localScale;

            //Square lenght/2 * sqr(2) + circle radius is the max distance of collision. SO anything under it is %100 collision
            if((playerPosition - transform.position).magnitude <= (playerScale.x + transform.localScale.x)/2)
            {
                playerTransforms[i].gameObject.GetComponent<PlayerSR>().PlayerReset();
                Destroy(gameObject);
            }
        }
    }
}
