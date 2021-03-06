﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;

public class PhotonNetCode : Photon.PunBehaviour {

    public GameObject voteCards;
    public GameObject voteLoss;
    public GameObject connectionStatusOverlay;
    public GameObject connectionStatusHeader;
    public GameObject connectionStatusPlayerCount;
    public GameObject connectionStatusRoomName;
    public GameObject gData;
    public GameObject player;

    public bool overrideLobbyMax = false;

    public int lobbyMax;
    public int playerNum;

    bool isMasterServer = false;
    bool isActive = false;
    GameObject spawnPoint;

    public GameTimer timer;
    
    
    RoomOptions roomDetails;
   
    TypedLobby typedLobby;
    int currentPlayers;
    string roomName;
    RoomData data;
    int pNum;

    public GameObject[] ships;

    public float roundStartTimer = 10.0f;
    float defaultRoundStartTimer;
    bool roundStarted = false;
    ScoreCount scoreData;


    //public GameObject mainCamera;


    // Use this for initialization
    void Start ()
    {
        Debug.Log(PhotonNetwork.connectionState);
        PhotonNetwork.Disconnect();
        if (GameObject.FindGameObjectWithTag("GameData") == null)
        {
            Instantiate(gData);
            data = GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>();
            if (data.roomName == "T3STROOM")
            {
                
                overrideLobbyMax = true;
            }
            if (!overrideLobbyMax)
            {
                lobbyMax = data.numberOfPlayers;
            }       
        }
        else
        {
            data = GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>();
            if (data.roomName == "T3STROOM1")
            {
                data.numberOfPlayers = 1;
            }
            if (data.roomName == "T3STROOM2")
            {
                data.numberOfPlayers = 2;
            }
            if (!overrideLobbyMax)
            {
                lobbyMax = data.numberOfPlayers;
            }
            
        }

        PhotonNetwork.sendRate = 20; 
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
        roomDetails = new RoomOptions();
        roomDetails.IsVisible = false;
        roomDetails.MaxPlayers = (byte)lobbyMax;
        typedLobby = new TypedLobby();

        //ships = GameObject.FindGameObjectsWithTag("Ship");
       

        defaultRoundStartTimer = roundStartTimer;

        scoreData = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreCount>();

        if (scoreData == null)
        {
            Debug.Log("SCORE DATA: Data could not be found! Is there a Score Data object in the scene?");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {     
        // Start the round timer when the player count has been met
        if (PhotonNetwork.playerList.Length == roomDetails.MaxPlayers && !roundStarted)
        {
            if (roundStartTimer >= 0.0f)
            {
                roundStartTimer -= Time.deltaTime;
            }
            else
            {
                SpawnPlayer();
                roundStarted = true;
                GameObject.FindGameObjectWithTag("HintBox").GetComponent<UIHintBox>().DisplayHint("REFUEL YOUR SHIP", "COLLECT FUEL BY OPENING NEARBY FUEL CRATES AND RETURN IT TO YOUR COLOURED SHIP!", 8.0f);
            }

            // Display round start information
            connectionStatusHeader.GetComponent<Text>().text = "ROUND " + (scoreData.roundCount + 1).ToString() + " OF " + scoreData.maxGameRounds;
            connectionStatusPlayerCount.GetComponent<Text>().text = "ROUND STARTING IN " + Mathf.Round(roundStartTimer).ToString() + " SECONDS!";
            connectionStatusRoomName.GetComponent<Text>().text = "";
        }
        else
        {
            // Reset the timer if the player count is not met
            roundStartTimer = defaultRoundStartTimer;

            // Display the amount of connected players
            connectionStatusHeader.GetComponent<Text>().text = "WAITING FOR PLAYERS...";
            connectionStatusPlayerCount.GetComponent<Text>().text = "PLAYERS CONNECTED: " + PhotonNetwork.playerList.Length.ToString() + " / " + lobbyMax.ToString();

            // Display the room name if valid
            if (PhotonNetwork.room != null)
            {
                connectionStatusRoomName.GetComponent<Text>().text = "ROOM NAME: " + PhotonNetwork.room.name;
            }
            else
            {
                // Display temporary message until player connects to a room via Photon
                connectionStatusRoomName.GetComponent<Text>().text = "CONNECTING TO ROOM...";
            }
        }

        //Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
        //debug.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    void OnGui()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        if (data.roomName != null)
        {
            roomName = data.roomName;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomDetails, typedLobby);
        }
        else
        {
            // Debug.Log("Joining Room");
            PhotonNetwork.JoinRandomRoom();
        }

        
    }

    void OnPhotonRandomJoinFailed()
    {
        // Debug.Log("Failed to Join Room, creating room");
        PhotonNetwork.CreateRoom(null, roomDetails, typedLobby);
    }

    void OnJoinedRoom()
    {
        isMasterServer = PhotonNetwork.playerList.Length == 1;

        if (PhotonNetwork.playerList.Length == roomDetails.MaxPlayers)
        {
         
            // SpawnPlayer();
      //   timer.enabled = true;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            if (GameObject.FindGameObjectWithTag("BackgroundMusic") != null)
            {
                GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<GameMusicHandler>().BeginMatchPlay();
            }
        }
        
       
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == "MainScene-Recovered")
        {
            SpawnPlayer();
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        
        if (PhotonNetwork.playerList.Length == roomDetails.MaxPlayers)
        {
            // SpawnPlayer();
        }
        
    }


    void SpawnPlayer()
    {
        GameObject controlledPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
        timer.enabled = true;
        if (isMasterServer)
        {
            foreach(ResourceScript n in FindObjectsOfType<ResourceScript>())
            {
                int seed = Random.Range(0, int.MaxValue);
                n.photonView.RPC("ReceiveSeed", PhotonTargets.All, seed);
            }
        }

        pNum = controlledPlayer.GetComponent<PhotonView>().ownerId;
        Vector3 moveTo = ships[pNum - 1].GetComponent<PlayerShipDocking>().dockObject.transform.position;
        controlledPlayer.transform.position = moveTo;
        controlledPlayer.GetComponent<MovementScript>().photonView.RPC("SetNum", PhotonTargets.All, pNum);
        controlledPlayer.GetComponent<MovementScript>().photonView.RPC("SetUsername", PhotonTargets.All, PlayerPrefs.GetString("Username"));
        controlledPlayer.GetComponent<MovementScript>().photonView.RPC("SetAppearance", PhotonTargets.All, PlayerPrefs.GetInt("Player Head"), PlayerPrefs.GetInt("Player Body"));

        connectionStatusOverlay.SetActive(false);     
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        // SceneManager.LoadScene("UI-MainMenu");

        // Display error message when a player disconnects providing the match has not ended
        if (scoreData.roundCount != scoreData.maxGameRounds)
        {
            GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIErrorMessage>().DisplayErrorMessage("MATCH ERROR", "A PLAYER DISCONNECTED FROM THE GAME");
        }     
    }
    
}
