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

    // Use this for initialization
    void Start()
    {
        
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    //      debug.text = "rayhit";
                    if (hit.collider.gameObject.tag == "Ship" && hit.collider.gameObject.GetComponent<ShipScript>().shipNum ==
                        this.GetComponent<MovementScript>().playerNum)
                    {

                        //hit.collider.gameObject.GetComponent<ResourceDepot>().AddTeamResource(this.gameObject);


                        // Let the player know how much they have deposited if they have anything to deposit
                        if (resource > 0)
                        {

                            informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You deposited " + resource.ToString(), 3.0f);
                            depositParticleObject.GetComponent<ParticleSystem>().Play();
                        }

                        resource = 0;
                    }
                }
            }
        }
    
    }

    
}