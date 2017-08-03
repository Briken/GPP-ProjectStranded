using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyMaxChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeLobbyMaxValue(int newLobbyMax)
    {
        if (GameObject.FindGameObjectWithTag("GameData") != null)
        {
            GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>().numberOfPlayers = newLobbyMax;
        }
    }
}
