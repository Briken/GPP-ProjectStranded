using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class ResourceScript : PunBehaviour {

    bool debug = false;

    public float waitTimer = 10.0f;
    public GameObject particleEffectPrefab; 

    PlayerResource playerResource;

    public GameObject votedOut;

    public List<GameObject> nearby = new List<GameObject>();

    public GameObject[] players;

    public float resourceDistance = 10.0f;

    int requirement;
    public int large = 10;
    public int medium = 5;
    public int small = 1;

    public int amount;
    
	// Use this for initialization
	void Start () {
        votedOut = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteLoss;
        if (tag == "Small")
        {
            requirement = small;
            amount = 3;
        }
        if (tag == "Medium")
        {
            requirement = medium;
            amount = 6;
        }
        if (tag == "Large")
        {
            requirement = large;
            amount = 12;
        }
                
        players = new GameObject[7];
	}

	// Update is called once per frame
	void Update ()
	{
        players = GameObject.FindGameObjectsWithTag("Player");
        
        if (players != null)
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
            StartCoroutine(Wait());
        }
        if (nearby.Count > requirement)
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
        
        playerResource.resource += amount;
        //informationBar.GetComponent<UIInformationBar>().DisplayInformationForSetTime("You picked up " + large.ToString() + " fuel!", 4.0f);
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
        player.GetComponent<VotingSystem>().voteCard.SetActive(false);
        int boot = player.GetComponent<VotingSystem>().CheckVote();
        foreach (GameObject n in nearby)
        {
            if (boot == n.GetComponent<MovementScript>().playerNum && photonView.isMine)
            {
                votedOut.SetActive(true);
                yield return new WaitForSeconds(5);
            }
            if (nearby.Count == requirement && boot != n.GetComponent<MovementScript>().playerNum)
            {
                AddResource(player);
            }
            else if (nearby.Count > requirement)
            {
                StartCoroutine(ResourceTime(waitTime, player));
            }
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTimer);
        if (nearby.Count == requirement)
        {
            foreach (GameObject n in nearby)
            {
                AddResource(n);
                //StartCoroutine(ResourceTime(waitTimer, n));
            }
        }
    }

    IEnumerator VotedOut()
    {
        yield return new WaitForSeconds(6);
        votedOut.SetActive(false);
    }
    [PunRPC]
    public void DestroyThis()
    {
        GameObject particleEffectObject = (GameObject)Instantiate(particleEffectPrefab, gameObject.transform.position, Random.rotation);
        Destroy(this.gameObject);
    }
}