using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaderManager : MonoBehaviour
{
    public int columnNumber = 10;
    public int rowNumber = 5;
    public float levelSpeed = 1;
    public float moveFrequency = 1;
    public GameObject spaceInvader; // TODO prefab oldugunu belirtmek faydali olabilir spaceInvaderPrefab
    public float invaderXDistance = 1f; // TODO invaderSizeX,invaderSizeZ or Vector2 invaderSize
    public float invaderZDistance = 1f;
    public Vector3 initialSpawnPoint;
    public List<GameObject> canBeShot = new List<GameObject>();
    public float frequencyDecrease = 0.1f;
    private  GameObject[,] spaceInvaderHolder = new GameObject[5,10];

    private float timer = 0;

    private void Start() 
    {
        SpawnInvaders();
        Debug.Log(spaceInvaderHolder.Length);
    }

    private void Update() 
    {
        const float minimumFrequency = 0.5f;
        //Slowly speeds up the movement of the invaders to make the game harder
        if(Time.time - timer >= moveFrequency)
        {
            transform.position -= Vector3.forward * levelSpeed;
            moveFrequency -= frequencyDecrease;
            moveFrequency = Mathf.Clamp(moveFrequency,minimumFrequency,moveFrequency);
            timer = Time.time;    
        }

        CanInvaderShoot();
        CanBeDamaged();
    }

    void SpawnInvaders()
    {
        //This is the row and column system.It also adds the invaders that are spawned to a multi dimensional array so later in the programo we can reach these invaders for different operations.
        spaceInvaderHolder = new GameObject[rowNumber,columnNumber];
        for(int a = 0; a < rowNumber; a++)
        {
            for(int i = 0; i < columnNumber;i++)
            {
                GameObject newSpaceInvader = Instantiate(spaceInvader,initialSpawnPoint + Vector3.right * invaderXDistance * i,Quaternion.Euler(90,0,0));
                SpaceInvader spaceInvaderScript= newSpaceInvader.transform.GetComponent<SpaceInvader>();
                newSpaceInvader.transform.parent = transform;
                if(newSpaceInvader != null)
                    spaceInvaderHolder[a,i] = (newSpaceInvader);
            }
            initialSpawnPoint -= Vector3.forward * invaderZDistance;
        }
    }
    void CanBeDamaged()
    {
        //We use our multi dimensional array to see if the invaders can actually be shot (only the ones that can shoot can also be shot) and assigns them to the list canBeShot (potential enemies list in SpaceInvaderLaser) 
        for(int a = 0; a < rowNumber; a++)
        {
            for(int i = 0; i < columnNumber; i++)
            {
                if(spaceInvaderHolder[a,i] == null)
                {
                    continue;
                }
                else if(spaceInvaderHolder[a,i].GetComponent<SpaceInvader>().canShoot && !canBeShot.Contains(spaceInvaderHolder[a,i]) && spaceInvaderHolder[a,i] != null)
                {
                    canBeShot.Add(spaceInvaderHolder[a,i]);
                }
            }
        }
    }
    public void CanInvaderShoot()
    {
        //Check if the array members of our multi dimensional array are infront of the other invaders. We check that if they are, in their column, the last member of the row. 
        int rowCanShoot = 0;
        for(int a = 0; a < columnNumber;a++)
        {
            for(int i = 0; i < rowNumber;i++)
            {
                if(spaceInvaderHolder[i,a] == null)
                {
                    break;
                }
                rowCanShoot++;
            }
            if(spaceInvaderHolder[Mathf.Clamp(rowCanShoot - 1,0,rowNumber),a] != null)
            {
                spaceInvaderHolder[Mathf.Clamp(rowCanShoot - 1,0,rowNumber),a].GetComponent<SpaceInvader>().canShoot = true;
            }
            rowCanShoot = 0;
        }
    }

}
