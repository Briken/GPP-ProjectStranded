using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInformationBar : MonoBehaviour {

    public GameObject informationBarText;

    bool isActive = false;
    float activeTimer = 0;

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<UILerpMovement>().ReverseLerp();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (activeTimer > 0)
        {
            activeTimer -= Time.deltaTime;
        }

        if (isActive && (activeTimer < 0))
        {
            gameObject.GetComponent<UILerpMovement>().ReverseLerp();
            isActive = false;
        }
	}

    // Mainly use this if you want to display information!
    public void DisplayInformationForSetTime(string message, float time)
    {
        informationBarText.GetComponent<Text>().text = message;
        activeTimer = time;
        
        if (!isActive)
        {
            gameObject.GetComponent<UILerpMovement>().ActivateLerp();
            isActive = true;
        }   
    }

    // Manual methods:
    public void DisplayInformation(string message)
    {
        informationBarText.GetComponent<Text>().text = message;

        gameObject.GetComponent<UILerpMovement>().ActivateLerp();
    }

    public void HideInformation()
    {
        gameObject.GetComponent<UILerpMovement>().ReverseLerp();
    }
}
