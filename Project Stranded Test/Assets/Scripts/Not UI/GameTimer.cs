using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class GameTimer : Photon.PunBehaviour {


    
    public GameObject resourceDepot;
    public float timer = 300;
    public float startTime;
    bool timerSet= false;

    bool called;
    // Use this for initialization
    List<GameObject> timeCheck;
    GameTimer mainTime;
    public GameObject roomOwner;
    public GameObject scoreData;

    void Start ()
    {
        timer = startTime;
        EventManager.Reset += Reset;
        scoreData = GameObject.FindGameObjectWithTag("ScoreData");
        resourceDepot = GameObject.Find("ResourceDepot");
        timerSet = true; 
        Debug.Log("time left: " + timer.ToString());

     //   photonView.RPC("SetTimer", PhotonTargets.All, timer);
	}

    // Update is called once per frame
    void Update()
    {

        Debug.Log(timer + " is the amoutn left");
        if (timerSet)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && called == false)
            {
                Debug.Log("time is less that or equal to 0");
                called = true;
                timeEnds();
            }
        }      
    }


    public void timeEnds()
    {
        
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Ship"))
        {
            scoreData.GetComponent<ScoreCount>().RecordScores(n.GetComponent<ShipScript>().shipNum, n.GetComponent<ShipScript>().totalFuel);
        }
        scoreData.GetComponent<ScoreCount>().LoadNextRound();
    }

    //[PunRPC]
    //float SetTimer(float masterTime)
    //{
    //    if (PhotonNetwork.player.ID == 0)
    //    {
    //        masterTime = timer;
    //    }
    //    else
    //    {
    //        timer = masterTime;
    //    }

    //    Debug.Log(timer.ToString());

    //    return timer;
        
    //}

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
    private void Reset()
    {
        timer = startTime;
        Debug.Log("RESETR CALLED");
    }
    
}
