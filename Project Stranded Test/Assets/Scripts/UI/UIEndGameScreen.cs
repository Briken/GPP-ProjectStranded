using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGameScreen : MonoBehaviour {

    public GameObject[] playerDetailsObjects;
    public GameObject[] usernameTextObjects;
    public GameObject[] roundsWonTextObjects;
    public GameObject[] playerShips;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void UpdateEndMatchInfo(int[] roundsWon, int mostRoundsWon)
    {
        GameObject[] currentPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int x = 0; x < currentPlayers.Length; x++)
        {
            Debug.Log("DISPLAYING RESULTS FOR PLAYER " + x.ToString());

            playerDetailsObjects[x].SetActive(true);

            // Display player's username if available
            if (playerShips[x].GetComponent<ShipScript>().playerMoves != null)
            {
                usernameTextObjects[x].GetComponent<Text>().text = playerShips[x].GetComponent<ShipScript>().playerMoves.publicUsername;
            }
            else
            {
                usernameTextObjects[x].GetComponent<Text>().text = "--USERNAME ERROR--";
            }
            
            roundsWonTextObjects[x].GetComponent<Text>().text = "ROUNDS WON: " + roundsWon[x].ToString();

            // Change the text to yellow if it is the winning player (in case there are multiple)
            if (roundsWon[x] == mostRoundsWon)
            {
                usernameTextObjects[x].GetComponent<Text>().color = Color.yellow;
                roundsWonTextObjects[x].GetComponent<Text>().color = Color.yellow;
            }
        }
    }
}
