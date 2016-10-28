using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class ResourceDepot : Photon.PunBehaviour
{

   
    public int team1Score, team2Score;
    public Text scores;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTeamResource(GameObject player)
    {
        if (player.GetComponent<MovementScript>().team == 1) //if the player is on team1
        {
            team1Score += player.GetComponent<PlayerResource>().resource; //increase team1s score by the players score
            player.GetComponent<PlayerResource>().resource = 0;  //set the players score to 0
            scores.text = ("Team1: " + team1Score + "\n" + "Team2: " + team2Score);
        }

        if (player.GetComponent<MovementScript>().team == 2) //if the player is on team2
        {
            team2Score += player.GetComponent<PlayerResource>().resource; //increase team2s score by the players score
            player.GetComponent<PlayerResource>().resource = 0; //set the players score to 0
            scores.text = ("Team1: " + team1Score + "\n" + "Team2: " + team2Score);
        }

    }
}
