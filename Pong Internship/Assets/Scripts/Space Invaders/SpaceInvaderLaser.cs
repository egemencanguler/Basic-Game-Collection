using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaderLaser : MonoBehaviour
{
    // TODO olunce patliyor null ref
    
    
    public float colliderRadius = 0.5f;
    public float colliderHeight = 0.5f;
    public int direction = 0;
    public float laserSpeed = 10f;
    public SpaceInvaderManager invaderManager;
    public SpaceInvaderPlayer player;
    public float screenLimitTop = 65f;
    public float screenLimitBottom = -10f;

    public List<GameObject> potentialEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        invaderManager = GameObject.Find("Space Invaders").GetComponent<SpaceInvaderManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceInvaderPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Collision();
        if(transform.position.z >= screenLimitTop || transform.position.z <= screenLimitBottom)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.position += Vector3.forward * laserSpeed * Time.deltaTime * direction;
    }

    void Collision()
    {
        // TODO iki farkli laser clasi yazilabilirdi bu direction trigi yerine PlayerLaser, EnemyLaser
        // yukarda erisilmesi gereken degiskenlerde farkli asagidaki kodda farkli
        // boyle durumlarda paylasilan kod ayri bi klasa yazilabilir
        
        if(direction > 0)
        {
            //The collision with the invaders
            //There is a list of potential enemies that holds all the enemies that can shoot.
            potentialEnemies = invaderManager.canBeShot;
            for(int a = 0; a < potentialEnemies.Count;a++)
            {
                if(potentialEnemies[a] == null)
                {  
                    potentialEnemies.RemoveAt(a); 
                    break;
                }
                if(((potentialEnemies[a].transform.position) - (transform.position)).magnitude <= potentialEnemies[a].GetComponent<SpaceInvader>().invaderColliderRadius)
                {
                    // TODO bu if neden var?
                    if(potentialEnemies[a].GetComponent<SpaceInvader>())
                    {
                        SpaceInvader invader = potentialEnemies[a].GetComponent<SpaceInvader>();
                        invader.isDestroyed = true;
                    }
                    Destroy(gameObject);
                }
            }
        }
        else
        {   
            //Collisions against the Player
            if((player.transform.position - transform.position).magnitude <= player.playerColliderRadius)
            {
                player.PlayerDamaged();
                Destroy(gameObject);
            }
        }

    }

    /*private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.forward * colliderHeight/2 * direction,colliderRadius/2);

    }*/
}
