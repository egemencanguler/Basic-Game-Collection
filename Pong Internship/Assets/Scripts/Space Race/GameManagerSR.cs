using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerSR : MonoBehaviour
{
    [SerializeField]
    private GameObject deadlyBall;

    [SerializeField]
    private float frequency;

    [SerializeField]
    private float endOfGameTime = 300;

    [SerializeField]
    private Vector2[] spawnSections;
    
    [SerializeField]
    private float gameLength = 45f;
    [SerializeField]
    private PlayerSR[] playerScripts;
    [SerializeField]
    private Text[] playerScoreTexts;

    private float lastTimeOfSpawn = 0;

    private Vector2 playerScore = new Vector2(0,0);

    private void Awake() 
    {
        playerScripts[0] = transform.Find("Player 1").GetComponent<PlayerSR>();
        playerScripts[1] = transform.Find("Player 2").GetComponent<PlayerSR>();  

        playerScoreTexts[0] = transform.Find("Canvas").transform.Find("Player 1 Score").GetComponent<Text>();
        playerScoreTexts[1] = transform.Find("Canvas").transform.Find("Player 2 Score").GetComponent<Text>();
        playerScoreTexts[2] = transform.Find("Canvas").transform.Find("Final").GetComponent<Text>();  
        playerScoreTexts[3] = transform.Find("Canvas").transform.Find("Timer").GetComponent<Text>(); 
 
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
        //Spawn in different points of an interval with a frequency
        if(Time.time - lastTimeOfSpawn >= frequency)
        {
            Instantiate(deadlyBall,new Vector3(spawnSections[0].x,Random.Range(spawnSections[1].y,spawnSections[0].y)),Quaternion.identity);
            Instantiate(deadlyBall,new Vector3(-spawnSections[0].x,Random.Range(spawnSections[1].y,spawnSections[0].y)),Quaternion.identity);
            lastTimeOfSpawn = Time.time;
        }
    }

    void PlayerScore()
    {
        //Score implementation
        if(playerScripts[0].transform.position.y > spawnSections[0].y)
        {
            playerScore.x++;
            playerScoreTexts[0].text ="" + playerScore.x;
            playerScripts[0].PlayerReset();
        }

        if(playerScripts[1].transform.position.y > spawnSections[0].y)
        {
            playerScore.y++;                        
            playerScoreTexts[1].text ="" + playerScore.y;
            playerScripts[1].PlayerReset();
        }
    }

    void WinCondition()
    {
        if(playerScore.x > playerScore.y)
        {
            playerScoreTexts[2].text = "Player 1 Won!!!";
        }
        else if (playerScore.x < playerScore.y)
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
