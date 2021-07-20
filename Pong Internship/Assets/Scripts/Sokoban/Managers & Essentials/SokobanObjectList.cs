using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanObjectList : MonoBehaviour
{
    public List<SokobanObjectManager> sokobanObjectManagers = new List<SokobanObjectManager>();
    public GameObject projectileObject;

    public Transform playerTransform;
    private void Awake()
    {
        foreach (SokobanObjectManager sokobanObjectManager in transform.GetComponentsInChildren<SokobanObjectManager>()) 
        {
            sokobanObjectManagers.Add(sokobanObjectManager);
        }
    }
}
