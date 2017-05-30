using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInternetConnection : MonoBehaviour {

    public GameObject connectionWarningText;
    public GameObject startGameButton;
    public GameObject startGameText;

    public GameObject errorScreenHandler;
    public GameObject startGameScreen;
    public GameObject customLobbyScreen;
    public GameObject mainMenuScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // Prevent the player from attempting to start a game if they are on any start game screen
            if (startGameScreen.GetActive() || customLobbyScreen.GetActive())
            {
                mainMenuScreen.SetActive(true);
                errorScreenHandler.GetComponent<UIErrorMessage>().DisplayErrorMessage("NETWORK ERROR", "INTERNET CONNECTION HAS BEEN LOST");
            }

            connectionWarningText.SetActive(true);
            startGameButton.GetComponent<Button>().interactable = false;
            startGameText.GetComponent<Text>().color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        }
        else
        {
            connectionWarningText.SetActive(false);
            startGameButton.GetComponent<Button>().interactable = true;
            startGameText.GetComponent<Text>().color = Color.white;
        }
    }
}
