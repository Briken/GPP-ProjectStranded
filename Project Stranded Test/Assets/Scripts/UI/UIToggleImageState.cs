using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIToggleImageState : MonoBehaviour {

    public Sprite imageActive;
    public Sprite imageInactive;

    bool isActive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Switch state bool and change the image (pretty useless until we have functionality)
    public void SwitchState()
    {
        if (isActive)
        {
            isActive = false;
            gameObject.GetComponent<Image>().sprite = imageInactive;
        }
        else
        {
            isActive = true;
            gameObject.GetComponent<Image>().sprite = imageActive;
        }
    }
}
