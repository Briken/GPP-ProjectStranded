using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class GameTimer : Photon.PunBehaviour {


    public GameObject resourceDepot;
    public float timer;
    public Text UITimer;
    // Use this for initialization
    List<GameObject> timeCheck;
    GameTimer mainTime;
    public GameObject roomOwner;
    public GameObject gameData;

    void Start ()
    {
        gameData = GameObject.Find("GameData");
        resourceDepot = GameObject.Find("ResourceDepot");
        timer = 300;
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetComponent<MovementScript>().playerNum == 1)
            {
                Debug.Log("player" + n.GetComponent<MovementScript>().playerNum.ToString() + "time being used");
                roomOwner = n;
               // timer = n.GetComponent<GameTimer>().timer;
            }
            
        }
        //mainTime = timeCheck.GetComponent<GameTimer>();
        //gameTime = mainTime.timer;
        Debug.Log("time left: " + timer.ToString());

     //   photonView.RPC("SetTimer", PhotonTargets.All, timer);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (this.gameObject == roomOwner)
        {
            timer -= Time.deltaTime;
        }
        if (roomOwner == null)
        {
            roomOwner = GameObject.Find("NetworkManager");
            //foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
            //{
            //    if (n.GetComponent<MovementScript>().playerNum == 1)
            //    {
            //        roomOwner = n;
            //    }
            //}
        }
        if (this.gameObject != roomOwner && roomOwner !=null)
        {
            timer = roomOwner.GetComponent<GameTimer>().timer;
          //  Debug.Log(timer.ToString());
        }
        //photonView.RPC("SetTimer", PhotonTargets.All);
        if (timer <= 0)
        {
            Debug.Log("time is less that or equal to 0");
            timeEnds();
        }
    }


    public void timeEnds()
    {
        SceneManager.LoadScene("MainScene-Recovered");
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Ship"))
        {
            gameData.GetComponent<RoomData>().RecordScores(n.GetComponent<ShipScript>().shipNum, n.GetComponent<ShipScript>().totalFuel);
            timer = 300;
        }
    }

    [PunRPC]
    float SetTimer(float masterTime)
    {
        if (PhotonNetwork.player.ID == 0)
        {
            masterTime = timer;
        }
        else
        {
            timer = masterTime;
        }

        Debug.Log(timer.ToString());

        return timer;
        
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(timer);
        }
        else
        {
            this.timer = (float)stream.ReceiveNext();
        }
    }
}
