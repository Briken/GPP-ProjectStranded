using UnityEngine;
using System.Collections;

public class PlayerColourChanger : MonoBehaviour {

    public Material[] playerMaterials;
    GameObject[] playerCharacters;

	// Use this for initialization
	void Start ()
    {
        playerCharacters = GameObject.FindGameObjectsWithTag("Player Body");

        for(int i = 0; i < playerCharacters.Length; i++)
        {
            playerCharacters[i].GetComponent<MeshRenderer>().material = playerMaterials[i];
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
