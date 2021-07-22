using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaderPlayer : MonoBehaviour
{
   public float playerSpeed;
    public float minXLimit;
    public float maxXLimit;
    public float playerColliderRadius = 1f;
    public float shootingRate = 1f;
    public GameObject playerLaser; // TODO sonuna prefab eklenebilir instantiate edilecegi belli olsun playerLaserPrefab
    public Transform shootingPoint;
    public SpaceInvaderManager invaderManager; // TODO kullanilmayan degiskenleri kaldir 
    public int health = 3;
    private int horizontal = 0;
    private float shootTimer = 0f;

    private void Update() 
    {
        horizontal = PlayerHorizontalInput();
        Move(ref horizontal,playerColliderRadius);  
        PlayerShoot();  
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    int PlayerHorizontalInput()
    {
        if(Input.GetKey(KeyCode.A))
        {
            return -1;
        }

        if(Input.GetKey(KeyCode.D))
        {
            return 1;
        }
        return 0;
    }

    void PlayerShoot()
    {
        // TODO timer patterni 
        if(Input.GetKeyDown(KeyCode.Space) && Time.time - shootTimer >= shootingRate)
        {
            shootTimer = Time.time;
            GameObject playerLaser = Instantiate(this.playerLaser,shootingPoint.position,Quaternion.Euler(90,0,0));
            //Direction allows to have only one laser script for two different users of the laser (invaders and the player)
            playerLaser.GetComponent<SpaceInvaderLaser>().direction = 1;
        }
    }
    public void PlayerDamaged()
    {
        health--;
    }
    void Move(ref int horizontalInput, float playerRadius)
    {
        // TODO inputu degil pozisyonu clample
        if(transform.position.x - playerRadius < minXLimit)
        {
            horizontalInput = Mathf.Clamp(horizontalInput,0,1);
        }
        if(transform.position.x + playerRadius > maxXLimit)
        {
            horizontalInput = Mathf.Clamp(horizontalInput,-1,0);
        }

        transform.position += Vector3.right * playerSpeed * horizontal * Time.deltaTime;
    }

    /*private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position,playerColliderRadius);    
    }*/
}
