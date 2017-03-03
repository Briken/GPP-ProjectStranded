using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundVictoryText : MonoBehaviour {

    public GameObject roundStateTextBox;
    public bool playerWon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (playerWon)
        {
            roundStateTextBox.GetComponent<Text>().text = "YOU WON";
        }
        else
        {
            roundStateTextBox.GetComponent<Text>().text = "YOU LOST";
        }
		
	}
}
