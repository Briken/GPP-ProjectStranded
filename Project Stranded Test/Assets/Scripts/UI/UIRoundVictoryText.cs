using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundVictoryText : MonoBehaviour {

    public GameObject roundStateTextBox;
    public GameObject newRoundStateTextBox;
    public bool playerWon;

    public GameObject scoreDataObject;

    public float timeUntilNewRound;
    bool shouldActivateNewRoundTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (shouldActivateNewRoundTimer)
        {
            if (timeUntilNewRound >= 0.0f)
            {
                timeUntilNewRound -= Time.deltaTime;

                if (scoreDataObject.GetComponent<ScoreCount>().roundCount != 4)
                {
                    newRoundStateTextBox.GetComponent<Text>().text = "ROUND " + (scoreDataObject.GetComponent<ScoreCount>().roundCount + 1).ToString() + " STARTING IN " + timeUntilNewRound.ToString("0.0") + "s";
                }
                else
                {
                    newRoundStateTextBox.GetComponent<Text>().text = "REVEALING MATCH RESULTS IN " + timeUntilNewRound.ToString("0.0") + "s";
                }
                
            }
            else
            {
                shouldActivateNewRoundTimer = false;
            }
        }
		
	}

    public void DisplayRoundOverScreen(float newRoundTime, bool hasPlayerWon)
    {
        timeUntilNewRound = newRoundTime;
        playerWon = hasPlayerWon;
        shouldActivateNewRoundTimer = true;

        if (playerWon)
        {
            roundStateTextBox.GetComponent<Text>().text = "YOU WON";
        }
        else
        {
            roundStateTextBox.GetComponent<Text>().text = "YOU LOST";
        }
    }
}
