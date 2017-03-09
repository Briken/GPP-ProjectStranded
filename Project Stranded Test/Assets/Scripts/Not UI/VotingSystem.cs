using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class VotingSystem : Behaviour {

    public GameObject voteCount;


    // Use this for initialization
    void Start ()
    {
        voteCount = GameObject.FindGameObjectWithTag("VoteObj");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallVote()
    {
        voteCount.SetActive(true);
        // on button press incrememnt the vote count dependant on the button presser
        // rpc to ensure all buttons are synced
        //send the highest vote cvount away from the other players
    }

    public int CheckVote()
    {
        if (voteCount.GetComponent<VoteTally>().player1Current > voteCount.GetComponent<VoteTally>().player2Current &&
           voteCount.GetComponent<VoteTally>().player1Current > voteCount.GetComponent<VoteTally>().player3Current &&
           voteCount.GetComponent<VoteTally>().player1Current > voteCount.GetComponent<VoteTally>().player4Current &&
           voteCount.GetComponent<VoteTally>().player1Current> voteCount.GetComponent<VoteTally>().player5Current)
        {
            return 1;
        }

        if (voteCount.GetComponent<VoteTally>().player2Current > voteCount.GetComponent<VoteTally>().player1Current &&
           voteCount.GetComponent<VoteTally>().player2Current > voteCount.GetComponent<VoteTally>().player3Current &&
           voteCount.GetComponent<VoteTally>().player2Current > voteCount.GetComponent<VoteTally>().player4Current &&
           voteCount.GetComponent<VoteTally>().player2Current > voteCount.GetComponent<VoteTally>().player5Current)
        {
            return 2;
        }

        if (voteCount.GetComponent<VoteTally>().player3Current > voteCount.GetComponent<VoteTally>().player2Current &&
           voteCount.GetComponent<VoteTally>().player3Current > voteCount.GetComponent<VoteTally>().player1Current &&
           voteCount.GetComponent<VoteTally>().player3Current > voteCount.GetComponent<VoteTally>().player4Current &&
           voteCount.GetComponent<VoteTally>().player3Current > voteCount.GetComponent<VoteTally>().player5Current)
        {
            return 3;
        }

        if (voteCount.GetComponent<VoteTally>().player4Current > voteCount.GetComponent<VoteTally>().player2Current &&
           voteCount.GetComponent<VoteTally>().player4Current > voteCount.GetComponent<VoteTally>().player3Current &&
           voteCount.GetComponent<VoteTally>().player4Current > voteCount.GetComponent<VoteTally>().player1Current &&
           voteCount.GetComponent<VoteTally>().player4Current > voteCount.GetComponent<VoteTally>().player5Current)
        {
            return 4;
        }

        if (voteCount.GetComponent<VoteTally>().player5Current > voteCount.GetComponent<VoteTally>().player2Current &&
           voteCount.GetComponent<VoteTally>().player5Current > voteCount.GetComponent<VoteTally>().player3Current &&
           voteCount.GetComponent<VoteTally>().player5Current > voteCount.GetComponent<VoteTally>().player4Current &&
           voteCount.GetComponent<VoteTally>().player5Current > voteCount.GetComponent<VoteTally>().player1Current)
        {
            return 5;
        }

        else
        {
            return (int)Random.Range(1, 5);
        }

    }
  

}
