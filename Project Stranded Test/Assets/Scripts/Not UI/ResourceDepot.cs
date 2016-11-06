using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class ResourceDepot : Photon.PunBehaviour
{

    PhotonStream photonStream;
    public int team1Score, team2Score;
    public Text scores;
    public int maxResources = 200;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (team1Score >= maxResources)
        {
            Application.LoadLevel("TEAM1WIN");
        }
        if (team2Score>=maxResources)
        {
            Application.LoadLevel("TEAM2WIN");
        }
    }

    [PunRPC]
    public void AddTeamResource(int pteam, int resource)
    {
         if (pteam == 1) //if the player is on team1
        {
            team1Score += resource; //increase team1s score by the players score
            
            scores.text = ("Team1: " + team1Score + "\n" + "Team2: " + team2Score);
            //photonSteam = new PhotonStream(true, );
            //photonSteam.Serialize(ref team1Score);
            //photonSteam.
        }

        if (pteam == 2) //if the player is on team2
        {
            team2Score += resource; //increase team2s score by the players score
            scores.text = ("Team1: " + team1Score + "\n" + "Team2: " + team2Score);
        }

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(team1Score);
            stream.SendNext(team2Score);
        }
        else
        {
            this.team1Score = (int)stream.ReceiveNext();
            this.team2Score = (int)stream.ReceiveNext();
        }
        Debug.Log("Called OnPhotonSerializeView");
    }
}
