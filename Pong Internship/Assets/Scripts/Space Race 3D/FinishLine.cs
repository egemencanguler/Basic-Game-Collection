using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public PlayerMechanicsSR playerScript;
    public GameManagerSpaceRace gameManager;

    private void Update() 
    {
        if(playerScript.transform.position.z >= transform.position.z)
        {
            gameManager.PlayerScore(playerScript.gameObject);
            playerScript.PlayerReset();
        }  
    }

}
