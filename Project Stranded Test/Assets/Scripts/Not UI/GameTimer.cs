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
        EventManager.Reset += ResetThis;
        scoreData = GameObject.FindGameObjectWithTag("ScoreData");
        resourceDepot = GameObject.Find("ResourceDepot");
        timerSet = true; 

     //   photonView.RPC("SetTimer", PhotonTargets.All, timer);
	}

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(timer + " is the amoutn left");
        if (timerSet)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && called == false)
            {
                called = true;
                timeEnds();
            }
        }      
    }


    public void timeEnds()
    {
        scoreData.GetComponent<ScoreCount>().RecordScores();
        scoreData.GetComponent<ScoreCount>().LoadNextRound();
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
    private void ResetThis()
    {
        timer = startTime;
        called = false;
    }
    
}
