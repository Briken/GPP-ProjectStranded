using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon;

public class PhotonNetCode : Photon.PunBehaviour {

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
        timer = netmanager.GetComponent<GameTimer>();
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
        timer.enabled = true;

        while (currentPlayers < roomDetails.MaxPlayers)
        {
            currentPlayers = PhotonNetwork.playerList.GetLength(0);
        }
        if (currentPlayers == roomDetails.MaxPlayers)
        {
            GameObject newPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
        }
    }

    void CreateRoom()
    {
        
    }
}
