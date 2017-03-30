using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;

public class RoomData : Photon.PunBehaviour {

    public int pNum = 77;
    public InputField roomTitle;
    public InputField userInput;
    public GameObject outVote;
    public string roomName;
    public string userName;
    string[] userNames;
    public int player1Score;
    public int player2Score;
    public int player3Score;
    public int player4Score;
    public int player5Score;
    public int roundCount;
    public GameObject netObj;
    public bool p1Win;
    public bool p2Win;
    public bool p3Win;
    public bool p4Win;
    public bool p5Win;

    public GameObject lossScreen;
    public GameObject winScreen;

    bool counted = false;
    bool filled = false;

	// Use this for initialization
	void Start ()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        roomName = null;
        DontDestroyOnLoad(this);
        if (SceneManager.GetActiveScene().name == "MainScene-Recovered")
        {
            winScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().winScreen;
            lossScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().lossScreen;
        }
        // Set username to a default name if data hasn't been saved yet
        if (PlayerPrefs.GetString("Username") == "")
        {
            userName = "New Player";
        }
        else
        {
            userName = PlayerPrefs.GetString("Username");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.FindGameObjectsWithTag("Player").Length >= 5 && filled == false)
        {
            RecordNames();
            filled = true;
        }
        
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            for (int n = 0; n < GameObject.FindGameObjectsWithTag("Username").Length; n++)
            {
                GameObject.FindGameObjectsWithTag("Username")[n].GetComponent<Text>().text = userNames[n];
            }
        }
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == "MainScene-Recovered")
        {
            winScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().winScreen;
            lossScreen = GameObject.Find("Main Game UI Handler").GetComponent<UIMainGameHandler>().lossScreen;
            if (GameObject.Find("NetworkManager") == null)
            {
                Instantiate(netObj);
            }
        }
    }
    public void SetRoomName()
    {
        roomName = roomTitle.text;
    }

    public void SetUsername()
    {
        userName = userInput.text;

        // Save the player's username
        PlayerPrefs.SetString("Username", userName);
    }

    // Used for manually reloading the username after being saved in the My Character screen
    public void ManualUsernameLoad()
    {
        userName = PlayerPrefs.GetString("Username");
    }

    void RecordNames()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetComponent<UserName>() != null)
            {
                userNames[n.GetComponent<MovementScript>().playerNum] = GetComponent<UserName>().myName;
            }
        }
        
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
                    if (n.GetComponent<MovementScript>().pv.isMine && n.GetComponent<MovementScript>().playerNum == 0)
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
                    if (n.GetPhotonView().isMine && n.GetComponent<MovementScript>().playerNum == 1)
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
            if (p4Win)
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
            if (p5Win)
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
            SceneManager.LoadScene("MainScene-Recovered");
            counted = false;
        }

    }
}
