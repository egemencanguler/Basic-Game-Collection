using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerSpaceRace : MonoBehaviour
{
    public GameObject deadlyBall;
    public float frequency;
    public Vector2[] spawnSections;
    public float gameLength = 45f;
    public Text[] playerScoreTexts;

    private float lastTimeOfSpawn;
    private int playerOneScore = 0;
    private int playerTwoScore = 0;

    private void Start() 
    {
        lastTimeOfSpawn = frequency;    
    }
    void Update()
    {
        //The time control states the games condition.
        if(Time.time <= gameLength)
        {
            SpawnDeadlyBalls(frequency);
            playerScoreTexts[3].text = "" + ((int)Time.time);
        }
        else
        {
            playerScoreTexts[3].text = "";
            WinCondition();
        }

    }

    void SpawnDeadlyBalls(float freq)
    {
        //TIMER
        //Spawn in different points of an interval with a frequency
        lastTimeOfSpawn -= Time.deltaTime;
        if(lastTimeOfSpawn <= 0f)
        {
            Instantiate(deadlyBall,new Vector3(spawnSections[0].x,0f,Random.Range(spawnSections[1].y,spawnSections[0].y)),Quaternion.identity);
            Instantiate(deadlyBall,new Vector3(-spawnSections[0].x,0f,Random.Range(spawnSections[1].y,spawnSections[0].y)),Quaternion.identity);
            lastTimeOfSpawn = frequency;
        }
    }

    public void PlayerScore(GameObject player)
    {
        if(player.CompareTag("Player 1"))
        {
            playerOneScore++;
            playerScoreTexts[0].text = "" + playerOneScore;
        }

        if(player.CompareTag("Player 2"))
        {
            playerTwoScore++;
            playerScoreTexts[1].text = "" + playerTwoScore;
        }


    }

    void WinCondition()
    {
        if(playerOneScore > playerTwoScore)
        {
            playerScoreTexts[2].text = "Player 1 Won!!!";
        }
        else if (playerOneScore < playerTwoScore)
        {
            playerScoreTexts[2].alignment = TextAnchor.UpperRight;
            playerScoreTexts[2].text = "Player 2 Won!!!";
        }
        else
        {
            playerScoreTexts[2].text = "It's a Draw!!!";
        }
    }
}
