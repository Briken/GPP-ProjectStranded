using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;


public class VotingSystem : Photon.PunBehaviour
{

    public GameObject voteCount;
    public GameObject voteCard;
    public int[] playerTotals;
    public GameObject votingInstructionsText;
    public GameObject voteTally;

    public bool hasVoted = false;

    // Use this for initialization
    void Start()
    {
        voteCount = GameObject.FindGameObjectWithTag("VoteObj");
        voteCard = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteCards;

        // Fetch voting card instructions by quickly enabling and disabling vote card object
        voteCard.SetActive(true);
        votingInstructionsText = GameObject.Find("Text - Vote Instructions");
        voteCard.SetActive(false);

        voteTally = GameObject.FindGameObjectWithTag("VoteObj");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallVote()
    {
        hasVoted = false;
        voteTally.GetComponent<VoteTally>().hasPlayerVoted = false;
		voteCard.SetActive (true);

        // Set voting instructions text to default text and colour
        votingInstructionsText.GetComponent<Text>().text = "TAP ONE CARD TO VOTE";
        votingInstructionsText.GetComponent<Text>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);

        if (playerTotals == null || playerTotals.Length == 0)
		{
			playerTotals = new int[5];
		}

        Button[] tempButtons = voteCard.GetComponentsInChildren<Button>();

        foreach (Button n in tempButtons)
        {
            string buttonName = n.gameObject.name;
            int num = buttonName[6] - 48;

			if (num > playerTotals.Length)
            {
				n.gameObject.SetActive (false);
			} 
			else 
			{
				n.gameObject.SetActive (true);

                // Reset card colour in case it was previously transparent
                n.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
        }
    }

    public int CheckVote(int seed)
    {
        int maxTotal = 0;
        List<int> playersVoted = new List<int>();
        for(int index = 0; index < playerTotals.Length; index++)
        {
            if (playerTotals[index] > maxTotal)
            {
                maxTotal = playerTotals[index];
            }
        }

        for(int index = 0; index < playerTotals.Length; index++)
        {
            if (playerTotals[index] == maxTotal)
            {
                playersVoted.Add(index + 1);
            }
            Debug.Log("player with number: " + (index + 1) + " has " + playerTotals[index] + " votes");
        }

        {
            System.Random random = new System.Random(seed);
            return playersVoted[random.Next(0, playersVoted.Count)];
        }
    }

    [PunRPC]
    public void IncrementPlayerRPC(int index)
    {
        if (!hasVoted)
        {  
		    if (playerTotals.Length == 0)
		    {
                // playerTotals = new int[GameObject.FindGameObjectsWithTag ("Player").Length];
                playerTotals = new int[5];
		    }

            // Increment the value based on the player voted for
            playerTotals[index]++;
        
            // Increment the value on the separate vote tally object
            switch (index)
            {
                case 0:
                    voteTally.GetComponent<VoteTally>().player1Total += 1;
                    voteTally.GetComponent<VoteTally>().player1Current += 1;
                    break;

                case 1:
                    voteTally.GetComponent<VoteTally>().player2Total += 1;
                    voteTally.GetComponent<VoteTally>().player2Current += 1;
                    break;

                case 2:
                    voteTally.GetComponent<VoteTally>().player3Total += 1;
                    voteTally.GetComponent<VoteTally>().player3Current += 1;
                    break;

                case 3:
                    voteTally.GetComponent<VoteTally>().player4Total += 1;
                    voteTally.GetComponent<VoteTally>().player4Current += 1;
                    break;

                case 4:
                    voteTally.GetComponent<VoteTally>().player5Total += 1;
                    voteTally.GetComponent<VoteTally>().player5Current += 1;
                    break;
            }

            hasVoted = true;
        }

        // voteCard.SetActive(false);
    }
}