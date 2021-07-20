using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContinuousSnake 
{
    public class SnakeMove : MonoBehaviour
    {
        public float snakeSpeed = 10f;
        public GameObject snakeTail;
        public List<GameObject> tailObjects = new List<GameObject>();
        private Vector3 direction = Vector3.zero;
        private bool isFPSDropped = false;
        private bool isSpawned = false;
        private List<Vector3> prevPos = new List<Vector3>();
        private List<Vector3> prevDirection = new List<Vector3>();
        private List<float> prevDeltaTime = new List<float>();
        private List<int> tailSpawnPosIndex = new List<int>();
        private void Start()
        {
            tailObjects.Add(this.gameObject);
            tailSpawnPosIndex.Add(0);
            Application.targetFrameRate = 60;
        }
        private void Update()
        {


            float deltaTime = Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.S))
            {
                isSpawned = true;
            }

            if (isSpawned)
            {
                isSpawned = false;
                GameObject tail = Instantiate(snakeTail, tailObjects[tailObjects.Count - 1].transform.position, Quaternion.identity);
                tailObjects.Add(tail);
                if (tailObjects.Count == 2) 
                {
                    tailSpawnPosIndex.Add(prevPos.Count);
                }
                else
                {
                    for (int a = prevPos.Count - 1; a >= 0; a--) 
                    {
                        if (prevPos[a] == tail.transform.position) 
                        {
                            tailSpawnPosIndex.Add(a);
                            break;
                        }
                    }
                }
            }

            for (int  a = 0; a < tailObjects.Count; a++) 
            {
                if (a == 0) 
                {
                    SnakeMoveFunc(transform,deltaTime);
                }
                else 
                {

                    if (PathDistanceCalculator(tailSpawnPosIndex[a],tailSpawnPosIndex[a-1]) >= transform.localScale.x/2) 
                    {
                        Debug.Log(PathDistanceCalculator(tailSpawnPosIndex[a], tailSpawnPosIndex[a - 1]));
                        tailObjects[a].transform.position = prevPos[tailSpawnPosIndex[a] + 1];
                        tailSpawnPosIndex[a]++;
                    }
                }
            }
            prevDeltaTime.Add(deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isFPSDropped = !isFPSDropped;
                if (isFPSDropped)
                {
                    Application.targetFrameRate = 10;
                }
                else
                {
                    Application.targetFrameRate = 60;
                }
            }
        }
        void SnakeMoveFunc(Transform snakeObjectTransform,float deltaTime) 
        {
            prevPos.Add(transform.position);
            prevDirection.Add(direction);
            tailSpawnPosIndex[0]++;
            /*Debug.Log(prevPos.Count + " POS");
            Debug.Log(prevDirection.Count + " DIR");
            Debug.Log(tailSpawnPosIndex[0] + " INDEX");*/
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = new Vector3(direction.x,direction.y,0f).normalized;
            snakeObjectTransform.position += direction * deltaTime * snakeSpeed;
        }

        float PathDistanceCalculator(int indexStart, int indexEnd) 
        {
            float distance = 0f;
            for (int a = indexStart; a < indexEnd; a++) 
            {
                distance += (prevPos[a] - prevPos[indexEnd - 1]).magnitude;
            }
            return distance;
        }

        int ClosestPointCalculator(int beginIndex, int endIndex) 
        {
            float distance = 0f;
            for (int a = endIndex; a > beginIndex; a--)
            {
                distance += Vector2.Distance(prevPos[beginIndex],prevPos[endIndex]);
                if (distance >= transform.localScale.x/2) 
                {
                    return a;
                }
            }
            return 0;
        }

        private void OnDrawGizmos()
        {

            if (prevPos.Count >= 2) 
            {
                for (int a = 0; a < prevPos.Count; a++ ) 
                {
                    if (a != prevPos.Count - 1) 
                    {
                        if (Gizmos.color != Color.red) 
                        {
                            Gizmos.color = Color.red;
                        }
                        else 
                        {
                            Gizmos.color = Color.green;
                        }
                        Gizmos.DrawLine(prevPos[a],prevPos[a + 1]);
                    }
                }
            }
        }
    }

}
