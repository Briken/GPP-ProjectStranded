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
    
    
    public GameObject netObj;
    

    public GameObject lossScreen;
    public GameObject winScreen;

    public int numberOfPlayers = 5;

    bool filled = false;

	// Use this for initialization
	void Start ()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
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
        
        //if (SceneManager.GetActiveScene().name == "EndScene")
        //{
        //    for (int n = 0; n < GameObject.FindGameObjectsWithTag("Username").Length; n++)
        //    {
        //        userNames[n] = GameObject.FindGameObjectsWithTag("Username")[n].GetComponent<Text>().text;
        //    }
        //}
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
        //foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        //{
        //    if (n.GetComponent<UserName>() != null)
        //    {
        //        userNames[n.GetComponent<MovementScript>().playerNum] = GetComponent<UserName>().myName;
        //    }
        //}
        
    }
   

  
}
