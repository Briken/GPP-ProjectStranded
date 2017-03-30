using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class VoteTally : Photon.PunBehaviour {


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

    public void IncrementP1()
    {
        photonView.RPC("IncrementP1RPC", PhotonTargets.All);
    }


    public void IncrementP2()
    {
        photonView.RPC("IncrementP2RPC", PhotonTargets.All);
    }

   
    public void IncrementP3()
    {
        photonView.RPC("IncrementP3RPC", PhotonTargets.All);
    }

  
    public void IncrementP4()
    {
        photonView.RPC("IncrementP4RPC", PhotonTargets.All);
    }
    
    public void IncrementP5()
    {
        photonView.RPC("IncrementP5RPC", PhotonTargets.All);
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

    public void OnSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
