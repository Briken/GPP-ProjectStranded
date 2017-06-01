using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class ResourceScript : PunBehaviour {

    bool debug = false;

    public int seed;
	bool isGifted = false;

    public float waitTimer = 5.0f;
    public GameObject particleEffectPrefab; 

    PlayerResource playerResource;
    bool voteIsCalled = false;
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


        
        if (nearby.Count == requirement && voteIsCalled == false)
        {
            
            
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0 && !isGifted)
            {
                isGifted = true;
                foreach (GameObject n in nearby)
                {
                    AddResource(n);
                }
                DestroyThis();
            }
        }



        if (nearby.Count > requirement && voteIsCalled == false)
        {
            voteIsCalled = true;
            foreach (GameObject n in nearby)
            {
                StartCoroutine(ResourceTime(waitTimer, n));
            }
            DestroyThis();
        }
    }

    public void AddResource(GameObject player)
    {

       
        Debug.Log("player " + player.GetComponent<MovementScript>().playerNum + " has recieved " + amount);
        playerResource = player.GetComponent<PlayerResource>();
        playerResource.resource += amount;
  
           
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
  

    IEnumerator VotedOut()
    {
        yield return new WaitForSeconds(6);
        votedOut.SetActive(false);
    }

    [PunRPC]
    public void ReceiveSeed(int recieved)
    {
        seed = recieved;
        Debug.Log("Seed for this crate " + name + " is " + seed);
    }

    public void DestroyThis()
    {
       
       PhotonNetwork.Destroy(this.gameObject);
    }
}