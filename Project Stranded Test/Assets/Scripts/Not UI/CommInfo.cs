using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class CommInfo : MonoBehaviour {

    public GameObject myPlayer;
    public Vector3 initialPos;
    public float duration;
    public bool eventFired;

	// Use this for initialization
	void Start () {

        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.gameObject.GetComponent<MovementScript>().photonView.isMine)
            {
                myPlayer = n;
            }
        }

        initialPos = myPlayer.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        duration += Time.deltaTime;
        if (myPlayer.transform.position != initialPos && eventFired == false)
        {
            Analytics.CustomEvent("Player Moved After Communicating", new Dictionary<string, object>
             {
                 { "Player moved", myPlayer.GetComponent<MovementScript>().publicUsername + " Moved after " + duration.ToString() + " seconds"},
                 { "Timestamp: ", System.DateTime.Now.ToString()},
             });
            eventFired = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<MovementScript>().playerNum != myPlayer.gameObject.GetComponent<MovementScript>().playerNum)
        {
            Object.FindObjectOfType<CommScript>().PlayerTagged(other.gameObject.GetComponent<MovementScript>().publicUsername);
        }
    }
}
