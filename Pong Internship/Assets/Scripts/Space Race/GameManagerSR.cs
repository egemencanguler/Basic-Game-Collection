using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerSR : MonoBehaviour
{
    public GameObject deadlyBall;
    public float frequency;
    public Vector2[] spawnSections;
    public float gameLength = 45f;
    public PlayerSR[] playerScripts;
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
            PlayerScore();
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
            Instantiate(deadlyBall,new Vector3(spawnSections[0].x,Random.Range(spawnSections[1].y,spawnSections[0].y)),Quaternion.identity);
            Instantiate(deadlyBall,new Vector3(-spawnSections[0].x,Random.Range(spawnSections[1].y,spawnSections[0].y)),Quaternion.identity);
            lastTimeOfSpawn = frequency;
        }
    }

    void PlayerScore()
    {
        Vector3 playerOnePosition = playerScripts[0].transform.position;
        Vector3 playerTwoPosition = playerScripts[1].transform.position;

        //Score implementation
        if(playerScripts[0].transform.position.y > spawnSections[0].y)
        {
            playerOneScore++;
            playerScoreTexts[0].text ="" + playerOneScore;
            playerScripts[0].PlayerReset();
        }

        if(playerScripts[1].transform.position.y > spawnSections[0].y)
        {
            playerTwoScore++;                        
            playerScoreTexts[1].text ="" + playerTwoScore;
            playerScripts[1].PlayerReset();
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
