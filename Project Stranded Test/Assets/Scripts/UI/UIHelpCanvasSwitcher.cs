using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelpCanvasSwitcher : MonoBehaviour {

    public GameObject[] canvasObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchToCanvas(GameObject canvasToShow)
    {
        foreach (GameObject canvasObject in canvasObjects)
        {
            if (canvasToShow == canvasObject)
            {
                canvasObject.SetActive(true);
            }
            else
            {
                canvasObject.SetActive(false);
            }
        }
    }
}
