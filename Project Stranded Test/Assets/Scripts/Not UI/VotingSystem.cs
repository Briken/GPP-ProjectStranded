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

  

}
