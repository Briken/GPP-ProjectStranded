using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour {

    Vector3 fleePoint;
    public Text debug;
    public Text squareloc;
    bool dog = false;

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

        squareloc.text = transform.position.ToString();
            
    }

    // Update is called once per frame
    void Update()
    {
        if (dog == false)
        {
            debug.text = "dancey dance";
            dog = true;
        }

        
        if (Input.GetButtonDown("Fire1"))
        {
            debug.text = "fire1 pressed";
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                debug.text = "rayhit";
                if (hit.collider.gameObject.tag == "Player")
                {
                    debug.text = "STOP TOUCHING MEEEEEEE";
                }
            }
        }


        foreach (Touch move in Input.touches)
        {
            int id = move.fingerId;

            Vector3 screenPos = new Vector3(Input.GetTouch(id).position.x,Input.GetTouch(id).position.y, 0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            squareloc.text = worldPos.ToString();

            Ray ray = Camera.main.ScreenPointToRay(worldPos);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                debug.text = "rayhit";
                if (hit.collider.gameObject.tag == "Player")
                {
                    debug.text = "STOP TOUCHING MEEEEEEE";
                }
            }
        }
     }

    void MoveFromTouch(Vector3 targetPoint, Vector3 velocity)
    {
        
    }

    void OnGUI()
    {

        foreach (Touch touch in Input.touches)
        {
            string message = "";
            message += "ID: " + touch.fingerId + "\n";
            message += "Phase: " + touch.phase.ToString() + "\n";
            message += "TapCount: " + touch.tapCount + "\n";
            message += "Pos X: " + touch.position.x + "\n";
            message += "Pos Y: " + touch.position.y + "\n";

            int num = touch.fingerId;
            GUI.Label(new Rect(0 + 130 * num, 0, 200, 180), message);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
