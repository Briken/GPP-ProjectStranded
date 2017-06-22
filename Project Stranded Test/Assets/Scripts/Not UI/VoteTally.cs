using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon;

public class VoteTally : MonoBehaviour {


    public int player1Total;
    public int player1Current;

    public int player2Total;
    public int player2Current;

    public int player3Total;
    public int player3Current;

    public int player4Total;
    public int player4Current;

    public int player5Total;
    public int player5Current;

    public GameObject[] votingCards;
    GameObject votingInstructionsText;
    public Color[] votingColours;

    // Use this for initialization
    void Start ()
    {
        votingInstructionsText = GameObject.Find("Text - Vote Instructions");
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
        IncrementPlayer(playerNumber);
        UpdateVoteCards(playerNumber);
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
