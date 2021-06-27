using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvader : MonoBehaviour
{
    public float invaderColliderRadius = 1;
    public float invaderColliderRadiusOffSet = 1.25f;
    public bool isDestroyed = false;
    public bool canShoot = false;
    public GameObject invaderLaser;
    public float shootingRate = 1f;
    public float shootingChance = 25f;
    public Transform shootingPosition;

    private float timer = 0f;
    
    private void Update() 
    {
        if(isDestroyed)
        {
            DestroyInvader();
        }

        if(canShoot)
        {
            InvaderShoot();
        }    
    }

    /*private void OnDrawGizmos() {
        Vector3 sphereColliderCenter = transform.position + Vector3.forward * invaderColliderRadiusOffSet;
        Gizmos.DrawWireSphere(sphereColliderCenter,invaderColliderRadius);
    }*/

    void DestroyInvader()
    {
        Destroy(gameObject);
    }

    void InvaderShoot()
    {
        //The timer adjusts the times when the invaders has a chance to shoot
        if( Time.time - timer >= shootingRate)
        {
            float random = Random.Range(0f,100f);
            if(random <= shootingChance)
            {
                SpaceInvaderLaser invaderLaser = Instantiate(this.invaderLaser,shootingPosition.position,Quaternion.Euler(90,0,0)).GetComponent<SpaceInvaderLaser>();
                invaderLaser.direction = -1;
            }
            timer = Time.time;
        }
    }

}
