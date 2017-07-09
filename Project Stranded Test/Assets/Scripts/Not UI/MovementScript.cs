using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Analytics;
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

    public bool canMove = true;
    bool hasLockedPosition = false;
    Vector3 lockedPosition;
    public float lockOverrideTime = 6.0f; // Used to manually unfreeze the player in the event that they are not unlocked elsewhere (ideally use vote time)
    float defaultLockOverrideTime;

    bool hasClaimed = false;
    bool stopHasBeenCalled = false;

    private Vector3 correctPPos;    
    private Quaternion correctPRot;

    public PhotonView pv;
    GameObject tempMgr;
    Vector3 shipPos;
    Color[] colours;
    public Color myColour;

    float currentSpeed = 10;
    bool moving = false;

    public GameObject playerBody;
    public GameObject[] playerColouredParts;
    public GameObject movementParticleSystem;
    public GameObject movementParticleSystemSmall;

    protected Rigidbody rBody;

    public float cameraSize = 9.0f;

    public string publicUsername;

    // Use this for initialization
    void Start()
    {
        EventManager.Reset += ResetThis;

        shipPos = transform.position;
        colours = new Color[5];
        SetColours();
        info = GameObject.Find("Information Bar").GetComponent<UIInformationBar>();
        menu = GetComponent<UISpiderButton>();
        rBody = GetComponent<Rigidbody>();
        pv = PhotonView.Get(this.gameObject);
        tempMgr = GameObject.Find("TeamManager");

        Analytics.CustomEvent("PlayerVoted", new Dictionary<string, object>
        {
             { "player username", publicUsername},
        });
        
        ships = GameObject.FindGameObjectWithTag("NetManager").GetComponent<PhotonNetCode>().ships;

        if (pv.isMine)
        {

            Debug.Log(cam.name);
            Camera.main.gameObject.GetComponent<Animator>().Stop();
            Camera.main.gameObject.transform.SetParent(this.transform);
            Camera.main.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -50);
            Camera.main.orthographicSize = cameraSize;

        }

        for (int n = 0; n < ships.Length; n++)// GameObject n in ships)
        {
            foreach (GameObject playerColouredPart in playerColouredParts)
            {

                playerColouredPart.GetComponent<SpriteRenderer>().color = colours[playerNum - 1];
                myColour = colours[playerNum - 1];
            }
            if (ships[playerNum - 1].GetComponent<ShipScript>().claimed != true && hasClaimed == false)
            {
                ships[playerNum - 1].GetComponent<ShipScript>().claimed = true;
                hasClaimed = true;
                ships[playerNum - 1].GetComponent<ShipScript>().player = this.gameObject;
                ships[playerNum - 1].GetComponent<ShipScript>().shipNum = playerNum;
                myShip = ships[playerNum - 1];
               // ships[n].GetComponent<SpriteRenderer>().color = playerColouredParts[0].GetComponent<SpriteRenderer>().color;
            }
        }

        defaultLockOverrideTime = lockOverrideTime;

        // Display tap to move prompt via main UI handler
        GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIMainGameHandler>().startMovementPrompt.SetActive(true);


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
         //   transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPRot, Time.deltaTime * 5);
        }
        //squareloc.text = transform.position.ToString();
        if (pv.isMine)
        {
            // If the player is not allowed to move, lock their position
            if (!canMove)
            {
                // Store the player's last location prior to locking their position
                if (!hasLockedPosition)
                {
                    lockedPosition = gameObject.transform.position;
                    playerBody.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    hasLockedPosition = true;
                }

                // Lock the player's movement to the set position until unlocked or override time expires
                if (lockOverrideTime >= 0.0f)
                {
                    gameObject.transform.position = lockedPosition;   
                    lockOverrideTime -= Time.deltaTime;
                }
                else
                {
                    canMove = true;
                    lockOverrideTime = defaultLockOverrideTime;
                }     
            }
            else
            {
                hasLockedPosition = false;
            }

            if (Input.GetButton("Fire1"))
            {
                Debug.Log("Button Pressed");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();


                // Adjust player sprite depending on what vertical direction they are moving in
                if (canMove)
                {
                    if (Input.mousePosition.x > Screen.width / 2)
                    {
                        playerBody.gameObject.transform.localScale = new Vector3(-1.0f, playerBody.gameObject.gameObject.transform.localScale.y, playerBody.gameObject.gameObject.transform.localScale.z);
                    }

                    if (Input.mousePosition.x < Screen.width / 2)
                    {
                        playerBody.gameObject.transform.localScale = new Vector3(1.0f, playerBody.gameObject.gameObject.transform.localScale.y, playerBody.gameObject.gameObject.transform.localScale.z);
                    }
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
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        Stop();
                        
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

                // Hide tap to move prompt via main UI handler
                GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIMainGameHandler>().startMovementPrompt.SetActive(false);
            }

            if (moving && canMove)
            {
                gameObject.GetComponent<PlayerStatTracker>().timeSpentMoving += Time.deltaTime;
                movementParticleSystem.GetComponent<ParticleSystem>().time = 0.0f;
                movementParticleSystem.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                gameObject.GetComponent<PlayerStatTracker>().timeSpentNotMoving += Time.deltaTime;
                movementParticleSystem.GetComponent<ParticleSystem>().Stop();
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
        colours[2] = new Color(0, 0.80f, 0); // Minor adjustment to green colour
        colours[3] = Color.yellow;
        colours[4] = new Color(1, 0.5f, 0);
    }

    [PunRPC]
    public void SetNum(int pNum)
    {
        playerNum = pNum;
    }

    [PunRPC]
    public void SetUsername(string username)
    {
        publicUsername = username;
    }

    public void Stop()
    {
        Debug.Log("Stop has been called");
        stopHasBeenCalled = true;
        Vector2 currentVelocity = rBody.velocity;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        currentSpeed = 0;
        currentVelocity = rBody.velocity;
        currentVelocity += MoveFromTouch(currentVelocity, currentVelocity);   //using arrive function
        rBody.velocity = currentVelocity;
        stopHasBeenCalled = false;
        moving = false;
    }

    public void Quit()
    {
     
        Application.Quit();
    }

    // Level boundaries handling
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "LevelBoundaries")
        {
            // Stop and move the player to 0, 0 if they attempt to leave
            Stop();
            currentSpeed = 10;
            Vector2 currentVelocity = rBody.velocity;
            currentVelocity += MoveFromTouch(new Vector3(0.0f, 0.0f, 0.0f), currentVelocity);  //using arrive function
            rBody.velocity = currentVelocity;
            moving = true;

            // Flip the player's sprite direction so they don't fly backwards
            playerBody.gameObject.transform.localScale = new Vector3(playerBody.gameObject.gameObject.transform.localScale.x*-1.0f, playerBody.gameObject.gameObject.transform.localScale.y, playerBody.gameObject.gameObject.transform.localScale.z);
        }
    }

    private void ResetThis()
    {
        transform.position = shipPos;
        canMove = true;
        lockOverrideTime = defaultLockOverrideTime;
        Debug.Log("reset Called");
    }
}
