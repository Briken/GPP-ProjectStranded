using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class GameTimer : Photon.PunBehaviour {


    
    public GameObject resourceDepot;
    public float timer;
    
    bool called;
    // Use this for initialization
    List<GameObject> timeCheck;
    GameTimer mainTime;
    public GameObject roomOwner;
    public GameObject gameData;

    void Start ()
    {
        gameData = GameObject.FindGameObjectWithTag("GameData");
        resourceDepot = GameObject.Find("ResourceDepot");
        timer = 300;
      
        //mainTime = timeCheck.GetComponent<GameTimer>();
        //gameTime = mainTime.timer;
        Debug.Log("time left: " + timer.ToString());

     //   photonView.RPC("SetTimer", PhotonTargets.All, timer);
	}

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;


        if (timer <= 0 && called == false)
        {
            Debug.Log("time is less that or equal to 0");
            called = true;
            timeEnds();
        }
    }


    public void timeEnds()
    {
        
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Ship"))
        {
            gameData.GetComponent<RoomData>().RecordScores(n.GetComponent<ShipScript>().shipNum, n.GetComponent<ShipScript>().totalFuel);
        }
        timer = 300;
        gameData.GetComponent<RoomData>().LoadNextRound();
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
