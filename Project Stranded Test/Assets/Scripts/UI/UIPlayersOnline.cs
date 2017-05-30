using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class UIPlayersOnline : Photon.PunBehaviour {

    int worldPlayers = 0;
    public GameObject worldPlayersText;

    // Use this for initialization
    void Start ()
    {
        // Connect to the network from main menu
        PhotonNetwork.ConnectUsingSettings("0.1");
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateTotalPlayerCount();
    }

    void UpdateTotalPlayerCount()
    {
        worldPlayers = PhotonNetwork.countOfPlayers;
        worldPlayersText.GetComponent<Text>().text = "PLAYERS ONLINE: " + worldPlayers.ToString();
    }
}
