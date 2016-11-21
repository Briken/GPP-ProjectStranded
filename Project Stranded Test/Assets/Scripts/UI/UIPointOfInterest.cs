using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// TO DO: Handle overlapping indicators
// TO DO: Fix the maths for determining position of the indicator

public class UIPointOfInterest : MonoBehaviour {

    Vector3 screenSpacePosition;
    Vector3 correctedScreenSpacePosition;

    public GameObject indicatorIcon;
    public Sprite indicatorArrowSprite;
    public Sprite indicatorTypeSprite;

    public float iconPaddingAmount = 0;
    public float heightAdjustmentAmount = 20;

    public GameObject gameCamera;
    Camera cameraObject;

    float lineGradient;

	// Use this for initialization
	void Start ()
    {
        Debug.Log("The screen size is: " + Screen.width.ToString() + " x " + Screen.height.ToString());

        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");

        cameraObject = gameCamera.GetComponent<Camera>();

        indicatorIcon = Instantiate(indicatorIcon);
	}
	
	// Update is called once per frame
	void Update ()
    {
        screenSpacePosition = cameraObject.WorldToScreenPoint(gameObject.transform.position);
        correctedScreenSpacePosition = new Vector3(screenSpacePosition.x - (Screen.width / 2), screenSpacePosition.y - (Screen.height / 2));

        // y = mx -> m = y/x
        lineGradient = correctedScreenSpacePosition.y / correctedScreenSpacePosition.x;     

        if (IsOffScreen())
        {
            indicatorIcon.gameObject.GetComponent<SpriteRenderer>().sprite = indicatorArrowSprite;
            // Debug.Log("Object is OFF SCREEN at: " + correctedScreenSpacePosition.ToString() + " with gradient: " + lineGradient.ToString() + " . Indicator position: " + DetermineIndicatorLocation().ToString());
        }
        else
        {
            DetermineIndicatorLocation();
            indicatorIcon.gameObject.GetComponent<SpriteRenderer>().sprite = indicatorTypeSprite;
            // Debug.Log("Object is ON SCREEN at: " + correctedScreenSpacePosition.ToString() + " with gradient: " + lineGradient.ToString() + " . Indicator position: " + DetermineIndicatorLocation().ToString());
        }

        // Rotate towards the target object
        Quaternion indicatorIconRotation = Quaternion.LookRotation(gameObject.transform.position - indicatorIcon.gameObject.transform.position, Vector3.back);

        // Change the rotation initially then clamp out the X and Y values (this might be a nasty way of appeasing the Quaternion gods?)
        indicatorIcon.gameObject.transform.rotation = indicatorIconRotation;
        indicatorIcon.gameObject.transform.rotation = Quaternion.Euler(0, 0, indicatorIcon.gameObject.transform.rotation.eulerAngles.z);

        indicatorIcon.gameObject.transform.position = cameraObject.ScreenToWorldPoint(new Vector3(DetermineIndicatorLocation().x + (Screen.width / 2), DetermineIndicatorLocation().y + (Screen.height / 2), 0));
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

    Vector3 DetermineIndicatorLocation()
    {
        // Used to construct the output vector
        float indicatorPositionX = 0;
        float indicatorPositionY = 0;

        // Determine where the offscreen target is and get relative position on the edge of the screen, clamping if required.
        // y = mx
        if (correctedScreenSpacePosition.x > (Screen.width / 2))
        {
            indicatorPositionX = (Screen.width / 2) - iconPaddingAmount;
            indicatorPositionY = Mathf.Clamp(lineGradient * (Screen.width / 2), -(Screen.height / 2) + iconPaddingAmount, (Screen.height / 2) - iconPaddingAmount);        
        }
        else if (correctedScreenSpacePosition.x < -(Screen.width / 2))
        {
            indicatorPositionX = -(Screen.width / 2) + iconPaddingAmount;
            indicatorPositionY = Mathf.Clamp(lineGradient * -(Screen.width / 2), -(Screen.height / 2) + iconPaddingAmount, (Screen.height / 2) - iconPaddingAmount);           
        }
        else
        {
            indicatorPositionY = Mathf.Clamp(correctedScreenSpacePosition.y + heightAdjustmentAmount, -(Screen.height / 2) + iconPaddingAmount, (Screen.height / 2) - iconPaddingAmount);
            indicatorPositionX = Mathf.Clamp(indicatorPositionY / lineGradient, -(Screen.width / 2) + iconPaddingAmount, (Screen.width / 2) - iconPaddingAmount);
        }

        Vector3 indicatorLocation = new Vector3(indicatorPositionX, indicatorPositionY, 0);

        return indicatorLocation; 
    }

    void OnDestroy()
    {
        Destroy(indicatorIcon);
    }
}
