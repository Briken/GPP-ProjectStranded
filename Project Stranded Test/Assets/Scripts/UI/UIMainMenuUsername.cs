using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuUsername : MonoBehaviour {

    public GameObject usernameText;

	// Use this for initialization
	void Start ()
    {
        // Use default username if player hasn't saved a username yet
        if (PlayerPrefs.GetString("Username") == "")
        {
            usernameText.gameObject.GetComponent<Text>().text = "New Player";
        }
        else
        {
            usernameText.gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("Username");
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
