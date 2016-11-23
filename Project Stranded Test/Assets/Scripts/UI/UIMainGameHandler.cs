using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainGameHandler : MonoBehaviour {

    public GameObject timeRemainingText;
    public GameObject fuelCarriedText;

    GameObject mainPlayer;
    float gameTimeRemaining;

    bool noMain = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (noMain)
        {
            foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (n.GetComponent<MovementScript>().playerNum == 1)
                {
                    mainPlayer = n;
                    noMain = false;
                }
            }
        }
        // Game time handling:
        //   mainPlayer = GameObject.FindGameObjectWithTag("Player");
        if (mainPlayer != null)
        {
            gameTimeRemaining = mainPlayer.GetComponent<GameTimer>().timer;
        }
        int minutes = Mathf.FloorToInt(gameTimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(gameTimeRemaining - (minutes * 60));
        string gameTimeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeRemainingText.GetComponent<Text>().text = "TIME: " + gameTimeString;
        if (mainPlayer != null)
        {
            fuelCarriedText.GetComponent<Text>().text = "FUEL: " + mainPlayer.GetComponent<PlayerResource>().resource.ToString();
        }
    }
}
