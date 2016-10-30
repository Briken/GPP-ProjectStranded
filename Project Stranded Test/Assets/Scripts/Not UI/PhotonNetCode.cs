﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon;

public class PhotonNetCode : Photon.PunBehaviour {

    public GameObject player;
    public int playerNum;
    //public GameObject mainCamera;
    

	// Use this for initialization
	void Start ()
    {
      //  mainCamera = GameObject.Find("Main Camera");
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
        //debug.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    void OnGui()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        
        if (playerNum ==0)
        {
            GameObject newPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            newPlayer.GetComponent<MovementScript>().team = 1;
            newPlayer.GetComponent<MovementScript>().playerNum = playerNum;
            playerNum++;
        }
        else if (playerNum % 2 == 0)
        {
            GameObject newPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            newPlayer.GetComponent<MovementScript>().team = 1;
            newPlayer.GetComponent<MovementScript>().playerNum = playerNum;
            playerNum++;
        }
        else if (playerNum%2 != 0)
        {
            GameObject newPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            newPlayer.GetComponent<MovementScript>().team = 2;
            newPlayer.GetComponent<MovementScript>().playerNum = playerNum;
            playerNum++;
        }
    }
}
