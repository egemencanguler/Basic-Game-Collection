using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    BOX,
    WALL,
    TARGET,
    SAW,
    TURRET,
    PROJECTILE
}
public class SokobanObjectManager : MonoBehaviour
{
    [Header("Object")]
    public ObjectType objectType;
    public GameObject objectParent;
    public SokobanObjectList objectsInScene;
    public GameObject animationModel;
    [Header("Moveable Objects")]
    public float gridSize = 1;
    public float speed = 10f;
    public Vector3 objectNextPos;
    public Vector3 direction = Vector3.zero;
    public List<Vector3> prevPos = new List<Vector3>();
    public Vector3 initialPosition = Vector3.zero;
    [Header("Box")]
    public Material boxMaterialInitial;
    public Material boxMaterialInserted;
    public MeshRenderer meshRendererAnim;
    public SokobanObjectManager targetOfThisBox;
    [Header("Turret")]
    public GameObject turretProjectile;
    public int fireRate = 5;
    public Vector3 fireDirection = Vector3.zero;
    public int turnCounter = 0;
    [Header("Projectile")]
    public GameObject animObject;
    public GameObject currentAnimModel;
    private Vector3 projectileSpawnPos;
    private Transform initialTransformParent;
    private void Awake()
    {
        if (objectType == ObjectType.SAW) 
        {
            direction = transform.right.normalized;
        }
        objectParent = GameObject.Find("Object Parent");
        objectsInScene = objectParent.GetComponent<SokobanObjectList>();
    }

    private void Start()
    {
        initialTransformParent = transform.parent;
        if (objectType != ObjectType.PROJECTILE) 
        {
            projectileSpawnPos = transform.position + fireDirection * gridSize;
            if(objectType == ObjectType.BOX)
                prevPos.Add(transform.position);
            objectNextPos = transform.position + direction * gridSize;
            if (objectType == ObjectType.WALL) 
            {
                objectNextPos = transform.position;
            }
        }
        else
        {
            //Initializa the projectile with the information
            direction = transform.parent.GetComponent<SokobanObjectManager>().fireDirection;
            transform.SetParent(transform.parent.transform.parent);
            objectNextPos = transform.position;
            currentAnimModel = Instantiate(animObject,transform.position,Quaternion.identity);
            currentAnimModel.GetComponent<AnimationHandler>().objectEntityManager = this;
        }
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (currentAnimModel == null && gameObject.activeSelf && objectType == ObjectType.PROJECTILE) 
        {
            //For the model that will follow the projectile
            currentAnimModel = Instantiate(animObject, transform.position, Quaternion.identity);
            currentAnimModel.GetComponent<AnimationHandler>().objectEntityManager = this;
        }

        //Check for box target position
        if (objectType == ObjectType.BOX) 
        {
            BoxTarget();
            if (targetOfThisBox != null)
            {
                if (transform.position != targetOfThisBox.transform.position)
                {
                    meshRendererAnim.material = boxMaterialInitial;
                    targetOfThisBox = new SokobanObjectManager();
                }
            }
        }
    }

    public void MoveObjects() 
    {
        //Commands for movement and fire are given
        if (objectParent == null) 
        {
            objectParent = GameObject.Find("Object Parent").gameObject;
        }
        objectsInScene = objectParent.GetComponent<SokobanObjectList>();
        prevPos.Add(transform.position);
        switch (objectType) 
        {
            case ObjectType.BOX:
                    ICommand commandPrevBox = new SokobanMoveObjectCommand(objectNextPos,transform, prevPos[prevPos.Count - 1]);
                    CommandInvoker.SaveBoxCommand(commandPrevBox);
                    ICommand commandBox = new SokobanMoveObjectCommand(objectNextPos, speed, direction, transform);
                    commandBox.Execute();
                break;
            case ObjectType.SAW:
                     objectNextPos = objectNextPos = transform.position + direction * gridSize;
                     if (HazardObjectTargetFinder())
                     {
                        ICommand commandPrevSaw= new SokobanMoveObjectCommand(objectNextPos, transform, prevPos[prevPos.Count - 1]);
                        CommandInvoker.SaveHazardCommand(commandPrevSaw);
                        ICommand commandSaw = new SokobanMoveObjectCommand(objectNextPos, speed, direction, transform);
                        commandSaw.Execute();

                        if (transform.position == objectNextPos)
                        {
                            HazardObjectTargetFinder();
                            objectNextPos = transform.position + direction * gridSize;
                        }
                     }
                break;
            case ObjectType.PROJECTILE:
                    objectNextPos = transform.position + direction * gridSize;
                    ICommand commandHazard = new SokobanMoveObjectCommand(objectNextPos, speed, direction, transform);
                    commandHazard.Execute();
                    ICommand commandPrevProjectile = new SokobanMoveObjectCommand(objectNextPos, transform, prevPos[prevPos.Count - 1]);
                    CommandInvoker.SaveHazardCommand(commandPrevProjectile);
                if (transform.position == objectNextPos)
                {
                    HazardObjectTargetFinder();
                    objectNextPos = transform.position + direction * gridSize;
                }
                break;
            case ObjectType.TURRET:
                if (objectType == ObjectType.TURRET)
                {
                    turnCounter++;
                    if (turnCounter % fireRate == 0)
                    {
                       GameObject newObject = Instantiate(turretProjectile, projectileSpawnPos, Quaternion.identity);
                       newObject.transform.SetParent(transform);
                       objectsInScene.sokobanObjectManagers.Add(newObject.GetComponent<SokobanObjectManager>());
                       Destroy(currentAnimModel);
                       ICommand commandSavedProjectileSpawn = new SokobanMoveObjectCommand(newObject,objectsInScene.sokobanObjectManagers);
                       CommandInvoker.SaveDestroyCommand(commandSavedProjectileSpawn);
                    }

                     }
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.R) && prevPos.Count > 0)
        {
            prevPos.RemoveAt(prevPos.Count - 1);
        }
    }
    //For object collision made by the saw or projectile. Saw changes direction, projectile disables its activity
    bool HazardObjectTargetFinder() 
    {
        Vector3 hazardNextPos = transform.position + direction * gridSize;
        Vector3 playerTransform = objectsInScene.playerTransform.position;
        foreach(SokobanObjectManager objectManager in objectsInScene.sokobanObjectManagers) 
        {
            if (hazardNextPos == objectManager.transform.position && objectType == ObjectType.SAW) 
            {
                direction = -direction;
                objectNextPos = transform.position + direction * gridSize;
                return false;
            }
            else if (objectType == ObjectType.PROJECTILE) 
            {
                if (transform.position == objectManager.transform.position && this != objectManager) 
                {
                    Vector3 spawnPos = transform.position - direction * gridSize;
                    //Save the command for reactivation when Undo is done
                    ICommand commandSaveDestroy = new SokobanMoveObjectCommand(gameObject);
                    CommandInvoker.SaveSpawnCommand(commandSaveDestroy);
                    gameObject.SetActive(false);
                    return false;
                }
            }
        }
        return true;
    }

    void BoxTarget()
    { 
        //Travel through the list of objects in the scene and check if the box is at the same posisiton with the target. 
        foreach(SokobanObjectManager objects in objectsInScene.sokobanObjectManagers) 
        {
            if (objects.objectType == ObjectType.TARGET && objects.transform.position == transform.position) 
            {
                if (meshRendererAnim.transform.position == transform.position) 
                {
                    meshRendererAnim.material = boxMaterialInserted;
                    targetOfThisBox = objects;
                }
                break;
            }

        }
    }
}
