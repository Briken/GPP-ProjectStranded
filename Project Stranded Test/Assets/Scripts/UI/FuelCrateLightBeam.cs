using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelCrateLightBeam : MonoBehaviour {

    public bool isActive = false;
    public bool voteStarted = false;

    Color activeColour = Color.green;
    Color inactiveColour = Color.red;
    Color voteColour = Color.white;

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<SpriteRenderer>().color = inactiveColour;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (voteStarted)
        {
            gameObject.GetComponent<SpriteRenderer>().color = voteColour;
        }
        else if (isActive)
        {
            gameObject.GetComponent<SpriteRenderer>().color = activeColour;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = inactiveColour;
        }

    }
}
