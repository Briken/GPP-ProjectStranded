using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;


public class MovementScript : Photon.PunBehaviour
{
    UISpiderButton menu;

    public UIInformationBar info;

    public GameObject[] ships;
    public GameObject myShip;
   
    public GameObject cam;

    public int playerNum;
    public float maxSteering = 50.0f;
    public float maxSpeed = 50;

    bool hasClaimed = false;
    
    private Vector3 correctPPos;    
    private Quaternion correctPRot;

    public PhotonView pv;
    GameObject tempMgr;
    Vector3 shipPos;
    Color[] colours;

    float currentSpeed = 10;
    bool moving = false;

    public GameObject playerBody;
    public GameObject[] playerColouredParts;
    public GameObject movementParticleSystem;

    protected Rigidbody rBody;
    // Use this for initialization
    void Start()
    {

        shipPos = transform.position;
        colours = new Color[5];
        SetColours();
        info = GameObject.Find("Information Bar").GetComponent<UIInformationBar>();
        menu = GetComponent<UISpiderButton>();
        rBody = GetComponent<Rigidbody>();
        pv = PhotonView.Get(this.gameObject);
        tempMgr = GameObject.Find("TeamManager");

        ships = GameObject.FindGameObjectsWithTag("Ship");

        if (pv.isMine)
        {

            Debug.Log(cam.name);
            Camera.main.gameObject.transform.SetParent(this.transform);
            Camera.main.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -5);

        }

        for (int n = 0; n < ships.Length; n++)// GameObject n in ships)
        {
            if (ships[n].GetComponent<ShipScript>().claimed != true && hasClaimed == false)
            {
                ships[n].GetComponent<ShipScript>().claimed = true;
                hasClaimed = true;
                ships[n].GetComponent<ShipScript>().player = this.gameObject;
                ships[n].GetComponent<ShipScript>().shipNum = playerNum;
                ships[n].GetComponent<SpriteRenderer>().color = colours[playerNum];
            }
            //if (ships[n].transform.position == shipPos && ships[n].GetComponent<ShipScript>().claimed == false)
            //{
            //    ships[n].GetComponent<ShipScript>().claimed = true;
            //       hasClaimed = true;
            //       ships[n].GetComponent<ShipScript>().player = this.gameObject;
            //       ships[n].GetComponent<ShipScript>().shipNum = playerNum;
            //       ships[n].GetComponent<SpriteRenderer>().color = colours[playerNum-1];
            //}
            //if (ships[n].transform.position == shipPos && ships[n].GetComponent<ShipScript>().claimed == true)
            //{
            //    shipPos = ships[n + 1].transform.position;
            //}
            //if (n == 4 && !hasClaimed) n = 0;
        }

        foreach (GameObject playerColouredPart in playerColouredParts)
        { 
            playerColouredPart.GetComponent<SpriteRenderer>().color = colours[playerNum-1];
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
                Debug.Log("Button Pressed");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                //if (Physics.Raycast(ray, out hit))
                //{
                //    Debug.Log("Raycast Hit, movement to begin now");
                //    Vector2 currentVelocity = rBody.velocity;
                //    currentVelocity += MoveFromTouch(target, currentVelocity);   //using arrive function
                //    rBody.velocity = currentVelocity;
                //    moving = true;
                //}

                

                // Adjust player sprite depending on what vertical direction they are moving in
                if (Input.mousePosition.x > Screen.width/2)
                {
                    playerBody.gameObject.transform.localScale = new Vector3(-1.0f, playerBody.gameObject.gameObject.transform.localScale.y, playerBody.gameObject.gameObject.transform.localScale.z);
                }

                if (Input.mousePosition.x < Screen.width / 2)
                {
                    playerBody.gameObject.transform.localScale = new Vector3(1.0f, playerBody.gameObject.gameObject.transform.localScale.y, playerBody.gameObject.gameObject.transform.localScale.z);
                }

                // movementParticleSystem.transform.LookAt(target);
            }


            if (Input.GetButtonDown("Fire1"))
            {
                Vector2 currentVelocity = rBody.velocity;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log(hit.collider.gameObject.GetComponent<PhotonView>().ownerId + " is this objects owner ID");
                    }
                    if (/*hit.collider.gameObject.GetComponent<MovementScript>().playerNum == playerNum*/ hit.collider.gameObject == this.gameObject)
                    {
                        currentSpeed = 0;
                       // menu.ToggleSpiderButtons();
                        currentVelocity = rBody.velocity;
                        currentVelocity += MoveFromTouch(currentVelocity, currentVelocity);   //using arrive function
                        rBody.velocity = currentVelocity;
                        moving = false;
                    }
                    else
                    {
                        currentSpeed = 10;
                        currentVelocity = rBody.velocity;
                        currentVelocity += MoveFromTouch(target, currentVelocity);   //using arrive function
                        rBody.velocity = currentVelocity;
                        moving = true;
                    }
                }
                else
                {
                    currentSpeed = 10;
                    currentVelocity += MoveFromTouch(target, currentVelocity);   //using arrive function
                    rBody.velocity = currentVelocity;
                    moving = true;
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


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
           
        }
        else
        {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        

        }
    }

    void SetColours()
    {
        
        colours[0] = Color.red;
        colours[1] = Color.blue;
        colours[2] = Color.green;
        colours[3] = Color.yellow;
        colours[4] = Color.white;
    }

    [PunRPC]
    public void SetNum(int pNum)
    {
        playerNum = pNum;
    }

    public void Quit()
    {
     
        Application.Quit();
    }
}
