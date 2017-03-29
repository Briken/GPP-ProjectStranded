using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;

public class PhotonNetCode : Photon.PunBehaviour {

    public GameObject voteCards;
    public GameObject voteLoss;

    bool isActive = false;
    public GameObject gData;
    public int lobbyMax;
    GameTimer timer;
    public GameObject player;
    public int playerNum;
    RoomOptions roomDetails;
    public bool radBound = false;
    TypedLobby typedLobby;
    int currentPlayers;
    string roomName;
    RoomData data;

    
    //public GameObject mainCamera;
    

	// Use this for initialization
	void Start ()
    {
        
        if (GameObject.FindGameObjectWithTag("GameData") == null)
        {
            Instantiate(gData);
            data = GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>();
        }
        else
        {
            data = GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>();
        }

        PhotonNetwork.sendRate = 20; 
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
        roomDetails = new RoomOptions();
        roomDetails.IsVisible = false;
        roomDetails.MaxPlayers = (byte)lobbyMax;
        typedLobby = new TypedLobby();

        GameObject netmanager = this.gameObject;
        //timer = netmanager.GetComponent<GameTimer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
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
            PhotonNetwork.JoinRandomRoom();
        }
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null, roomDetails, typedLobby);
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length == roomDetails.MaxPlayers)
        {
           GameObject controlledPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
            controlledPlayer.GetComponent<MovementScript>().playerNum = controlledPlayer.GetComponent<PhotonView>().ownerId;
            timer.enabled = true;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
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
            SpawnPlayer();
        }
        
    }

    void CreateRoom()
    {
        
    }

    void SpawnPlayer()
    {
        GameObject controlledPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
        controlledPlayer.GetComponent<MovementScript>().playerNum = controlledPlayer.GetComponent<PhotonView>().ownerId;
        timer.enabled = true;
    }

    void SetPlayerNums()
    {
        for (int i = 0; i < PhotonNetwork.room.playerCount; i++)
        {
            GameObject.FindGameObjectsWithTag("Player")[i].GetComponent<MovementScript>().playerNum = i;
        }
    }

    
}
