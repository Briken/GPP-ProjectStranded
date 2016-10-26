using UnityEngine;
using System.Collections;

public class PlayerResource : MonoBehaviour
{


    public int resource;


    public GameObject[] largeResources;
    public GameObject[] medResources;
    public GameObject[] smallResources;
    PhotonView pv;

    // Use this for initialization
    void Start()
    {
        pv = PhotonView.Get(this.gameObject);
        largeResources = GameObject.FindGameObjectsWithTag("Large");
        medResources = GameObject.FindGameObjectsWithTag("Medium");
        smallResources = GameObject.FindGameObjectsWithTag("Small");


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
                    if (hit.collider.gameObject.tag == "ResourceDepot")
                    {
                        hit.collider.gameObject.GetComponent<ResourceDepot>().AddTeamResource(this.gameObject);
                    }
                }
            }
        }
    }
}