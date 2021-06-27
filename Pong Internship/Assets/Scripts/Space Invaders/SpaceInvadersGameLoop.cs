using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceInvadersGameLoop : MonoBehaviour
{
    public SpaceInvaderPlayer player;
    public GameObject invaderParent;
    public Text leftLives;
    public Text endGame;
    private void Update() 
    {
        if(invaderParent.transform.position.z < 0f)
        {
            player.health = 0;
        }
        if(player.health <= 0)
        {
            endGame.text = "!!!You Lost!!!";
        }
        else
        {
            leftLives.text = "Life: " + player.health;
        }

        if(!GameObject.FindObjectOfType<SpaceInvader>())
        {
            endGame.text = "!!!You Won!!!";
        }    
    }
}
