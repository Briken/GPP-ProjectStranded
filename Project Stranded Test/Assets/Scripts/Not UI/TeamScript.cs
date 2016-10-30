using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamScript : MonoBehaviour {

    public int playerCount;

    bool teamsFilled = false;

    public GameObject[] totalPlayers = new GameObject[8];
    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (totalPlayers[1] != null && teamsFilled == false)
        {
            SetTeamLists();
        }
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

            return 1;
        }

        if (player % 2 == 0)
        {
            playerCount++;

            return 1;
        }

        else //if (playerCount % 2 != 0)
        {
            playerCount++;

            return 2;
        }
    }

    void SetTeamLists()
    {
        foreach (GameObject p in totalPlayers)
        {
            if (p == null) { continue; }
            if (p.GetComponent<MovementScript>().team == 1)
            {
                team1.Add(p);
            }

            if (p.GetComponent<MovementScript>().team == 2)
            {
                team2.Add(p);
            }
        }
        teamsFilled = true;
    }
}
