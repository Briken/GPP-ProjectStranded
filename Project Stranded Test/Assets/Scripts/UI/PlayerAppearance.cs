using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour {

    public GameObject playerHead;
    public GameObject playerBody;

    public Sprite[] playerHeadSprites;
    public Sprite[] playerBodySprites;

    public int playerHeadNumber;
    public int playerBodyNumber;

	// Use this for initialization
	void Start ()
    {
        playerHeadNumber = PlayerPrefs.GetInt("Player Head");
        playerBodyNumber = PlayerPrefs.GetInt("Player Body");
    }
	
	// Update is called once per frame
	void Update ()
    {
        playerHead.gameObject.GetComponent<SpriteRenderer>().sprite = playerHeadSprites[playerHeadNumber];

        // In the case that the script is attached to the root sprite
        if (playerBody == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyNumber];
        }
        else
        {
            playerBody.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyNumber];
        }
        

	}

    public void CycleNextHeadForward()
    {
        if (playerHeadNumber == playerHeadSprites.Length - 1)
        {
            playerHeadNumber = 0;
        }
        else
        {
            playerHeadNumber++;
        }
        
    }

    public void CycleNextHeadBack()
    {
        if (playerHeadNumber == 0)
        {
            playerHeadNumber = playerHeadSprites.Length - 1;
        }
        else
        {
            playerHeadNumber--;
        }
    }

    public void CycleNextBodyForward()
    {
        if (playerBodyNumber == playerBodySprites.Length - 1)
        {
            playerBodyNumber = 0;
        }
        else
        {
            playerBodyNumber++;
        }

    }

    public void CycleNextBodyBack()
    {
        if (playerBodyNumber == 0)
        {
            playerBodyNumber = playerBodySprites.Length - 1;
        }
        else
        {
            playerBodyNumber--;
        }
    }

    public void SaveCharacterPreferences()
    {
        PlayerPrefs.SetInt("Player Body", playerBodyNumber);
        PlayerPrefs.SetInt("Player Head", playerHeadNumber);
    }
}
