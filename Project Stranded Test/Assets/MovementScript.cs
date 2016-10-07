using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour {

    Vector3 fleePoint;
    public Text debug;

	// Use this for initialization
	void Start ()
    {

#if UNITY_EDITOR
        {
            debug.text = "Unity Editor";
        }
#endif

#if UNITY_ANDROID
        {
            debug.text = "I'm Running on Android";
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetTouch[0]) 
       // {
            fleePoint = Input.GetTouch(0).position;
            if (fleePoint.x == transform.position.x)
            {
                if (fleePoint.y == transform.position.y)
                {
                    debug.text = "StopTouchingMEEEEEEEE";
                }
            }
      //  }
        //if (moveTouch)
        //{
        //    Debug.Log("Stop touching meeeee!");

        //    fleePoint = moveTouch.position;
        //}
    }

    void MoveFromTouch(Vector3 targetPoint, Vector3 velocity)
    {
        
    }

    void OnGUI()
    {

        foreach (Touch touch in Input.touches)
        {
            string message = "";
            message += "ID: " + touch.fingerId + "/n";
            message += "Phase: " + touch.phase.ToString() + "/n";
            message += "TapCount: " + touch.tapCount + "/n";
            message += "Pos X: " + touch.position.x + "/n";
            message += "Pos Y: " + touch.position.y + "/n";

            int num = touch.fingerId;
            GUI.Label(new Rect(0 + 130 * num, 0, 120, 100), message);
        }
    }

}
