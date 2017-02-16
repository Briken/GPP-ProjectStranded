﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour {

    InputField RoomTitle;
    public string roomName;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    void SetRoomName()
    {
        roomName = RoomTitle.text;
    } 

}
