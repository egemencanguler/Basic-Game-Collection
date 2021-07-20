using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SokobanMovementManager : MonoBehaviour
{
    public SokobanObjectList objectParent;
    public float gridSquareSize = 0.5f;
    public float characterSpeed = 0.01f;
    public float swipeSensitivity = 1f;
    public Animator playerAnimator;
    public AnimationHandler playerAnimHandler;
    public float rewindSpeed = 3f;
    private Vector3 direction = Vector3.zero;
    private Vector3 initialPosition;
    public Vector3 targetMovePos;
    private Vector3 initialPositionMouse,finalPositionMouse;
    public List<Vector3> playerPrevGrid;
    private int prevIndex = 0;
    private Vector3 undoPos;
    public bool playerDead = false;

    private void Awake()
    {
        playerPrevGrid = new List<Vector3>();
    }
    private void Start()
    {
        playerPrevGrid.Add(transform.position);
        prevIndex = playerPrevGrid.Count - 1;
        targetMovePos = transform.position;
        initialPosition = transform.position;
    }
    private void Update()
    {

        if (playerPrevGrid.Count == 0) 
        {
            targetMovePos = initialPosition;
        }
        else 
        {
            if (prevIndex != playerPrevGrid.Count - 1) 
            {
                prevIndex = playerPrevGrid.Count - 1;
                targetMovePos = undoPos;
            }
        }
        //Only get input when the player is not moving
        if (transform.position == targetMovePos && !playerDead) 
        {
            PlayerInput();
            if (transform.position != targetMovePos) 
            {
                ObjectCollision();
                playerPrevGrid.Add(transform.position);
                prevIndex = playerPrevGrid.Count - 1;
                ICommand commandPrev = new SokobanMovePlayerCommand(playerPrevGrid[playerPrevGrid.Count - 1],transform);
                CommandInvoker.SavePlayerCommand(commandPrev);
                ICommand command = new SokobanMovePlayerCommand(targetMovePos,transform);
                CommandInvoker.AddCommand(command);
                ObjectCollision();
                for (int a = 0; a < objectParent.sokobanObjectManagers.Count; a++) 
                {
                    objectParent.sokobanObjectManagers[a].MoveObjects();
                }
            }
        }
        ObjectCollision();
    }

    void PlayerInput()
    {
        //Mouse Swipe Input
        //Initial position the player clicked
        if (Input.GetMouseButtonDown(0)) 
        {
            initialPositionMouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        //The position where the mouse is at that frame 
        if (Input.GetMouseButton(0)) 
        {
            finalPositionMouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if ((initialPositionMouse - finalPositionMouse).magnitude >= swipeSensitivity/10) 
        {
            direction = (finalPositionMouse - initialPositionMouse).normalized;
            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
            {
                direction = new Vector3(direction.x,0f,0f).normalized;
            }
            else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) 
            {
                direction = new Vector3(0f, 0f, direction.y).normalized;
            }
            initialPositionMouse = finalPositionMouse;
        }

        targetMovePos += direction * gridSquareSize;
        direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerPrevGrid.Clear();
            prevIndex = 0;
            for (int a = 0; a < objectParent.sokobanObjectManagers.Count; a++) 
            {
                SokobanObjectManager sokobanObject = objectParent.sokobanObjectManagers[a];
                if (objectParent.sokobanObjectManagers[a].prevPos.Count > 0 && sokobanObject.gameObject.activeSelf) 
                {
                    sokobanObject.objectNextPos = sokobanObject.initialPosition;
                    sokobanObject.prevPos.Clear();
                    sokobanObject.turnCounter = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && playerPrevGrid.Count > 1) 
        {
            undoPos = playerPrevGrid[playerPrevGrid.Count - 1];
            playerPrevGrid.RemoveAt(playerPrevGrid.Count - 1);
            for (int a = 0; a < objectParent.sokobanObjectManagers.Count; a++)
            {
                SokobanObjectManager sokobanObject = objectParent.sokobanObjectManagers[a];
                if (objectParent.sokobanObjectManagers[a].prevPos.Count > 0 && sokobanObject.gameObject.activeSelf)
                {
                    sokobanObject.objectNextPos = sokobanObject.prevPos[sokobanObject.prevPos.Count - 1];
                    sokobanObject.prevPos.RemoveAt(sokobanObject.prevPos.Count - 1);
                    sokobanObject.turnCounter--;
                }
            }
        }
    }

    void ObjectCollision() 
    {
        Vector3 playerNextPos = targetMovePos;
        Vector3 objectTargetTravelDis = ((targetMovePos - transform.position).normalized * gridSquareSize);
        List<SokobanObjectManager> objectsInScene = objectParent.sokobanObjectManagers;
        //Check every object on the scene (walls, tragets and boxes)
        for (int a = 0; a < objectsInScene.Count;a++) 
        {
            //If the player tries to collide with an object
            if (playerNextPos == objectsInScene[a].transform.position)
            {
                //Only Walls and Boxes can collide with the player
                switch (objectsInScene[a].objectType)
                {
                    case ObjectType.BOX:
                        //Scan every object and find if the next position of the box collides with another box or wall (if the player tries to smash tha box onto the wall)
                        Vector3 boxNextPos = playerNextPos + objectTargetTravelDis;
                        foreach (SokobanObjectManager otherObject in objectsInScene)
                        {
                            if (boxNextPos == otherObject.transform.position && objectsInScene[a] != otherObject && otherObject.objectType != ObjectType.TARGET)
                            {
                               objectTargetTravelDis = Vector3.zero;
                               targetMovePos = transform.position;
                            }

                            if (otherObject.objectNextPos == boxNextPos && otherObject.objectType == ObjectType.SAW)
                            {
                                objectTargetTravelDis = Vector3.zero;
                                targetMovePos = transform.position;
                            }
                        }

                        if(targetMovePos != transform.position) 
                        {
                            objectsInScene[a].speed = characterSpeed;
                            objectsInScene[a].objectNextPos = boxNextPos;
                            objectsInScene[a].direction = (boxNextPos - playerNextPos).normalized;
                            playerAnimator.SetBool("isRunning",false);
                            playerAnimator.SetBool("isPushing",true);
                        }
                        break;
                    case ObjectType.WALL:
                        //The player cannot go trough walls
                        CommandInvoker.RemoveLastSavedCommand();
                        playerPrevGrid.RemoveAt(playerPrevGrid.Count - 1);
                        targetMovePos = objectsInScene[a].transform.position + (transform.position - objectsInScene[a].transform.position).normalized * gridSquareSize;
                        direction = Vector3.zero;
                        break;
                }
            }
            
            //Initiate death (ragdoll)
            if (objectsInScene[a].objectType == ObjectType.SAW || objectsInScene[a].objectType == ObjectType.PROJECTILE) 
            {
                if (transform.position == objectsInScene[a].transform.position) 
                {
                    Debug.Log("Before Func");
                    playerAnimHandler.PointOfIntersection(transform.position);
                }
            }
        }
    }
}
