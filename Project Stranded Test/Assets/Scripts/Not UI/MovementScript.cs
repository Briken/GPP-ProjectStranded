using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MovementScript : MonoBehaviour
{

    UISpiderButton menu;
   
    public float maxSteering = 50.0f;
    public float maxSpeed = 50;
    float currentSpeed = 10;
    protected Rigidbody rBody;
    Vector3 fleePoint;
  //  public Text debug;
  //  public Text squareloc;
    bool moving = false;

    PhotonView pv;

    // Use this for initialization
    void Start()
    {
        menu = GetComponent<UISpiderButton>();
        rBody = GetComponent<Rigidbody>();
        pv = PhotonView.Get(this);

#if UNITY_EDITOR
        {
    //        debug.text = "Unity Editor";
        }
#endif

#if UNITY_ANDROID
        {
      //      debug.text = "I'm Running on Android";
        }
#endif



    }

    // Update is called once per frame
    void Update()
    {
        //squareloc.text = transform.position.ToString();
        if (pv.isMine)
        {
            if (Input.GetButton("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    //      debug.text = "rayhit";
                    if (hit.collider.gameObject.tag == "Player" && moving == false)
                    {
                        currentSpeed = 0;
                        //debug.text = "STOP TOUCHING MEEEEEEE";
                        //menu.ToggleSpiderButtons();
                        rBody.velocity = new Vector3(0, 0, 0);
                    }
                    else if (hit.collider.gameObject.tag == "Player" && moving == true)
                    {
                        rBody.velocity = new Vector3(0, 0, 0);
                        moving = false;
                    }
                }
                else
                {
                    Vector2 currentVelocity = rBody.velocity;
                    currentVelocity += MoveFromTouch(target, currentVelocity);   //using arrive function
                    rBody.velocity = currentVelocity;
                    Debug.DrawLine(currentVelocity, target, Color.green);
                    moving = true;
                }
            }



            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    //    debug.text = "rayhit";
                    if (hit.collider.gameObject.tag == "Player" && moving == false)
                    {
                        currentSpeed = 0;
                        //debug.text = "STOP TOUCHING MEEEEEEE";
                        menu.ToggleSpiderButtons();
                        rBody.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }

    Vector2 MoveFromTouch(Vector2 targetPoint, Vector2 velocity)
    {
        Vector2 desiredVel = targetPoint - new Vector2(transform.position.x, transform.position.y);
        desiredVel.Normalize();



        Vector3 target = targetPoint;
        Vector3 distance = (target - transform.position);
        //currentSpeed = distance.magnitude
        currentSpeed = maxSpeed;
        desiredVel *= currentSpeed;


        Vector2 steeringVel = desiredVel - velocity;

        steeringVel *= (1.0f / rBody.mass);
        steeringVel = LimitSteering(steeringVel);

        return steeringVel;
    }

    protected Vector2 LimitSteering(Vector2 SteeringVelocity)
    {
        if (SteeringVelocity.sqrMagnitude > maxSteering * maxSteering)
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
