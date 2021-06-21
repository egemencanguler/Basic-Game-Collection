using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private GameObject ballObject;

    [SerializeField]
    private Text[] scoreText;

    [SerializeField]
    private int scoreToWin = 10;
    #endregion

    #region Variables
    private Vector2 goalCount = new Vector2(0,0);

    private int side = -1;
    #endregion

    #region Awake - Update
    private void Awake() 
    {
        Instantiate(ballObject);

        scoreText[0] = transform.Find("Canvas").transform.Find("Player 1 Score").GetComponent<Text>();
        scoreText[1] = transform.Find("Canvas").transform.Find("Player 2 Score").GetComponent<Text>();
        scoreText[2] = transform.Find("Canvas").transform.Find("End Game").GetComponent<Text>();
    }
    void Update()
    {
        scoreText[0].text = "" + goalCount.x;
        scoreText[1].text = "" + goalCount.y; 
    }
    #endregion

    public void Goal(int side)
    {
        //Check the side that scored, see if the game score limit is reached and display the text or spawn a new ball. 
        if(side == -1)
        {
            goalCount.x++;
            this.side = 1;
        }
        else
        {
            goalCount.y++;
            this.side = -1;
        }

        if(goalCount.x < scoreToWin && goalCount.y < scoreToWin)
            Instantiate(ballObject);
        else
        {
            if(goalCount.x > goalCount.y)
            {
                scoreText[2].gameObject.SetActive(true);
                scoreText[2].text = "Player 1   Won!!!";
            }
            else
            {
                scoreText[2].gameObject.SetActive(true);
                scoreText[2].text = "Player 2   Won!!!";
            }
        }
    }

    public int GetSide()
    {
        return side;
    }
}
