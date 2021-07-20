using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public SokobanMovementManager playerEntityManager;
    public SokobanObjectManager objectEntityManager;
    public Vector3 modelPositionOffset;
    public float offSet = 0.1f;
    public float speed = 5f;
    public const string runningAnim = "isRunning";
    public const string pushingAnim = "isPushing";
    public const string idleAnim = "Stop Distance";
    public Animator animator;
    public Vector3 lastPosition;
    public float knocknackForce = 3f;
    public GameObject prefabPlayer;
    private List<AnimationHandler> animationHandlers = new List<AnimationHandler>();
    private bool crashAlert = false;
    private Vector3 crashPoint;
    //public bool isDead;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigids;
    private void Awake() 
    {
        //Disable Ragdoll
        if(playerEntityManager != null)
        {
            ragdollColliders = transform.GetComponentsInChildren<Collider>();
            ragdollRigids = transform.GetComponentsInChildren<Rigidbody>();

            foreach(Collider collider in ragdollColliders)
            {
                collider.enabled = false;
            }

            foreach(Rigidbody rigids in ragdollRigids)
            {
                rigids.isKinematic = true;
            }
        }

        if (transform.parent == null) 
        {
            transform.SetParent(GameObject.Find("Models").transform);
        }
    }

    private void Update()
    {
        if (playerEntityManager != null) 
        {
            //Move the model towards the grid position of the player slowly
            Vector3 playerGridPosition = playerEntityManager.transform.position + modelPositionOffset;
            if (playerGridPosition != transform.position) 
            {
                transform.LookAt(playerGridPosition);
                MoveToDestination(playerGridPosition);
                if (crashAlert) 
                {
                    PlayerRagdDollEnabler(new Vector3(0f, 0f, 0f));
                }
            }
        }
        else if (objectEntityManager != null && objectEntityManager.gameObject.activeSelf) 
        {
            //Move the model towards the grid position of the object slowly
            Vector3 objectGridPos = objectEntityManager.transform.position;
            if (objectGridPos != transform.position) 
            {
                MoveToDestination(objectGridPos);
            }
            lastPosition = objectGridPos;
        }
        else 
        {
            if (transform.position == lastPosition) 
            {
                Destroy(gameObject);
            }
            else 
            {
                MoveToDestination(lastPosition);
            }

        }

        if (Input.GetKeyDown(KeyCode.R) && playerEntityManager != null) 
        {
           AnimationHandler animationHandler = Instantiate(prefabPlayer,playerEntityManager.transform.position,Quaternion.identity).GetComponent<AnimationHandler>();

        }
    }

    public void PointOfIntersection(Vector3 pointOfIntersection) 
    {
        crashAlert = true;
        crashPoint = pointOfIntersection + modelPositionOffset;
    }

    public void PlayerRagdDollEnabler(Vector3 force)
    {
 
        //If death occured than initiate the ragdoll
        if (Vector3.Distance(transform.position,crashPoint) <= 0.1f) 
        {
            Debug.Log("Girdim");
            foreach(Collider collider in ragdollColliders)
            {
                //Belki daha optimize olması için erken bir break sistemi yazılabilr.
                collider.enabled = true;
            }

            foreach(Rigidbody rigidbody in ragdollRigids)
            {
                if (rigidbody.isKinematic) 
                {
                    rigidbody.isKinematic = false;
                    rigidbody.AddForce(force.normalized *knocknackForce);
                }
            }

            animator.enabled = false;
            crashAlert = false;
        }
    }

    void MoveToDestination(Vector3 destination) 
    {
        //Move and execute the animations for the models
        Vector3 direction = (destination - transform.position).normalized;
        transform.position += direction * Time.deltaTime * speed;
        if (Vector3.Distance(transform.position,destination) <= offSet) 
        {
            transform.position = destination;
        }

        if (animator != null) 
        {
            animator.SetFloat(idleAnim,Vector3.Distance(transform.position, destination));
            if (Vector3.Distance(transform.position, destination) > offSet) 
            {
                animator.SetBool(runningAnim,true);
            }
            else 
            {
                animator.SetBool(runningAnim, false);
                animator.SetBool(pushingAnim, false);
            }
        }
    }
}
