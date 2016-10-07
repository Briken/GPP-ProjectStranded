using UnityEngine;
using System.Collections;

public class UICanvasSwitching : MonoBehaviour {

    public GameObject[] canvassesToHide;
    public GameObject[] canvassesToShow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Shows and hides canvasses depending on what array they're in
    public void SwitchCanvasses()
    {
        foreach (GameObject canvasToHide in canvassesToHide)
        {
            canvasToHide.SetActive(false);
        }
        
        foreach (GameObject canvasToShow in canvassesToShow)
        {
            canvasToShow.SetActive(true);
        }
    }
}
