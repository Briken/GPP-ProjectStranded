using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceScript : MonoBehaviour {

    bool debug = false;

    PlayerResource playerResource;

    List<GameObject> nearby = new List<GameObject>();

    GameObject[] players;

    public float resourceDistance = 10.0f;

    public int large, medium, small;

	// Use this for initialization
	void Start () {
        players = new GameObject[7];
	}

	// Update is called once per frame
	void Update ()
	{
        if (players[0] != null)
        {
            foreach (GameObject p in players)
            {
                float distance = GetDistance(p.transform.position);
                if (distance <= resourceDistance)
                {
                    nearby.Add(p);
         //           Debug.Log(p.name);
                }
            }
        }
        if (nearby.Count >= large && this.gameObject.tag == "Large")
        {
            foreach (GameObject n in nearby)
            {
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
           //     Debug.Log("made it into the loop");
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
        }

        if (this.tag == "Medium")
        {
            playerResource.resource += medium;
        }

        if (this.tag == "Small" && debug == false)
        {
            debug = true;
            playerResource.resource += small;
            //Debug.Log(playerResource.resource.ToString());
            
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
}