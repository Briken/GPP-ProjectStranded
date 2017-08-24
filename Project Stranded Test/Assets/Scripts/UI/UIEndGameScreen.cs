using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class UIEndGameScreen : Photon.PunBehaviour
{

    public GameObject[] playerDetailsObjects;
    public GameObject[] usernameTextObjects;
    public GameObject[] roundsWonTextObjects;
    public GameObject[] playerShips;
    public GameObject[] accoladeNameTextObjects;
    public GameObject[] accoladeDescriptionTextObjects;

    GameObject localPlayer;

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

        foreach (GameObject player in currentPlayers)
        {
            if (player.GetPhotonView().isMine)
            {
                localPlayer = player;
            }
        }

        for (int x = 0; x < currentPlayers.Length; x++)
        {
            // Debug.Log("DISPLAYING RESULTS FOR PLAYER " + x.ToString());

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
            
            // Quick check for when to use the plural for round
            if (roundsWon[x] == 1)
            {
                roundsWonTextObjects[x].GetComponent<Text>().text = roundsWon[x].ToString() + " ROUND WON";
            }
            else
            {
                roundsWonTextObjects[x].GetComponent<Text>().text = roundsWon[x].ToString() + " ROUNDS WON";
            }
            
            accoladeNameTextObjects[x].GetComponent<Text>().text = localPlayer.GetComponent<PlayerStatTracker>().playerFinalAccolade[x].accoladeName;
            accoladeDescriptionTextObjects[x].GetComponent<Text>().text = localPlayer.GetComponent<PlayerStatTracker>().playerFinalAccolade[x].accoladeDescription;

            // Change the text to yellow if it is the winning player (in case there are multiple)
            if (roundsWon[x] == mostRoundsWon)
            {
                usernameTextObjects[x].GetComponent<Text>().color = Color.yellow;
                roundsWonTextObjects[x].GetComponent<Text>().color = Color.yellow;
            }
        }
    }
}
