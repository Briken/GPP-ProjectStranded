﻿using UnityEngine;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {
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
                        // Let the player know how much they have deposited if they have anything to deposit
                        if (resource > 0)
                        {
                            hit.collider.gameObject.GetComponent<ShipScript>().photonView.RPC("DepositFuel", PhotonTargets.All, resource);
                            // informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You deposited +" + resource.ToString() + "% fuel", 5.0f);
                            // depositParticleObject.GetComponent<ParticleSystem>().Play();
                            GameObject.Find("HintBox").GetComponent<UIHintBox>().DisplayHint("FUEL DEPOSITED!", "YOU DEPOSITED \n" + resource.ToString() + "% OF FUEL \nTO YOUR SHIP!", 5.0f);

                            gameObject.GetComponent<PlayerStatTracker>().overallFuelDeposited += resource;
                            gameObject.GetComponent<PlayerStatTracker>().timesDepositingFuel += 1;
                            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastFuelDeposit = 0;

                            resource = 0;
                        }
                    }
                }
            }

            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastNearFuelCrate += Time.deltaTime;
            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastFuelCratePickup += Time.deltaTime;
            gameObject.GetComponent<PlayerStatTracker>().timeSinceLastFuelDeposit += Time.deltaTime;
        }
    
    }

    public void ResetThis()
    {
        resource = 0;
    }

}