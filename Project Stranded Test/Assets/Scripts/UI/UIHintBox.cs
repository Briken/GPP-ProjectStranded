using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHintBox : MonoBehaviour {

    public GameObject hintBox;
    public GameObject hintHeader;
    public GameObject hintDetails;

    float displayTime = 0.0f;
    bool isDisplaying = false;

	// Use this for initialization
	void Start ()
    {
        // DisplayHint("REFUEL YOUR SHIP", "REFUEL YOUR SHIP BY COLLECTING FUEL FROM THE SCATTERED CRATES. RETURN IT TO YOUR SHIP TO REFUEL!", 8.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (displayTime >= 0.0f && isDisplaying)
        {
            displayTime -= Time.deltaTime;   
        }	

        if (displayTime <= 0.0f && isDisplaying)
        {
            HideHint();
            isDisplaying = false;
        }
	}

    public void DisplayHint(string header, string details, float time)
    {  
        displayTime = time;
        isDisplaying = true;

        hintHeader.GetComponent<Text>().text = header;
        hintDetails.GetComponent<Text>().text = details;

        hintBox.GetComponent<Animator>().enabled = true;
        hintBox.GetComponent<Animator>().Play("Anim_HintBoxEnter");
    }

    public void HideHint()
    {
        hintBox.GetComponent<Animator>().Play("Anim_HintBoxExit");
    }
}
