using UnityEngine;
using System.Collections;

public class TeamScript : MonoBehaviour {

    public int playerCount;

    public GameObject[] totalPlayers;
    public GameObject[] team1;
    public GameObject[] team2;


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getPlayerNum(GameObject player)
    {
        for (int x = 0; x < totalPlayers.GetLength(0); x++)
        {
            if (totalPlayers[x] == null)
            {
                totalPlayers[x] = player;
                return x;
            }
        }
        return 0;
    }
    

    public int AddPlayer(int player, GameObject me)
    {
        if (player == 0)
        { 
            playerCount++;
            team1[player] = me;
            return 1;
        }

        if (player % 2 == 0)
        {
            playerCount++;
            team1[player] = me;
            return 1;
        }

        else //if (playerCount % 2 != 0)
        {
            playerCount++;
            team2[player] = me;
            return 2;
        }
    }
}
