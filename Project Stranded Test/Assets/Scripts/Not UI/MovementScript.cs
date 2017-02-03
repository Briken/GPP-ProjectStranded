using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;


public class MovementScript : Photon.PunBehaviour
{
    UISpiderButton menu;

    public UIInformationBar info;

  

    public GameObject teamMgr;
    public GameObject cam;

    public int playerNum;
    public float maxSteering = 50.0f;
    public float maxSpeed = 50;

    
    private Vector3 correctPPos;    
    private Quaternion correctPRot;

    PhotonView pv;
    GameObject tempMgr;
    
    float currentSpeed = 10;
    bool moving = false;


    protected Rigidbody rBody;
    // Use this for initialization
    void Start()
    {
        info = GameObject.Find("Information Bar").GetComponent<UIInformationBar>();
        menu = GetComponent<UISpiderButton>();
        rBody = GetComponent<Rigidbody>();
        pv = PhotonView.Get(this.gameObject);
        tempMgr = GameObject.Find("TeamManager");




        if (pv.isMine)
        {

            Debug.Log(cam.name);
            Camera.main.gameObject.transform.SetParent(this.transform);
            playerNum = PhotonNetwork.player.ID;
        }

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
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPRot, Time.deltaTime * 5);
        }
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
                    Vector2 currentVelocity = rBody.velocity;
                    currentVelocity += MoveFromTouch(target, currentVelocity);   //using arrive function
                    rBody.velocity = currentVelocity;
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
                    if (hit.collider.gameObject.tag == "Player" && moving == false && hit.collider.gameObject == this.gameObject)
                    {
                        currentSpeed = 0;
                        menu.ToggleSpiderButtons();
                        rBody.velocity = new Vector3(0, 0, 0);
                    }
                }
                Vector3 pos = transform.position;
                pos.z = 0;
                transform.position = pos;
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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("dis shit being called yo");
    }

    public void OnSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rBody.velocity);
        }
        else
        {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            this.rBody.velocity = (Vector3)stream.ReceiveNext();

        }
    }

    
    public void Quit()
    {
     
        Application.Quit();
    }
}
