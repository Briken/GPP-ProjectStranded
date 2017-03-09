using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class VoteTally : PunBehaviour {
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
