using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;

public class ScoreCount : Photon.PunBehaviour
{
    public int[] playerCurrentScores;
    public int[] playerTotalScores;
    public int[] winCounts;

    public int roundWinner;

    public int maxGameRounds = 5;

    public int roundCount;
    bool counted = false;

    // public GameObject lossScreen;
    // public GameObject winScreen;

    public GameObject roundOverScreen;
    public GameObject matchOverScreen;

    public float timeUntilNextRound = 8.0f;

    // Use this for initialization
    void Start ()
    {
        playerCurrentScores = new int[5];
        playerTotalScores = new int[5];
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

    public void RecordScores()
    {
        // Record and update fuel values at end of round
        foreach (GameObject playerShip in GameObject.FindGameObjectsWithTag("Ship"))
        {
            playerCurrentScores[playerShip.GetComponent<ShipScript>().shipNum - 1] = playerShip.GetComponent<ShipScript>().currentFuel;
            playerTotalScores[playerShip.GetComponent<ShipScript>().shipNum - 1] += playerShip.GetComponent<ShipScript>().currentFuel;
        }
  
        // Determine what player has the highest score
        int currentHighestScore = 0;
        roundWinner = 0;

        for (int y = 0; y < playerCurrentScores.Length; y++)
        {
            if (currentHighestScore < playerCurrentScores[y])
            {
                currentHighestScore = playerCurrentScores[y];
                roundWinner = y + 1;

                // NEED TO ADD FUNCTIONALITY FOR DRAWS
            }          
        }

        // Store the win to the winner's total
        if (roundWinner >= 1 && roundWinner <= 5)
        {
            winCounts[roundWinner - 1] += 1;
        }

        // Finish recording score and increase round number
        if (counted == false)
        {
            counted = true;
            roundCount++;
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
            if (n.GetPhotonView().isMine)
            {
                if (n.GetComponent<MovementScript>().playerNum == roundWinner)
                {
                    roundOverScreen.SetActive(true);
                    roundOverScreen.GetComponent<UIRoundVictoryText>().DisplayRoundOverScreen(timeUntilNextRound, true);
                }
                else
                {
                    roundOverScreen.SetActive(true);
                    roundOverScreen.GetComponent<UIRoundVictoryText>().DisplayRoundOverScreen(timeUntilNextRound, false);
                }
            }

            // Reset player's fuel to prevent depositing when positioned at ship
            n.GetComponent<PlayerResource>().resource = 0;

            // Position player at their ship and lock their movement until round starts
            n.transform.position = n.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockObject.transform.position;
            n.GetComponent<MovementScript>().canMove = false;
            n.GetComponent<MovementScript>().lockOverrideTime = timeUntilNextRound;
        }

        yield return new WaitForSeconds(timeUntilNextRound);

        if (roundCount == maxGameRounds)
        {
            matchOverScreen.SetActive(true);

            // Determine what the max amount of rounds won was
            int currentMaxRoundsWon = 0;

            for (int x = 0; x < winCounts.Length; x++)
            {
                if (currentMaxRoundsWon < winCounts[x])
                {
                    currentMaxRoundsWon = winCounts[x];
                }
            }

            matchOverScreen.GetComponent<UIEndGameScreen>().UpdateEndMatchInfo(winCounts, currentMaxRoundsWon);

            // Lock players position and movement to prevent anything from happening behind the endgame overlay
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetPhotonView().isMine)
                {
                    player.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
                    player.GetComponent<MovementScript>().canMove = false;
                    player.GetComponent<MovementScript>().lockOverrideTime = 9999.9f;
                }
            }
        }
        else
        {
            EventManager.ResetObjects();

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetPhotonView().isMine)
                {
                    GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIFuelCollected>().fuelCarriedFuelText.GetComponent<Text>().text = "0";
                    GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIFuelCollected>().fuelShipAmountText.GetComponent<Text>().text = "0 / 100";
                }
            }

            // winScreen.SetActive(false);
            // lossScreen.SetActive(false);

            counted = false;
        }

        roundOverScreen.SetActive(false);

    }
}
