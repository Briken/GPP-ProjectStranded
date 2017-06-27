using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class UIVotedOutHider : Photon.PunBehaviour
{

    float remainingDisplayTime;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (remainingDisplayTime > 0.0f)
        {
            remainingDisplayTime -= Time.deltaTime;
        }

		if (remainingDisplayTime <= 0.0f)
        {
            // Temporary work around for players getting frozen due to vote not appearing / ending properly
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (photonView.isMine)
                {
                    player.GetComponent<MovementScript>().canMove = true;
                }
            }

            gameObject.SetActive(false);
        }
	}

    public void DisplayVotedOut(float displayDuration)
    {
        remainingDisplayTime = displayDuration;
    }
}
