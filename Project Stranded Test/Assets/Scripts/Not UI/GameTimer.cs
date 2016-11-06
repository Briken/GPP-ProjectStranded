using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon;

public class GameTimer : Photon.PunBehaviour {


    public float gameTime = 300.0f;
    float timer;
    public Text UITimer;
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameTime -= Time.deltaTime;
        photonView.RPC("SetTimer", PhotonTargets.All);
        if (gameTime <= 0)
        {
            timeEnds();
        }
	}


    public void timeEnds()
    {
        
    }

    [PunRPC]
    void SetTimer()
    {
        timer = gameTime;
        //UITimer.text = timer.ToString();
        Debug.Log(timer.ToString());
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
