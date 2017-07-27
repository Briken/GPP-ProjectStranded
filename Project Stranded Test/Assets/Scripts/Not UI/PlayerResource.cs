using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerResource : MonoBehaviour
{


    public int resource;

    public Text uiResource;

    public GameObject[] largeResources;
    public GameObject[] medResources;
    public GameObject[] smallResources;
    PhotonView pv;
    GameObject depositParticleObject;

    GameObject informationBar;

    public bool isDepositing = false;
    public float depositTime = 3.0f;
    float defaultDepositTime;

    float distanceFromShip;
    public float maximumDistanceForDepositing = 5.0f;

    public AudioClip depositSound;

    // Use this for initialization
    void Start()
    {
        EventManager.Reset += ResetThis;
        //DontDestroyOnLoad(this);
        pv = PhotonView.Get(this.gameObject);
        largeResources = GameObject.FindGameObjectsWithTag("Large");
        medResources = GameObject.FindGameObjectsWithTag("Medium");
        smallResources = GameObject.FindGameObjectsWithTag("Small");

        informationBar = GameObject.FindGameObjectWithTag("Information Bar");
        depositParticleObject = GameObject.FindGameObjectWithTag("Deposit Particle System");

        foreach (GameObject r in largeResources)
        {
            r.GetComponent<ResourceScript>().SetPlayers();
        }
        foreach (GameObject r in medResources)
        {
            r.GetComponent<ResourceScript>().SetPlayers();
        }
        foreach (GameObject r in smallResources)
        {
            r.GetComponent<ResourceScript>().SetPlayers();
        }

        defaultDepositTime = depositTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {

            distanceFromShip = Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<MovementScript>().myShip.gameObject.transform.position);

            if (Input.GetButton("Fire1"))
            {
                Debug.Log("button down");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("ray hit");
                    if (hit.collider.gameObject.tag == "Ship" && hit.collider.gameObject.GetComponent<ShipScript>().shipNum ==
                        this.GetComponent<MovementScript>().playerNum)
                    {

                        //hit.collider.gameObject.GetComponent<ResourceDepot>().AddTeamResource(this.gameObject);

                        Debug.Log("hit ship");

                        if (resource > 0 && (distanceFromShip < maximumDistanceForDepositing))
                        {
                            StartFuelDeposit();
                        }

                    }
                }
            }

            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastNearFuelCrate += Time.deltaTime;
            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastFuelCratePickup += Time.deltaTime;
            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastFuelDeposit += Time.deltaTime;

            // Change player state if they are depositing
            if (isDepositing)
            {
                gameObject.transform.position = gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockObject.transform.position;
                gameObject.GetComponent<MovementScript>().canMove = false;

                // Update ship text prompts
                gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockPrompt.SetActive(true);
                gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<PlayerShipDocking>().dockTextObjects[0].GetComponent<Text>().text = "DEPOSITING FUEL...";
                gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<PlayerShipDocking>().dockTextObjects[1].GetComponent<Text>().text = depositTime.ToString("0.0") + "s";
                gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<PlayerShipDocking>().ChangePromptColour(new Color(1.0f, 0.68f, 0.0f, 1.0f));

                if (depositTime >= 0.0f && GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer >= 0.0f)
                {
                    depositTime -= Time.deltaTime;
                }
                else
                {
                    FinishFuelDeposit();
                }
            }
            else
            {     
                // Display deposit prompt if the player has fuel and is close enough to their ship
                if (resource > 0 && distanceFromShip < maximumDistanceForDepositing)
                {
                    gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockPrompt.SetActive(true);

                    foreach (GameObject dockPromptTextObject in gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<PlayerShipDocking>().dockTextObjects)
                    {
                        dockPromptTextObject.GetComponent<Text>().text = "TAP TO DEPOSIT";
                        dockPromptTextObject.GetComponent<Text>().resizeTextForBestFit = false;
                    }
                }
                else
                {
                    if (resource > 0 && distanceFromShip < maximumDistanceForDepositing + 7.5f)
                    {
                        gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockPrompt.SetActive(true);

                        foreach (GameObject dockPromptTextObject in gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<PlayerShipDocking>().dockTextObjects)
                        {
                            dockPromptTextObject.GetComponent<Text>().text = "MOVE CLOSER TO DEPOSIT";
                            dockPromptTextObject.GetComponent<Text>().resizeTextForBestFit = true;
                        }
                    }
                    else
                    {
                        gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockPrompt.SetActive(false);
                    }           
                }
            }
        }
    
    }

    public void ResetThis()
    {
        resource = 0;
    }

    void StartFuelDeposit()
    {
        if (!isDepositing)
        {
            isDepositing = true;
            gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockFuelPipe.SetActive(true);
        }
    }

    void FinishFuelDeposit()
    {
        // Check if the time has expired before allowing deposit
        if (GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer >= 0.0f)
        {
            gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<ShipScript>().photonView.RPC("DepositFuel", PhotonTargets.All, resource);
            // informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You deposited +" + resource.ToString() + "% fuel", 5.0f);
            // depositParticleObject.GetComponent<ParticleSystem>().Play();

            // Let the player know how much they have deposited via a hint (temporary)
            // GameObject.Find("HintBox").GetComponent<UIHintBox>().DisplayHint("FUEL DEPOSITED!", "YOU DEPOSITED \n" + resource.ToString() + "% OF FUEL \nTO YOUR SHIP!", 5.0f);

            GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIFuelCollected>().DisplayFuelDeposited(resource);

            if (gameObject.GetComponent<AudioSource>() != null && gameObject.GetPhotonView().isMine)
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(depositSound);
            }

            gameObject.GetComponent<PlayerStatTracker>().overallFuelDeposited += resource;
            gameObject.GetComponent<PlayerStatTracker>().timesDepositingFuel += 1;
            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastFuelDeposit = 0;

            if (resource > gameObject.GetComponent<PlayerStatTracker>().maxFuelDepositedAtOnce)
            {
                gameObject.GetComponent<PlayerStatTracker>().maxFuelDepositedAtOnce = resource;
            }

            if (GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer > gameObject.GetComponent<PlayerStatTracker>().earliestDepositTimeRemaining)
            {
                gameObject.GetComponent<PlayerStatTracker>().earliestDepositTimeRemaining = GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer;
            }

            if (GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer < gameObject.GetComponent<PlayerStatTracker>().latestDepositTimeRemaining)
            {
                gameObject.GetComponent<PlayerStatTracker>().latestDepositTimeRemaining = GameObject.FindGameObjectWithTag("NetManager").GetComponent<GameTimer>().timer;
            }

            resource = 0;

            gameObject.GetComponent<MovementScript>().canMove = true;
        }
        else // Prevent deposit behaviour and don't unfreeze player if time has expired
        {
            gameObject.GetComponent<MovementScript>().canMove = false;
        }

        gameObject.GetComponent<MovementScript>().myShip.GetComponent<PlayerShipDocking>().dockFuelPipe.SetActive(false);
        gameObject.GetComponent<MovementScript>().myShip.gameObject.GetComponent<PlayerShipDocking>().ChangePromptColour(Color.white);

        depositTime = defaultDepositTime;
        isDepositing = false;
    }
}