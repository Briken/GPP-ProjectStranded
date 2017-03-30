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

    public void IncrementP1()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementP1RPC", PhotonTargets.All);
        }
    }


    public void IncrementP2()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementP2RPC", PhotonTargets.All);
        }
    }

   
    public void IncrementP3()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementP3RPC", PhotonTargets.All);
        }
    }

  
    public void IncrementP4()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementP4RPC", PhotonTargets.All);
        }
    }
    
    public void IncrementP5()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            n.GetComponent<VotingSystem>().photonView.RPC("IncrementP5RPC", PhotonTargets.All);
        }
    }

   

}
