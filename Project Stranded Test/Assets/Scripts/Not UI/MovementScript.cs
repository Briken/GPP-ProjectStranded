using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;


public class MovementScript : Photon.PunBehaviour
{
    UISpiderButton menu;

    public UIInformationBar info;

    bool isSpedUp = false;
    public float speedMultiplier;

    public GameObject teamMgr;
    public GameObject cam;
    public int team;
    public int playerNum;
    public float maxSteering = 50.0f;
    public float maxSpeed = 50;
    public bool isFrozen = false;
    public float speedDuration = 4.0f;
    public float freezeDuration = 4.0f;

    private Vector3 correctPPos;
    private Quaternion correctPRot;

    PhotonView pv;
    GameObject tempMgr;
    Vector3 fleePoint;
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
            //if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue) { team = 1; }
            //if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) { team = 2; }
            playerNum = PhotonNetwork.player.ID;
            if (playerNum % 2 == 0) { team = 1; }
            if (playerNum % 2 != 0) { team = 2; }
        }
        else
        {
            playerNum = this.photonView.ownerId;
            if (playerNum % 2 == 0) { team = 1; }
            if (playerNum % 2 != 0) { team = 2; }
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

                    if (hit.collider.gameObject.tag == "Player" && moving == true)
                    {
                        Debug.Log(hit.collider.gameObject.GetComponent<MovementScript>().team.ToString() + "is this players team");
                        rBody.velocity = new Vector3(0, 0, 0);
                        moving = false;
                    }
                }
                else if (isFrozen == false)
                {
                    Vector2 currentVelocity = rBody.velocity;
                    currentVelocity += MoveFromTouch(target, currentVelocity);   //using arrive function
                    rBody.velocity = currentVelocity;
                    moving = true;
                }
                if (isFrozen)
                {
                    info.DisplayInformationForSetTime("You have been frozen by another player!", 4.0f);
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
        if (isFrozen)
        {
            rBody.velocity = new Vector3(0, 0, 0);
        }
    }
     
    Vector2 MoveFromTouch(Vector2 targetPoint, Vector2 velocity)
    {
        Vector2 desiredVel = targetPoint - new Vector2(transform.position.x, transform.position.y);
        desiredVel.Normalize();



        Vector3 target = targetPoint;
        Vector3 distance = (target - transform.position);
        //currentSpeed = distance.magnitude
        if (isSpedUp)
        {
            currentSpeed = maxSpeed * speedMultiplier;
        }
        if (!isSpedUp)
        {
            currentSpeed = maxSpeed;
        }
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
            stream.SendNext(isFrozen);
            stream.SendNext(isSpedUp);
        }
        else
        {
            isFrozen = (bool)stream.ReceiveNext();
            isSpedUp = (bool)stream.ReceiveNext();

            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            this.rBody.velocity = (Vector3)stream.ReceiveNext();

        }
    }

    [PunRPC]
    public void SetPlayerFrozen(int target)
    {
        Debug.Log("target is: " + target);
        if (pv.isMine)
        {
            if (target == playerNum)
            {
                this.isFrozen = true;
                StartCoroutine (FreezeTime(freezeDuration));
            }
        }
    }

    [PunRPC]
    public void SetSpeedBuff(int target)
    {
        if (target == playerNum)
        {
            this.isSpedUp = true;
            StartCoroutine(SpeedTime(speedDuration));
        }
    }
    public void Quit()
    {
     
        Application.Quit();
    }

    IEnumerator FreezeTime(float time)
    {
        Debug.Log(Time.time.ToString());
        yield return new WaitForSeconds(time);
        this.isFrozen = false;
        
    }
    IEnumerator SpeedTime(float time)
    {
        yield return new WaitForSeconds(time);
        this.isSpedUp = false;
    }

}
