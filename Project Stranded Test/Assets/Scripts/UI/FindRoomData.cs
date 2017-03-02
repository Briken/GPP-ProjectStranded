using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to find and reference the persistent room data so that Unity's UI features can be used more easily

public class FindRoomData : MonoBehaviour {

    public GameObject roomData;
    public InputField roomDataInputField;

	// Use this for initialization
	void Start ()
    {
        roomData = GameObject.FindGameObjectWithTag("GameData");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FindRoomDataAndSetRoom()
    {
        // Set room name and convert to upper due to font being uppercase
        roomData.GetComponent<RoomData>().roomName = roomDataInputField.text.ToUpper();
    }
}
