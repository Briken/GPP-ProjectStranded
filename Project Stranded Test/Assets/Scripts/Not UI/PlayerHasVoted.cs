using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHasVoted : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeHasVoted()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetPhotonView().isMine)
            {
                n.GetComponent<VotingSystem>().hasVoted = true;
            }
        }
    }
}
