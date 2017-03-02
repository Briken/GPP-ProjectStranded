﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomData : MonoBehaviour {

    public InputField roomTitle;
    public InputField userInput;
    public string roomName;
    public string userName;
    string[] userNames;


    bool filled = false;

	// Use this for initialization
	void Start ()
    {
        roomName = null;
        DontDestroyOnLoad(this);	
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
    }
    void RecordNames()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            userNames[GetComponent<MovementScript>().playerNum - 1] = GetComponent<UserName>().myName;
        }
    }

    
}