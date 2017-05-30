using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used for displaying error messages to the player via the error UI overlay

public class UIErrorMessage : MonoBehaviour {

    public GameObject errorMessageCanvas;
    public GameObject errorMessageHeader;
    public GameObject errorMessageText;

    // Not currently used
    public GameObject errorMessageIconObject;

	// Use this for initialization
	void Start ()
    {
       // DisplayErrorMessageSample();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void DisplayErrorMessage(string messageHeader, string messageText)
    {
        DisplayErrorOverlay(true);
        errorMessageHeader.GetComponent<Text>().text = messageHeader;
        errorMessageText.GetComponent<Text>().text = messageText;
    }

    // Public function in case we have to manually hide the error screen overlay
    public void DisplayErrorOverlay(bool activeState)
    {
        errorMessageCanvas.gameObject.SetActive(activeState);
    }

    // For testing purposes
    void DisplayErrorMessageSample()
    {
        DisplayErrorMessage("TEST ERROR", "JUST A TEST MESSAGE");
    }
}
