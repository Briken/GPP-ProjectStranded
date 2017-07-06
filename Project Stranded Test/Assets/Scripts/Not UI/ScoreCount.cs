using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCount : MonoBehaviour {
    public int[] playerScores;
    public int[] winCounts;
    public int roundWinner;

    public int player1Score;
    public int player2Score;
    public int player3Score;
    public int player4Score;
    public int player5Score;
    public bool p1Win;
    public bool p2Win;
    public bool p3Win;
    public bool p4Win;
    public bool p5Win;
    public int roundCount;
    bool counted = false;

    // public GameObject lossScreen;
    // public GameObject winScreen;

    public GameObject roundOverScreen;
    public GameObject matchOverScreen;

    // Use this for initialization
    void Start ()
    {
        playerScores = new int[5];
        winCounts = new int[5];

        /*
        if (SceneManager.GetActiveScene().name == "MainScene-Recovered")
        {
            winScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().winScreen;
            lossScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().lossScreen;
        }
        */
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecordScores(int playerNum, int score)
    {
        for(int x = 0; x < playerScores.Length; x++ )
        {
            if (x == playerNum - 1)
            {
                playerScores[x] += score;
            }
        }   
        if (counted == false)
        {
            counted = true;
            roundCount++;
        }

        int currentLead = 0;
        for (int y = 0; y < playerScores.Length; y++)
        {
            if (currentLead < playerScores[y])
            {
                currentLead = playerScores[y];
                roundWinner = y + 1;
            }
        }

        

    }
    public void LoadNextRound()
    {

        StartCoroutine(LoadLevel());

    }
    IEnumerator LoadLevel()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetComponent<MovementScript>().playerNum == roundWinner && n.GetPhotonView().isMine)
            {
                roundOverScreen.SetActive(true);
                roundOverScreen.GetComponent<UIRoundVictoryText>().DisplayRoundOverScreen(7.0f, true);
            }
            else
            {
                roundOverScreen.SetActive(true);
                roundOverScreen.GetComponent<UIRoundVictoryText>().DisplayRoundOverScreen(7.0f, false);
            }
        }
        yield return new WaitForSeconds(7);
        if (roundCount == 4)
        {
            matchOverScreen.SetActive(true);
        }
        else
        {
            EventManager.ResetObjects();
            // winScreen.SetActive(false);
            // lossScreen.SetActive(false);

            counted = false;
        }

        roundOverScreen.SetActive(false);

    }
}
