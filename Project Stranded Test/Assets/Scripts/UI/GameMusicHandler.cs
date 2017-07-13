using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicHandler : MonoBehaviour {

    float defaultVolume;
    public float timeRunOutPitch = 1.3f;
    public float timeRunOutVolumeBoost = 0.05f;

	// Use this for initialization
	void Start ()
    {
        defaultVolume = gameObject.GetComponent<AudioSource>().volume;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer < GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIMainGameHandler>().timeToDisplayCountdown 
            && GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer > 0)
        {
            gameObject.GetComponent<AudioSource>().pitch = timeRunOutPitch;
            gameObject.GetComponent<AudioSource>().volume = defaultVolume + timeRunOutVolumeBoost;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().pitch = 1.0f;
            gameObject.GetComponent<AudioSource>().volume = defaultVolume;
        }
	}

    public void BeginMatchPlay()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
