﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class ResourceScript : PunBehaviour {

    bool debug = false;

    public float waitTimer = 10.0f;
    public GameObject particleEffectPrefab; 

    PlayerResource playerResource;

    public List<GameObject> nearby = new List<GameObject>();

    public GameObject[] players;

    public float resourceDistance = 10.0f;

    int requirement;
    public int large = 10;
    public int medium = 5;
    public int small = 1;
	// Use this for initialization
	void Start () {
        if (tag == "Small")
        {
            requirement = small;
        }
        if (tag == "Medium")
        {
            requirement = medium;
        }
        if (tag == "Large")
        {
            requirement = large;
        }
                
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
        
        if (nearby.Count == requirement)
        {
            foreach (GameObject n in nearby)
            {
                StartCoroutine(ResourceTime(waitTimer, n));
            }
        }
        //if (nearby.Count == small && this.gameObject.tag == "Small")
        //{
        //    foreach (GameObject n in nearby)
        //    {
        //        AddResource(n);
        //    }
        //}
        //if (nearby.Count == large && this.gameObject.tag == "Large")
        //{
        //    foreach (GameObject n in nearby)
        //    {
        //        AddResource(n);
        //    }
        //}
    }

    public void AddResource(GameObject player)
    {
        
        //Debug.Log(player.name);
        playerResource = player.GetComponent<PlayerResource>();
        GameObject informationBar = GameObject.FindGameObjectWithTag("Information Bar");
        
        playerResource.resource += requirement;
        informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You picked up " + large.ToString() + " fuel!", 4.0f);
        photonView.RPC("DestroyThis", PhotonTargets.All);
        
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

    IEnumerator ResourceTime(float waitTime, GameObject player)
    {
        player.GetComponent<VotingSystem>().CallVote();
        yield return new WaitForSeconds(waitTime);
        player.GetComponent<VotingSystem>().voteCount.SetActive(false);
        if (nearby.Count == requirement)
        {
            AddResource(player);
        }
    }

    [PunRPC]
    public void DestroyThis()
    {
        GameObject particleEffectObject = (GameObject)Instantiate(particleEffectPrefab, gameObject.transform.position, Random.rotation);
        Destroy(this.gameObject);
    }
}