using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;

public class PhotonNetCode : Photon.PunBehaviour {

    public GameObject voteCards;
    public GameObject voteLoss;
    public GameObject connectionStatusOverlay;

    bool isMasterServer = false;
    GameObject spawnPoint;
    bool isActive = false;
    public GameObject gData;
    public int lobbyMax;
    //GameTimer timer;
    public GameObject player;
    public int playerNum;
    RoomOptions roomDetails;
    public bool radBound = false;
    TypedLobby typedLobby;
    int currentPlayers;
    string roomName;
    RoomData data;
    int pNum;

    public GameObject[] ships;

    
    //public GameObject mainCamera;
    

	// Use this for initialization
	void Start ()
    {
        Debug.Log(PhotonNetwork.connectionState);
        PhotonNetwork.Disconnect();
        if (GameObject.FindGameObjectWithTag("GameData") == null)
        {
            Instantiate(gData);
            data = GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>();
        }
        else
        {
            data = GameObject.FindGameObjectWithTag("GameData").GetComponent<RoomData>();
        }

        PhotonNetwork.sendRate = 20; 
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
        roomDetails = new RoomOptions();
        roomDetails.IsVisible = false;
        roomDetails.MaxPlayers = (byte)lobbyMax;
        typedLobby = new TypedLobby();

        //ships = GameObject.FindGameObjectsWithTag("Ship");
        
        GameObject netmanager = this.gameObject;
        //timer = netmanager.GetComponent<GameTimer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        //Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
        //debug.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    void OnGui()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        if (data.roomName != null)
        {
            roomName = data.roomName;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomDetails, typedLobby);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null, roomDetails, typedLobby);
    }

    void OnJoinedRoom()
    {
        isMasterServer = PhotonNetwork.playerList.Length == 1;

        if (PhotonNetwork.playerList.Length == roomDetails.MaxPlayers)
        {
         
            SpawnPlayer();
      //      timer.enabled = true;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        
       
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == "MainScene-Recovered")
        {
            SpawnPlayer();
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        
        if (PhotonNetwork.playerList.Length == roomDetails.MaxPlayers)
        {
            SpawnPlayer();
        }
        
    }


    void SpawnPlayer()
    {
        GameObject controlledPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);
        if (isMasterServer)
        {
            foreach(ResourceScript n in FindObjectsOfType<ResourceScript>())
            {
                int seed = Random.Range(0, int.MaxValue);
                n.photonView.RPC("ReceiveSeed", PhotonTargets.All, seed);
            }
        }

        pNum = controlledPlayer.GetComponent<PhotonView>().ownerId;
        Vector3 moveTo = ships[pNum - 1].transform.position;
        controlledPlayer.transform.position = moveTo;
        controlledPlayer.GetComponent<MovementScript>().photonView.RPC("SetNum", PhotonTargets.All, pNum);

        connectionStatusOverlay.SetActive(false);
    }

    

    
}
