using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTesting : MonoBehaviour
{
    public Vector3 movementDirection;
    public int ballSpeed;

    public Transform[] playerTransforms;
    
    private Vector3 pointOfIntersection = Vector3.zero;

    // Start is called before the first frame update
    private void Awake() 
    {
        playerTransforms[0] = GameObject.FindGameObjectWithTag("Player 1").transform;
        playerTransforms[1] = GameObject.FindGameObjectWithTag("Player 2").transform;    
    }
        

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate() 
    {
        Move(movementDirection, ballSpeed);
        Vector3 playerOnePosition = playerTransforms[0].position;
        Vector3 playerTwoPosition = playerTransforms[1].position;

        Vector3 playerOneScale = playerTransforms[0].localScale;
        Vector3 playerTwoScale = playerTransforms[1].localScale;
        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = initialPosition + (movementDirection * Time.fixedDeltaTime) * ballSpeed;

        float playerOneXComponent = playerOnePosition.x + playerOneScale.x/2;
        float playerOneYComponentMax = playerOnePosition.y + playerOneScale.y/2;
        float playerOneYComponentMin = playerOnePosition.y - playerOneScale.y/2;

        if(LineIntersection(initialPosition,finalPosition,new Vector3(playerOneXComponent,playerOneYComponentMax + 1,0),new Vector3(playerOneXComponent,playerOneYComponentMin - 1,0),ref pointOfIntersection))
        {
            Debug.Log(pointOfIntersection);
        }    
    }
    
    void Move(Vector3 direction, int speed)
    {
        transform.position += movementDirection.normalized * Time.deltaTime * speed;
    }

    bool LineIntersection(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, ref Vector3 pointOfIntersection)
    {
        Vector3 rLine = point2 - point1;
        Vector3 sLine = point4 - point3;
        
        float topFractionT = (point3.x - point1.x) * sLine.y - (point3.y - point1.y) * sLine.x;
        float topFractionU = (point3.x - point1.x) * rLine.y - (point3.y - point1.y) * rLine.x;
        float crossRS = rLine.x * sLine.y - rLine.y*sLine.x;
        float t = topFractionT/crossRS;
        float u = topFractionU/crossRS;

        if(t > 0f && t < 1f && u < 1f && u > 0f)
        {
            pointOfIntersection = point1 + t * rLine;
            return true; 
        }
        return false;
    }

    /*private void OnDrawGizmos() {
        Vector3 playerOnePosition = playerTransforms[0].position;
        Vector3 playerTwoPosition = playerTransforms[1].position;

        Vector3 playerOneScale = playerTransforms[0].localScale;
        Vector3 playerTwoScale = playerTransforms[1].localScale;

        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = initialPosition + (movementDirection * Time.fixedDeltaTime) * ballSpeed;

        float playerOneXComponent = playerOnePosition.x + playerOneScale.x/2;
        float playerOneYComponentMax = playerOnePosition.y + playerOneScale.y/2;
        float playerOneYComponentMin = playerOnePosition.y - playerOneScale.y/2;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(playerOneXComponent,playerOneYComponentMax + 1,0),new Vector3(playerOneXComponent,playerOneYComponentMin - 1,0));
        Gizmos.DrawLine(initialPosition,finalPosition);
    }*/
}
