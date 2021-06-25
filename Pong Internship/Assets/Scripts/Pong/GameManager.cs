using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballObject;
    public Text[] scoreText;
    public int scoreToWin = 10;
    public int side = -1;

    public bool isBallTest = false;
    public bool isEndlessMode = false;
    public Vector3 prototypeSpawnPoint = new Vector3(0f,0f,0f);

    private int goalCountPlayerOne = 0;
    private int goalCountPlayerTwo = 0;
    private Transform ballTransform;

    private void Awake() 
    {
        if(!isBallTest)
            Instantiate(ballObject,transform.position,Quaternion.identity);
        else
            Instantiate(ballObject,prototypeSpawnPoint,Quaternion.identity);
        ballTransform = ballObject.transform;
    }

    void Update()
    {
        scoreText[0].text = "" + goalCountPlayerOne;
        scoreText[1].text = "" + goalCountPlayerTwo; 
    }

    public void Goal(int side)
    {
        Vector3 ballPosition = ballTransform.position;

        //Check the side that scored, see if the game score limit is reached and display the text or spawn a new ball. 
        if(side > 0)
        {
            goalCountPlayerOne++;
        }
        else
        {
            goalCountPlayerTwo++;
        }

        if(!isEndlessMode)
        {
            if(goalCountPlayerOne < scoreToWin && goalCountPlayerTwo < scoreToWin)
                        if(!isBallTest)
                            Instantiate(ballObject,transform.position,Quaternion.identity);
                        else
                            Instantiate(ballObject,prototypeSpawnPoint,Quaternion.identity);
            else
            {
                if(goalCountPlayerOne > goalCountPlayerTwo)
                {
                    scoreText[2].text = "Player 1   Won!!!";
                }
                else
                {
                    scoreText[2].text = "Player 2   Won!!!";
                }
            }
        }
    }
}
