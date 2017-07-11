using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommInfo : MonoBehaviour {

    public GameObject myPlayer;

	// Use this for initialization
	void Start () {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.gameObject.GetComponent<MovementScript>().photonView.isMine)
            {
                myPlayer = n;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<MovementScript>().playerNum != myPlayer.gameObject.GetComponent<MovementScript>().playerNum)
        {
            Object.FindObjectOfType<CommScript>().PlayerTagged(other.gameObject.GetComponent<MovementScript>().playerNum);
        }
    }
}
