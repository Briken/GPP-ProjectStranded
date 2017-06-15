using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCount : MonoBehaviour {
    public int player1Score;
    public int player2Score;
    public int player3Score;
    public int player4Score;
    public int player5Score;
    public bool p1Win;
    public bool p2Win;
    public bool p3Win;
    public bool p4Win;
    public bool p5Win;
    public int roundCount;
    bool counted = false;

    public GameObject lossScreen;
    public GameObject winScreen;

    // Use this for initialization
    void Start ()
    {
        if (SceneManager.GetActiveScene().name == "MainScene-Recovered")
        {
            winScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().winScreen;
            lossScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().lossScreen;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecordScores(int playerNum, int score)
    {
        if (playerNum == 0)
        {
            player1Score += score;
        }

        if (playerNum == 1)
        {
            player2Score += score;
        }

        if (playerNum == 2)
        {
            player3Score += score;
        }

        if (playerNum == 3)
        {
            player4Score += score;
        }

        if (playerNum == 4)
        {
            player5Score += score;
        }
        if (counted == false)
        {
            counted = true;
            roundCount++;
        }

        if (player1Score > player2Score && player1Score > player3Score && player1Score > player4Score && player1Score > player5Score)
        {
            p1Win = true;
        }
        if (player2Score > player1Score && player2Score > player3Score && player2Score > player4Score && player2Score > player5Score)
        {
            p2Win = true;
        }
        if (player3Score > player1Score && player3Score > player2Score && player3Score > player4Score && player3Score > player5Score)
        {
            p3Win = true;
        }
        if (player4Score > player1Score && player4Score > player3Score && player4Score > player2Score && player4Score > player5Score)
        {
            p4Win = true;
        }
        if (player5Score > player1Score && player5Score > player3Score && player5Score > player4Score && player5Score > player2Score)
        {
            p5Win = true;
        }
    }
    public void LoadNextRound()
    {

        StartCoroutine(LoadLevel());

    }
    IEnumerator LoadLevel()
    {
        if (p1Win || p2Win || p3Win || p4Win || p5Win)
        {
            if (p1Win)
            {
                foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (n.GetComponent<MovementScript>().pv.isMine && n.GetComponent<MovementScript>().playerNum == 1)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        lossScreen.SetActive(true);
                    }
                }
            }
            if (p2Win)
            {
                foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (n.GetPhotonView().isMine && n.GetComponent<MovementScript>().playerNum == 2)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        lossScreen.SetActive(true);
                    }
                }
            }
            if (p3Win)
            {
                foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (n.GetPhotonView().isMine && n.GetComponent<MovementScript>().playerNum == 3)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        lossScreen.SetActive(true);
                    }
                }
            }
            if (p4Win)
            {
                foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (n.GetPhotonView().isMine && n.GetComponent<MovementScript>().playerNum == 4)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        lossScreen.SetActive(true);
                    }
                }
            }
            if (p5Win)
            {
                foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (n.GetPhotonView().isMine && n.GetComponent<MovementScript>().playerNum == 5)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        lossScreen.SetActive(true);
                    }
                }
            }
        }
        else
        {
            lossScreen.SetActive(true);
        }
        yield return new WaitForSeconds(7);
        p1Win = false;
        p2Win = false;
        p3Win = false;
        p4Win = false;
        p5Win = false;
        if (roundCount == 5)
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {

            PhotonNetwork.LeaveRoom();
            // SceneManager.LoadScene("MainScene-Recovered");
            SceneManager.LoadScene("UI-MainMenu");
            counted = false;
        }

    }
}
