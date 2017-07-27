using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Photon;


public class VotingSystem : Photon.PunBehaviour
{

    public GameObject voteCount;
    public GameObject voteCard;
    public int[] playerTotals;
    public GameObject votingInstructionsText;
    public GameObject voteTally;
    public Button[] tempButtons;
    GameObject votedOutScreen;

    public bool hasVoted = false;
    public bool displayDebugVoteValues = false;

    // Debug voting values
    public GameObject[] debugVoteCountText;

    // Use this for initialization
    void Start()
    {
        voteCount = GameObject.FindGameObjectWithTag("VoteObj");
        voteCard = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteCards;

        playerTotals = new int[5];
        debugVoteCountText = new GameObject[5];

        // Fetch voting card instructions by quickly enabling and disabling vote card object
        voteCard.SetActive(true);

        votingInstructionsText = GameObject.Find("Text - Vote Instructions");

        // Fetch and assign debug text objects (...messy but safe)
        debugVoteCountText[0] = GameObject.Find("Player1VoteCount");
        debugVoteCountText[1] = GameObject.Find("Player2VoteCount");
        debugVoteCountText[2] = GameObject.Find("Player3VoteCount");
        debugVoteCountText[3] = GameObject.Find("Player4VoteCount");
        debugVoteCountText[4] = GameObject.Find("Player5VoteCount");

        tempButtons = voteCard.GetComponentsInChildren<Button>();

        voteCard.SetActive(false);

        // Fetch voted out overlay via the Network Manager
        votedOutScreen = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteLoss;

        voteTally = GameObject.FindGameObjectWithTag("VoteObj");

    }

    // Update is called once per frame
    void Update()
    {
        // Display and update current vote numbers if required
        for (int index = 0; index < playerTotals.Length; index++)
        {
            if (displayDebugVoteValues)
            {
                debugVoteCountText[index].GetComponent<Text>().text = playerTotals[index].ToString();
            }
            else
            {
                debugVoteCountText[index].GetComponent<Text>().text = "";
            }
        }
    }

    public void CallVote(List<GameObject> votingPlayers, float votingCardErrorTimeout)
    {
        hasVoted = false;
        voteTally.GetComponent<VoteTally>().hasPlayerVoted = false;
        voteCard.SetActive(true);

        // Reuse of voted out overlay script to hide vote cards in the event that they do not disappear after vote or unexpected freeze
        voteCard.GetComponent<UIVotedOutHider>().DisplayVotedOut(votingCardErrorTimeout);

        // Reset vote values when vote called
        foreach (GameObject votingPlayer in votingPlayers)
        {
            for (int index = 0; index < playerTotals.Length; index++)
            {
                votingPlayer.GetComponent<VotingSystem>().playerTotals[index] = 0;
                voteTally.GetComponent<VoteTally>().playerCurrentVotes[index] = 0;
            }
        }

        // Set voting instructions text to default text and colour
        votingInstructionsText.GetComponent<Text>().text = "TAP ONE CARD TO VOTE";
        votingInstructionsText.GetComponent<Text>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);

        // Initially hide all buttons prior to determining what ones to display
        foreach (Button tempButton in tempButtons)
        {
            tempButton.gameObject.SetActive(false);
        }

        // Activate and show the button if its index value matches the associated player number
        foreach (GameObject votingPlayer in votingPlayers)
        {
            if (!votingPlayer.GetPhotonView().isMine)
            {
                tempButtons[votingPlayer.GetComponent<MovementScript>().playerNum - 1].gameObject.SetActive(true);
                tempButtons[votingPlayer.GetComponent<MovementScript>().playerNum - 1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    public int CheckVote(int seed)
    {
        int maxTotal = 0;
        List<int> playersVoted = new List<int>();
        int maxTotalMatchedCount = 0;

        // Check through each of the player totals to find and assign the highest value
        for (int index = 0; index < playerTotals.Length; index++)
        {
            if (playerTotals[index] > maxTotal)
            {
                maxTotal = playerTotals[index];

                Debug.Log("CHECKING VOTE: New max value of " + playerTotals[index].ToString() + " votes for Player " + (index + 1).ToString());
            }
        }

        for (int index = 0; index < playerTotals.Length; index++)
        {
            // Add a player to the list of players to vote out if they have the highest vote count
            if (playerTotals[index] == maxTotal)
            {
                // Add player to the list alongside their player ID
                playersVoted.Insert(playersVoted.Count, index + 1);

                maxTotalMatchedCount += 1;
            }

            Debug.Log("VOTE RESULTS: Player with number: " + (index + 1) + " has " + playerTotals[index].ToString() + " votes");
        }

        // Check if there has been multiple matched vote amounts and choose a player at random to vote out
        if (maxTotalMatchedCount >= 2)
        {
            // Generate random number and output int value of player to kick out
            System.Random random = new System.Random(seed);

            Debug.Log("VOTE SYSTEM: Draw - Providing boot value of " + playersVoted[random.Next(0, playersVoted.Count)]);

            return playersVoted[random.Next(0, playersVoted.Count)];
        }
        else
        {
            Debug.Log("VOTE SYSTEM: No Draw - Providing boot value of " + playersVoted[0].ToString());
            return playersVoted[0];
        }


    }

    // Increment values over the network
    [PunRPC]
    public void IncrementPlayerRPC(int index)
    {
        // Increment the value based on the player voted for
        playerTotals[index]++;

        // Increment the values on the separate vote tally
        if (photonView.isMine)
        {
            voteTally.GetComponent<VoteTally>().playerCurrentVotes[index]++;
            voteTally.GetComponent<VoteTally>().playerTotalVotes[index]++;
        }
    }

    // Used to display the voting screen via ResourceScript on a specific player
    public void DisplayVotedOutScreen()
    {
        if (photonView.isMine)
        {
            votedOutScreen.SetActive(true);
            votedOutScreen.GetComponent<UIVotedOutHider>().DisplayVotedOut(4.0f);
        }
    }
}