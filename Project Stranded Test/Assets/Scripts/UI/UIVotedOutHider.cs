using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVotedOutHider : MonoBehaviour {

    float remainingDisplayTime;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (remainingDisplayTime > 0.0f)
        {
            remainingDisplayTime -= Time.deltaTime;
        }

		if (remainingDisplayTime <= 0.0f)
        {
            gameObject.SetActive(false);
        }
	}

    public void DisplayVotedOut(float displayDuration)
    {
        remainingDisplayTime = displayDuration;
    }
}
