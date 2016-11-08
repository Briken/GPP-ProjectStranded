using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class GameTimer : Photon.PunBehaviour {


    
    public float timer;
    public Text UITimer;
    // Use this for initialization
    List<GameObject> timeCheck;
    GameTimer mainTime;
    public GameObject roomOwner;
    void Start ()
    {
        timer = 300.0f;
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
        photonView.RPC("SetTimer", PhotonTargets.All, timer);
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
            foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (n.GetComponent<MovementScript>().playerNum == 1)
                {
                    roomOwner = n;
                }
            }
        }
        if (this.gameObject != roomOwner)
        {
            timer = roomOwner.GetComponent<GameTimer>().timer;
            Debug.Log(timer.ToString());
        }
        //photonView.RPC("SetTimer", PhotonTargets.All);
        //if (gameTime <= 0)
        //{
        //    timeEnds();
        //}
    }


    public void timeEnds()
    {
        
    }

    [PunRPC]
    float SetTimer(float masterTime)
    {
        if (this.gameObject.GetComponent<MovementScript>().playerNum == 1)
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
