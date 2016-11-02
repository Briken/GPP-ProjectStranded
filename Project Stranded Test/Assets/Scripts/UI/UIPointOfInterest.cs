using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPointOfInterest : MonoBehaviour {

    Vector3 screenSpacePosition;
    Vector3 correctedScreenSpacePosition;

    public GameObject gameCamera;
    Camera cameraObject;

    float lineGradient;
	// Use this for initialization
	void Start ()
    {
        Debug.Log("The screen size is: " + Screen.width.ToString() + " x " + Screen.height.ToString());

        cameraObject = gameCamera.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        screenSpacePosition = cameraObject.WorldToScreenPoint(gameObject.transform.position);
        correctedScreenSpacePosition = new Vector3(screenSpacePosition.x - (Screen.width / 2), screenSpacePosition.y - (Screen.height / 2));
        // Debug.Log(screenSpacePosition.ToString());

        

        if (IsOffScreen())
        {
            Debug.Log("Object is OFF SCREEN at: " + correctedScreenSpacePosition.ToString() + " with gradient: " +(correctedScreenSpacePosition.y / correctedScreenSpacePosition.x).ToString());
        }
        else
        {
            Debug.Log("Object is ON SCREEN at: " + correctedScreenSpacePosition.ToString() + " with gradient: " + (correctedScreenSpacePosition.y / correctedScreenSpacePosition.x).ToString());
        }

    }

    bool IsOffScreen()
    {
        if (screenSpacePosition.x > 0 &&
            screenSpacePosition.x < Screen.width &&
            screenSpacePosition.y > 0 &&
            screenSpacePosition.y < Screen.height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
