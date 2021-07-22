using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvader : MonoBehaviour
{
    public float invaderColliderRadius = 1;
    public float invaderColliderRadiusOffSet = 1.25f; // TODO kullanilmayan degiskeni kaldir
    public bool isDestroyed = false;
    public bool canShoot = false;
    public GameObject invaderLaser;
    public float shootingRate = 1f;
    public float shootingChance = 25f;
    public Transform shootingPosition;

    private float timer = 0f;
    
    private void Update() 
    {
        // TODO neden direk Destroy(invaderGameObject) yapmak yerine isDestroyed -> DestroyInvader -> Destroy(gameObject)
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
        // TODO Time.time - timer guzel bir pattern degil - burdada sira bizim elemana geldigi an direk ates edicek timer 0 oldugu icin
        //The timer adjusts the times when the invaders has a chance to shoot
        if( Time.time - timer >= shootingRate)
        {
            float random = Random.Range(0f,100f);
            // TODO Random.value < 0.25f ([Range(0,1)] shootingChange ) kullanilabilirdi 0,1 arasi seyler herzaman daha guzel seyler
            if(random <= shootingChance)
            {
                // TODO field la ayni isme sahip visual studio gostermiyormu
                SpaceInvaderLaser invaderLaser = Instantiate(this.invaderLaser,shootingPosition.position,
                    Quaternion.Euler(90,0,0)).GetComponent<SpaceInvaderLaser>();
                invaderLaser.direction = -1;
            }
            timer = Time.time;
        }
    }

}
