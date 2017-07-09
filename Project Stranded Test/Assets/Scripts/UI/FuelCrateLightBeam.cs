using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelCrateLightBeam : MonoBehaviour {

    public bool isActive = false;

    public Color activeColour = Color.green;
    Color inactiveColour = Color.white;

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<SpriteRenderer>().color = inactiveColour;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
            gameObject.GetComponent<SpriteRenderer>().color = activeColour;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = inactiveColour;
        }

    }
}
