using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to prevent GameData duplications when returning to the main menu in the new UI setup

public class GameDataSpawner : MonoBehaviour {

    public GameObject gameDataPrefab;

	// Use this for initialization
	void Start ()
    {
        if (GameObject.FindGameObjectWithTag("GameData") == null)
        {
            Instantiate(gameDataPrefab);
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
