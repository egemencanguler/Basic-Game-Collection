using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanMoveObjectCommand : ICommand
{
    Vector3 objectNextPos;
    float speed;
    Vector3 direction;
    Transform objectTransform;
    Vector3 prevPos;
    GameObject thisObject; 
    List<SokobanObjectManager> sokobanObjects;
    public SokobanMoveObjectCommand(Vector3 objectNextPos, float speed, Vector3 direction, Transform objectTransform) 
    {
        this.objectNextPos = objectNextPos;
        this.objectTransform = objectTransform;
        this.direction = direction;
        this.speed = speed;
    }

    public SokobanMoveObjectCommand(Vector3 objectNextPos, Transform objectTransform, Vector3 prevPos)
    {
        this.objectNextPos = objectNextPos;
        this.objectTransform = objectTransform;
        this.prevPos = prevPos;
    }

    public SokobanMoveObjectCommand(Transform objectTransform)
    {
        this.objectTransform = objectTransform;
    }    
    public SokobanMoveObjectCommand(GameObject thisObject, List<SokobanObjectManager> sokobanObjectManagers)
    {
        this.thisObject = thisObject;
        this.sokobanObjects = sokobanObjectManagers;
    }

    public SokobanMoveObjectCommand(GameObject thisObject)
    {
        this.thisObject = thisObject;
    }

    public void Execute()
    {
        ObjectMoveSokoban.MoveObjectToNext(objectNextPos,speed,direction,objectTransform);
    }

    public void Undo() 
    {
        ObjectMoveSokoban.MoveObjectToPrev(prevPos,objectTransform);
    }

    public void Destroy() 
    {
        ObjectMoveSokoban.DestroyObjectInScene(thisObject,sokobanObjects);
    }

    public void Instantiate()
    {
    }
}
