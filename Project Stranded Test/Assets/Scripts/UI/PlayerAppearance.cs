using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class PlayerAppearance : Photon.PunBehaviour {

    public GameObject playerHead;
    public GameObject playerBody;
    public InputField playerUsername;

    public Sprite[] playerHeadSprites;
    public Sprite[] playerBodySprites;

    public int playerHeadNumber;
    public int playerBodyNumber;

    public GameObject mainMenuUsernameDisplay;

    public int randomPlayerHeadNumber;
    public int randomPlayerBodyNumber;

	// Use this for initialization
	void Start ()
    {
        playerHeadNumber = PlayerPrefs.GetInt("Player Head");
        playerBodyNumber = PlayerPrefs.GetInt("Player Body"); 

        randomPlayerHeadNumber = Random.Range(0, playerHeadSprites.Length - 1);
        randomPlayerBodyNumber = Random.Range(0, playerBodySprites.Length - 1);

        // Adjust values if player parts match same value
        if (playerHeadNumber == randomPlayerHeadNumber)
        {
            if (randomPlayerHeadNumber == playerHeadSprites.Length - 1)
            {
                randomPlayerHeadNumber = 0;
            }
            else
            {
                randomPlayerHeadNumber++;
            }
        }

        if (playerBodyNumber == randomPlayerBodyNumber)
        {
            if (randomPlayerBodyNumber == playerBodySprites.Length - 1)
            {
                randomPlayerBodyNumber = 0;
            }
            else
            {
                randomPlayerBodyNumber++;
            }
        }

        // Send random number to each other player so they're synced
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerAppearance>().randomPlayerBodyNumber = randomPlayerBodyNumber;
            player.GetComponent<PlayerAppearance>().randomPlayerHeadNumber = randomPlayerHeadNumber;
        }

    }

    // Update is called once per frame
    void Update ()
    {
        playerHead.gameObject.GetComponent<SpriteRenderer>().sprite = playerHeadSprites[playerHeadNumber];

        if (photonView != null)
        {
            // Update networked player appearance based on player ownership
            if (photonView.isMine)
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
            else
            {
                playerHead.gameObject.GetComponent<SpriteRenderer>().sprite = playerHeadSprites[randomPlayerHeadNumber];

                // In the case that the script is attached to the root sprite
                if (playerBody == null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = playerBodySprites[randomPlayerBodyNumber];
                }
                else
                {
                    playerBody.GetComponent<SpriteRenderer>().sprite = playerBodySprites[randomPlayerBodyNumber];
                }

            }
        }
        else
        {
            playerHead.gameObject.GetComponent<SpriteRenderer>().sprite = playerHeadSprites[playerHeadNumber];

            if (playerBody == null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyNumber];
            }
            else
            {
                playerBody.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyNumber];
            }
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

        // Prevents the player from saving with no name
        if (playerUsername.text != "")
        {
            PlayerPrefs.SetString("Username", playerUsername.text);
        }
        
        // Manually reloads the username after saving
        GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>().ManualUsernameLoad();

        // Quick workaround for updating the username string on main menu following UI merge
        mainMenuUsernameDisplay.gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("Username");
    }
}
