﻿using System;
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

    // Use this for initialization
    void Start()
    {
        voteCount = GameObject.FindGameObjectWithTag("VoteObj");
        voteCard = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteCards;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallVote()
    {
		voteCard.SetActive (true);

		if (playerTotals == null || playerTotals.Length == 0)
		{
			playerTotals = new int[GameObject.FindGameObjectsWithTag ("Player").Length];
		}
        Button[] tempButtons = voteCard.GetComponentsInChildren<Button>();
        foreach (Button n in tempButtons)
        {
            string buttonName = n.gameObject.name;
            int num = buttonName[6] - 48;

			if (num > playerTotals.Length) {
				n.gameObject.SetActive (false);
			} 
			else 
			{
				n.gameObject.SetActive (true);
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
		if (playerTotals.Length == 0)
		{
			playerTotals = new int[GameObject.FindGameObjectsWithTag ("Player").Length];
		}
        playerTotals[index]++;
        voteCard.SetActive(false);
    }
}