using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInternetConnection : MonoBehaviour {

    public bool hasInternetConnectivity = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            hasInternetConnectivity = false;
            GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIErrorMessage>().DisplayErrorMessage("NETWORK ERROR", "INTERNET CONNECTION HAS BEEN LOST");
        }
        else
        {
            hasInternetConnectivity = true;
        }
    }
}
