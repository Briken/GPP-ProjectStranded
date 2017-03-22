using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomData : MonoBehaviour {

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
    int roundCount;

    bool filled = false;

	// Use this for initialization
	void Start ()
    {
        roomName = null;
        DontDestroyOnLoad(this);

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
            if (GetComponent<UserName>() != null)
            {
                userNames[GetComponent<MovementScript>().playerNum - 1] = GetComponent<UserName>().myName;
            }
        }
    }
    public void RecordScores(int playerNum, int score)
    {
        if (playerNum == 1)
        {
            player1Score += score;
        }

        if (playerNum == 2)
        {
            player2Score += score;
        }

        if (playerNum == 3)
        {
            player3Score += score;
        }

        if (playerNum == 4)
        {
            player4Score += score;
        }

        if (playerNum == 5)
        {
            player5Score += score;
        }
        roundCount++;
        if (roundCount == 5)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
    
}
