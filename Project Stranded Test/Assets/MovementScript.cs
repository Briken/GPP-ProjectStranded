using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour {


    public float maxSteering = 50.0f;
    public float maxSpeed = 50;
    public float currentSpeed = 10;
    protected Rigidbody rBody;
    Vector3 fleePoint;
    public Text debug;
    public Text squareloc;
    bool dog = false;

	// Use this for initialization
	void Start ()
    {
        rBody = GetComponent<Rigidbody>();

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
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                debug.text = "rayhit";
                if (hit.collider.gameObject.tag == "Player")
                {
                    debug.text = "STOP TOUCHING MEEEEEEE";
                }
            }
            else
            {
                Vector2 currentVelocity = rBody.velocity;
                currentVelocity += MoveFromTouch(target, currentVelocity);
                debug.text = "moving";
            }
        }


     //   foreach (Touch move in Input.touches)
     //   {
     //       int id = move.fingerId;

     //       Vector3 screenPos = new Vector3(Input.GetTouch(id).position.x,Input.GetTouch(id).position.y, 0);
     //       Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
     //       squareloc.text = worldPos.ToString();

     //       Ray ray = Camera.main.ScreenPointToRay(worldPos);
     //       RaycastHit hit = new RaycastHit();

     //       if (Physics.Raycast(ray, out hit))
     //       {
     //           debug.text = "rayhit";
     //           if (hit.collider.gameObject.tag == "Player")
     //           {
     //               debug.text = "STOP TOUCHING MEEEEEEE";
     //           }
     //       }
     //   }
     }

    Vector2 MoveFromTouch(Vector2 targetPoint, Vector2 velocity)
    {
        Vector2 desiredVel = targetPoint - new Vector2(transform.position.x, transform.position.y);
        desiredVel.Normalize();

        Vector3 target = targetPoint;
        Vector3 distance = (target - transform.position);
        currentSpeed = distance.magnitude;
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        desiredVel *= currentSpeed;

        currentSpeed = 50.0f;

        Vector2 steeringVel = desiredVel - velocity;

        steeringVel *= (1.0f / rBody.mass);
        steeringVel = LimitSteering(steeringVel);

        return steeringVel;
     }

    protected Vector2 LimitSteering(Vector2 SteeringVelocity)
    {
        if (SteeringVelocity.sqrMagnitude > maxSteering*maxSteering)
        {
            SteeringVelocity.Normalize();
            SteeringVelocity *= maxSteering;
        }
        return SteeringVelocity;
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
