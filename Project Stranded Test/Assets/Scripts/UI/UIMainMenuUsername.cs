using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuUsername : MonoBehaviour {

    public GameObject usernameText;
    public string[] defaultUsernames;

	// Use this for initialization
	void Start ()
    {
        // Use default username if player hasn't saved a username yet
        if (PlayerPrefs.GetString("Username") == "")
        {
            PlayerPrefs.SetString("Username", defaultUsernames[Random.Range(0, defaultUsernames.Length-1)]);

            Debug.Log(defaultUsernames.Length.ToString());
        }

        usernameText.gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("Username");


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
