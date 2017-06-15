using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainGameHandler : MonoBehaviour {

    public GameObject timeRemainingText;
    public GameObject playerTeamText;
    public GameObject fuelCarriedText;
    public GameObject fuelCarriedBar;
    public float barMaxValue = 20.0f;
    public float barValue = 0.0f;

    public GameObject winScreen;
    public GameObject lossScreen;

    GameObject mainPlayer;
    float gameTimeRemaining;

    public GameObject voteCards;
    public GameObject voteLoss;

    public GameObject lobbyWait;

    bool noMain = true;

    // Use this for initialization
    void Start()
    {
        timeRemainingText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (noMain)
        {
            foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (n.GetComponent<MovementScript>().photonView.isMine)
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
        // string gameTimeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        string gameTimeString = System.Math.Round(gameTimeRemaining, 2).ToString();

        if (gameTimeRemaining > 60.0f || gameTimeRemaining == 0.0f)
        {
            timeRemainingText.GetComponent<Text>().text = "";
            timeRemainingText.SetActive(false);         
        }
        else
        {
            timeRemainingText.SetActive(true);
            timeRemainingText.GetComponent<Text>().text = "ROUND ENDS IN " + gameTimeString + "s";
        }
        
    }
}
