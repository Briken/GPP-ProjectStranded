using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class VoteTally : PunBehaviour {
    int player1Total;
    int player1Current;

    int player2Total;
    int player2Current;

    int player3Total;
    int player3Current;

    int player4Total;
    int player4Current;

    int player5Total;
    int player5Current;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [PunRPC]
    public void IncrementP1()
    {
        player1Total++;
        player1Current++;
    }

    [PunRPC]
    public void IncrementP2()
    {
        player2Total++;
        player2Current++;
    }

    [PunRPC]
    public void IncrementP3()
    {
        player3Total++;
        player3Current++;
    }

    [PunRPC]
    public void IncrementP4()
    {
        player4Total++;
        player4Current++;
    }
    [PunRPC]
    public void IncrementP5()
    {
        player5Total++;
        player5Current++;
    }
}
