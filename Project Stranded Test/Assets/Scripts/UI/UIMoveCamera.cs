using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveCamera : MonoBehaviour {

    public GameObject sceneCamera;
    public GameObject targetObject;
    public float speed;
    bool shouldMove = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float step = speed * Time.deltaTime;

        if (shouldMove)
        {
            sceneCamera.transform.position = Vector3.MoveTowards(sceneCamera.transform.position, targetObject.transform.position, step);

            if (sceneCamera.transform.position == targetObject.transform.position)
            {
                shouldMove = false;
            }
        }     
    }

    public void MoveCameraToNewTarget(GameObject newTargetObject)
    {
        targetObject = newTargetObject;
        shouldMove = true;
    }

}
