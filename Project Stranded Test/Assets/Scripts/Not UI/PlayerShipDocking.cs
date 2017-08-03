using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipDocking : MonoBehaviour {

    public GameObject dockObject;
    public GameObject dockPrompt;
    public GameObject[] dockTextObjects;
    public GameObject[] dockArrowObjects;
    public GameObject dockFuelPipe;

	// Use this for initialization
	void Start ()
    {
        // dockPrompt.SetActive(false);
        dockFuelPipe.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void ChangePromptColour(Color newColour)
    {
        foreach (GameObject dockTextObject in dockTextObjects)
        {
            dockTextObject.GetComponent<Text>().color = newColour;
        }

        foreach (GameObject dockArrowObject in dockArrowObjects)
        {
            dockArrowObject.GetComponent<SpriteRenderer>().color = newColour;
        }
    }
}
