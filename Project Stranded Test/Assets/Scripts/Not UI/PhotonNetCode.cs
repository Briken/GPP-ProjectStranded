using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class PhotonNetCode : Photon.PunBehaviour {

    TeamScript team;
    public GameObject player;
    bool team1 = true;

    // Use this for initialization
    void Start()
    {
        team = GetComponent<TeamScript>();
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnGui()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
        team.TeamAdd(player, team1);
        Debug.Log(team1);
        team1 = toggle(team1);
        Debug.Log(team1);
    }
    
    bool toggle(bool t)
    {
        if (t == true)
        {
            t = false;
            return t;    
        }
        else
        {
            t = true;
            return t;
        }
    }
}
