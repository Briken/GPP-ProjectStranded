using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInformationBar : MonoBehaviour {

    public GameObject informationBarText;

    bool isActive = false;
    float activeTimer = 0.0f;
    Color defaultBarColour;

	// Use this for initialization
	void Start ()
    {
        // gameObject.GetComponent<UILerpMovement>().ReverseLerp();

        defaultBarColour = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        informationBarText.GetComponent<Text>().text = System.String.Empty;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (activeTimer > 0)
        {
            gameObject.GetComponent<Image>().color = defaultBarColour;
            activeTimer -= Time.deltaTime;
        }

        if (isActive && (activeTimer < 0))
        {
            // gameObject.GetComponent<UILerpMovement>().ReverseLerp();
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            informationBarText.GetComponent<Text>().text = System.String.Empty;
            isActive = false;
        }
	}

    // Mainly use this if you want to display information!
    public void DisplayInformationForSetTime(string message, float time)
    {
        informationBarText.GetComponent<Text>().text = message;
        activeTimer = time;

        // Workaround so the information box doesn't disappear if only the text has been updated
        gameObject.GetComponent<UILerpMovement>().debugStuckTimer += time;

        if (!isActive)
        {
            // gameObject.GetComponent<UILerpMovement>().ActivateLerp();
            isActive = true;
        }   
    }

    // Manual methods:
    public void DisplayInformation(string message)
    {
        informationBarText.GetComponent<Text>().text = message;

        // gameObject.GetComponent<UILerpMovement>().ActivateLerp();
    }

    public void HideInformation()
    {
        // gameObject.GetComponent<UILerpMovement>().ReverseLerp();
    }
}
