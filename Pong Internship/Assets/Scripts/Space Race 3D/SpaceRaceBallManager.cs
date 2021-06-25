using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRaceBallManager : MonoBehaviour
{
    public float ballSpeed;
    public Transform ballsParents;
    public Transform[] playerTransforms;

    private Vector3 movementDirection;

    private void Awake() 
    {
        ballsParents = GameObject.Find("Balls").transform;

        playerTransforms[0] = GameObject.FindWithTag("Player 1").transform;
        playerTransforms[1] = GameObject.FindWithTag("Player 2").transform;
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



        if((transform.position - playerTransforms[0].position).magnitude <= transform.localScale.x/2 + playerTransforms[0].GetComponent<PlayerMechanicsSR>().playerColliderRadius)
        {
            playerTransforms[0].GetComponent<PlayerMechanicsSR>().PlayerReset();
            Destroy(gameObject);
        }

        if((transform.position - playerTransforms[1].position).magnitude <= transform.localScale.x/2 + playerTransforms[1].GetComponent<PlayerMechanicsSR>().playerColliderRadius)
        {
            playerTransforms[1].GetComponent<PlayerMechanicsSR>().PlayerReset();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position,transform.localScale.x/2);    
    }
}
