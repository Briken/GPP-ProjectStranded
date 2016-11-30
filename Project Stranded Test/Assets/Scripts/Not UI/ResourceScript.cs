using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class ResourceScript : PunBehaviour {

    bool debug = false;

    public GameObject particleEffectPrefab; 

    PlayerResource playerResource;

    public List<GameObject> nearby = new List<GameObject>();

    public GameObject[] players;

    public float resourceDistance = 10.0f;

    public int large = 10;
    public int medium = 5;
    public int small = 1;
	// Use this for initialization
	void Start () {
        players = new GameObject[7];
	}

	// Update is called once per frame
	void Update ()
	{
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0] != null)
        {
            nearby.Clear();
            foreach (GameObject p in players)
            {   
                if (p == null)
                {
                    continue;
                }
                float distance = GetDistance(p.transform.position);
                if (distance <= resourceDistance)
                {
                    nearby.Add(p);
                    Debug.Log(nearby);
                }
            }
        }
        if (nearby.Count >= large && this.gameObject.tag == "Large")
        {
            foreach (GameObject n in nearby)
            {
                Debug.Log("fire flaming suck lance");
                AddResource(n);
            }
        }
        if (nearby.Count >= medium && this.gameObject.tag == "Medium")
        {
            foreach (GameObject n in nearby)
            {
                AddResource(n);
            }
        }
        if (nearby.Count >= small && this.gameObject.tag == "Small")
        {
            foreach (GameObject n in nearby)
            {
                Debug.Log("small nearby loop");
                AddResource(n);
            }
        }
    }

    public void AddResource(GameObject player)
    {
        
        //Debug.Log(player.name);
        playerResource = player.GetComponent<PlayerResource>();
        GameObject informationBar = GameObject.FindGameObjectWithTag("Information Bar");

        if (this.tag == "Large")
        {
            playerResource.resource += large;
            informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You picked up " + large.ToString() + " fuel!", 4.0f);
            photonView.RPC("DestroyThis", PhotonTargets.All);
        }

        if (this.tag == "Medium")
        {
            playerResource.resource += medium;
            informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You picked up " + medium.ToString() + " fuel!", 4.0f);
            photonView.RPC("DestroyThis", PhotonTargets.All);
        }

        if (this.tag == "Small" && debug == false)
        {
            debug = true;
            playerResource.resource += small;
            informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You picked up " + small.ToString() + " fuel!", 4.0f);
            //Debug.Log(playerResource.resource.ToString());
            photonView.RPC("DestroyThis", PhotonTargets.All);
        }
        
    }

    
    float GetDistance(Vector3 pos)
    {
        Vector3 distanceVec = pos - this.transform.position;
        float distance = Vector3.Magnitude(distanceVec);
        return distance;
    }

    public void SetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    [PunRPC]
    public void DestroyThis()
    {
        GameObject particleEffectObject = (GameObject)Instantiate(particleEffectPrefab, gameObject.transform.position, Random.rotation);
        Destroy(this.gameObject);
    }
}