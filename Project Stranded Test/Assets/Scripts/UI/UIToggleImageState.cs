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
            // Check if the object has a SpriteRenderer component or an Image component and change appropriately
            if (gameObject.GetComponent(typeof(SpriteRenderer)) != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = imageInactive;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = imageInactive;
            }

            isActive = false;
        }
        else
        {
            if (gameObject.GetComponent(typeof(SpriteRenderer)) != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = imageActive;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = imageActive;
            }

            isActive = true;
        }
    }
}
