using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class ResourceScript : PunBehaviour {

    bool debug = false;

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
        if (this.tag == "Large")
        {
            playerResource.resource += large;
            Destroy(this.gameObject);
        }

        if (this.tag == "Medium")
        {
            playerResource.resource += medium;
            Destroy(this.gameObject);
        }

        if (this.tag == "Small" && debug == false)
        {
            debug = true;
            playerResource.resource += small;
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
        Destroy(this.gameObject);
    }
}