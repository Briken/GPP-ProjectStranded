using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainGameHandler : MonoBehaviour {

    public GameObject timeRemainingText;
    public GameObject fuelCarriedText;

    GameObject mainPlayer;
    float gameTimeRemaining;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Game time handling:
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
        gameTimeRemaining = mainPlayer.GetComponent<GameTimer>().timer;

        int minutes = Mathf.FloorToInt(gameTimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(gameTimeRemaining - (minutes * 60));
        string gameTimeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeRemainingText.GetComponent<Text>().text = "TIME: " + gameTimeString;
        fuelCarriedText.GetComponent<Text>().text = "FUEL: " + mainPlayer.GetComponent<PlayerResource>().resource.ToString();
    }
}
