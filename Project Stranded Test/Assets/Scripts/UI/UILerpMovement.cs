using UnityEngine;
using System.Collections;

public class UILerpMovement : MonoBehaviour {

    // Initial and target values:
    Vector3 activeLocationAdjustment;
    Vector3 inactiveLocation;
    Vector3 activeScale;
    public Vector3 inactiveScale;

    // Data for lerping objects:
    public GameObject objectLocationToTarget;
    public float speed = 10.0f;
    bool shouldLerpObject;
    bool shouldLerpObjectReversed;
    private float startTime;
    private float distanceToTravel;
    float distanceCovered;
    float fracJourney;

    public bool deactivateAfterInitialValues;
    // Use this for initialization
    void Start ()
    {
        // Get our initial values
        activeLocationAdjustment = new Vector3((gameObject.transform.position.x - objectLocationToTarget.transform.position.x), (gameObject.transform.position.y - objectLocationToTarget.transform.position.y), (gameObject.transform.position.z - objectLocationToTarget.transform.position.z));
        float distance = activeLocationAdjustment.magnitude;
        inactiveLocation = objectLocationToTarget.gameObject.transform.position;
        activeScale = gameObject.transform.localScale;

        if (deactivateAfterInitialValues)
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Maths, timing and stuff for lerping:
        if (shouldLerpObject)
        {
            if (gameObject.transform.position != objectLocationToTarget.gameObject.transform.position + activeLocationAdjustment)
            {
                distanceToTravel = Vector3.Distance(objectLocationToTarget.gameObject.transform.position, objectLocationToTarget.gameObject.transform.position + activeLocationAdjustment);
                distanceCovered = (Time.time - startTime) * speed;
                fracJourney = distanceCovered / distanceToTravel;

                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, objectLocationToTarget.gameObject.transform.position + activeLocationAdjustment, fracJourney);
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, activeScale, fracJourney);
            }
            else
            {
                shouldLerpObject = false;

                Debug.Log("Should Lerp: FALSE");
            }
        }

        if (shouldLerpObjectReversed)
        {
            if (gameObject.transform.position != objectLocationToTarget.gameObject.transform.position)
            {
                distanceToTravel = Vector3.Distance(objectLocationToTarget.gameObject.transform.position, objectLocationToTarget.gameObject.transform.position + activeLocationAdjustment);
                distanceCovered = (Time.time - startTime) * speed;
                fracJourney = distanceCovered / distanceToTravel;

               gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, objectLocationToTarget.gameObject.transform.position, fracJourney);
               gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, inactiveScale, fracJourney);
            }
            else
            {
                shouldLerpObjectReversed = false;

                Debug.Log("Should Lerp Reversed: FALSE");
            }
        }

        Debug.DrawLine(objectLocationToTarget.transform.position + activeLocationAdjustment, objectLocationToTarget.transform.position, Color.red, 1, false);
    }

    public void ActivateLerp()
    {
        inactiveLocation = objectLocationToTarget.gameObject.transform.position;
        startTime = Time.time;
        

        shouldLerpObject = true;
        gameObject.transform.position = inactiveLocation;
        gameObject.transform.localScale = inactiveScale;

        Debug.Log("FORWARD LERP");
    }

    public void ReverseLerp()
    {
        inactiveLocation = objectLocationToTarget.gameObject.transform.position;
        startTime = Time.time;

        shouldLerpObjectReversed = true;

        Debug.Log("REVERSE LERP");
    }

    void ResetValues()
    {

    }
}
