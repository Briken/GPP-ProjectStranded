using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;


public class VotingSystem : Photon.PunBehaviour
{

    public GameObject voteCount;
    public GameObject voteCard;

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

    public int CheckVote()
    {
       if ( player1Current > player2Current &&
           player1Current > player3Current &&
           player1Current > player4Current && 
           player1Current > player5Current)
        {
            return 1;
        }

        if (player2Current >player1Current &&
          player2Current >player3Current &&
          player2Current >player4Current &&
          player2Current >player5Current)
        {
            return 2;
        }

        if (player3Current >player2Current &&
          player3Current >player1Current &&
          player3Current >player4Current &&
          player3Current >player5Current)
        {
            return 3;
        }

        if (player4Current >player2Current &&
          player4Current >player3Current &&
          player4Current >player1Current &&
          player4Current >player5Current)
        {
            return 4;
        }

        if (player5Current >player2Current &&
          player5Current >player3Current &&
          player5Current >player4Current &&
          player5Current >player1Current)
        {
            return 5;
        }

        else
        {
            return (int)Random.Range(1, 5);
        }

    }

    [PunRPC]
    public void IncrementP1RPC()
    {
        player1Total++;
        player1Current++;
    }

    [PunRPC]
    public void IncrementP2RPC()
    {
        player2Total++;
        player2Current++;
    }

    [PunRPC]
    public void IncrementP3RPC()
    {
        player3Total++;
        player3Current++;
    }

    [PunRPC]
    public void IncrementP4RPC()
    {
        player4Total++;
        player4Current++;
    }
    [PunRPC]
    public void IncrementP5RPC()
    {
        player5Total++;
        player5Current++;
    }
}