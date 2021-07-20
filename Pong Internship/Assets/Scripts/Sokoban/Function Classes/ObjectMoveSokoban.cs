using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectMoveSokoban
{
    public static void MoveObjectToNext(Vector3 objectNextPos, float speed, Vector3 direction, Transform objectTransform)
    {
        objectTransform.position = objectNextPos;
    }

    public static void MoveObjectToPrev(Vector3 prevPos, Transform boxTransform) 
    {
        if (boxTransform == null) 
        {
            return;
        }

        boxTransform.position = prevPos;
    }

    public static void DestroyObjectInScene(GameObject thisObject, List<SokobanObjectManager> sokobanObjectManagers) 
    {
        if (thisObject == null) 
        {
            return;
        }
        sokobanObjectManagers.Remove(thisObject.GetComponent<SokobanObjectManager>());
        GameObject.Destroy(thisObject);
    }

    public static void InstantiateObjectOnPosition(GameObject projectileObject, Vector3 position) 
    {
        projectileObject.SetActive(true);
        projectileObject.transform.position = position;
    }
}
