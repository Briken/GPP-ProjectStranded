using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
//using Photon;

public class VoteTally : MonoBehaviour {


    public int[] playerTotalVotes;
    public int[] playerCurrentVotes;

    public GameObject[] votingCards;
    public GameObject votingInstructionsText;
    public Color[] votingColours;

    public bool hasPlayerVoted = false;

    // Use this for initialization
    void Start ()
    {
        playerTotalVotes = new int[5];
        playerCurrentVotes = new int[5];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void IncrementPlayer(int index)
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementPlayerRPC", PhotonTargets.All, index);
        }
    }

    // Used for button cards
    public void IncrementPlayerVote(int playerNumber)
    {
        if (!hasPlayerVoted)
        {
            IncrementPlayer(playerNumber);
            UpdateVoteCards(playerNumber);
            hasPlayerVoted = true;
        }
    }

    void UpdateVoteCards(int playerCardNumber)
    {
        foreach(GameObject votingCard in votingCards)
        {
            // Make non-selected cards transparent
            if (votingCard == votingCards[playerCardNumber])
            {
                votingCard.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                votingCard.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            }
        }

        // Update instructions text to display what player the player voted for
        foreach (MovementScript n in FindObjectsOfType<MovementScript>())
        {
            if (n.pv.isMine)
            {
                Analytics.CustomEvent("PlayerVoted", new Dictionary<string, object>
                {
                    { "player " + n.playerNum.ToString(), "voted out " + playerCardNumber+1.ToString()},
                });
            }
        }
        switch (playerCardNumber)
        {   
            case 0:
                votingInstructionsText.GetComponent<Text>().text = "YOU VOTED RED PLAYER";
                break;

            case 1:
                votingInstructionsText.GetComponent<Text>().text = "YOU VOTED BLUE PLAYER";
                break;

            case 2:
                votingInstructionsText.GetComponent<Text>().text = "YOU VOTED GREEN PLAYER";
                break;

            case 3:
                votingInstructionsText.GetComponent<Text>().text = "YOU VOTED YELLOW PLAYER";
                break;

            case 4:
                votingInstructionsText.GetComponent<Text>().text = "YOU VOTED ORANGE PLAYER";
                break;
        }

        // Change voting card colour text to colour of voted player
        votingInstructionsText.GetComponent<Text>().color = votingColours[playerCardNumber];
    }

}
