using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;


public class VotingSystem : Photon.PunBehaviour
{

    public GameObject voteCount;
    public GameObject voteCard;

    public int[] playerTotals;

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


    // Use this for initialization
    void Start()
    {
        playerTotals = new int[GameObject.FindGameObjectsWithTag("Player").Length];
        voteCount = GameObject.FindGameObjectWithTag("VoteObj");
        voteCard = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteCards;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallVote()
    {
        voteCard.SetActive(true);
        // on button press incrememnt the vote count dependant on the button presser
        // rpc to ensure all buttons are synced
        //send the highest vote cvount away from the other players

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
            Debug.Log("player with number: " + (index + 1) + " has " + playersVoted[index] + " votes");
        }

        {
            System.Random random = new System.Random(seed);
            return playersVoted[random.Next(0, playersVoted.Count)];
        }

    }

    [PunRPC]
    public void IncrementPlayerRPC(int index)
    {
        playerTotals[index]++;
    }
}