using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserName : MonoBehaviour {

    
    public string myName;
    GameObject gameData;
    bool filled = false;


	// Use this for initialization
	void Start ()
    {
        
        gameData = GameObject.FindGameObjectWithTag("GameData");
        myName = gameData.GetComponent<RoomData>().userName;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    
}
