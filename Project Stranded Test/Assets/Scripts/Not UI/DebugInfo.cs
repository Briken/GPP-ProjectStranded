using UnityEngine;
using System.Collections;
using Photon;

public class DebugInfo : PunBehaviour {

    float delta = 0.0f;
    float fps = 0.0f;
    public UnityEngine.UI.Text fpsCount;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        delta += Time.deltaTime;
        delta /= 2.0f;
        fps = 1.0f / delta;
        fpsCount.text = "FPS: " + fps.ToString() + "\n" + "Ping: " + PhotonNetwork.networkingPeer.RoundTripTime.ToString() + "  ms";
    }
}
