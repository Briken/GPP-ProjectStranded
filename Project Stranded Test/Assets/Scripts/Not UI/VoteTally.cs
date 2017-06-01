using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon;

public class VoteTally : MonoBehaviour {


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

    private void IncrementPlayer(int index)
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementPlayerRPC", PhotonTargets.All, index);
        }
    }

    public void IncrementP1()
    {
        IncrementPlayer(0);
    }


    public void IncrementP2()
    {
        IncrementPlayer(1);
    }

   
    public void IncrementP3()
    {
        IncrementPlayer(2);
    }

  
    public void IncrementP4()
    {
        IncrementPlayer(3);
    }
    
    public void IncrementP5()
    {
        IncrementPlayer(4);
    }

   

}
