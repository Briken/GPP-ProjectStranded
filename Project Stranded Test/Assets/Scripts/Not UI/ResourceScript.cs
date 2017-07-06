﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;

public class ResourceScript : PunBehaviour {

    bool debug = false;

    SpriteRenderer thisMainSprite;
    Image[] attachedImages;
    SpriteRenderer[] attachedSprites;
    bool isRunning;


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

    public int requirement;

    public int amount;

    public float totalVoteTime = 5.0f;
    public float remainingVoteTime = 0.0f;
    public bool voteConcluded = false;
    public List<GameObject> playersCurrentlyVoting = new List<GameObject>();
    
	// Use this for initialization
	void Start ()
    {
        thisMainSprite = this.GetComponent<SpriteRenderer>();
        EventManager.Reset += ResetThis;

        votedOut = GameObject.Find("NetworkManager").GetComponent<PhotonNetCode>().voteLoss;
                
        players = new GameObject[7];

        isRunning = true;
	}

	// Update is called once per frame
	void Update ()
	{
        if (isRunning)
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
                        p.GetComponent<PlayerStatTracker>().timeSinceLastNearFuelCrate = 0.0f;
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
                        Debug.Log("FUEL CRATE: Giving fuel to " + nearby.Count.ToString() + " players!");
                    }

                    DestroyThis();
                }
            }


            // Start a new vote if the player count is exceeded and there is not currently an ongoing vote
            if (nearby.Count > requirement && voteIsCalled == false)
            {
                voteIsCalled = true;

                // Create a list of players that will be voting
                foreach (GameObject n in nearby)
                {
                    playersCurrentlyVoting.Add(n);
                }

                // Initiate a new vote on each nearby player's end
                foreach (GameObject n in nearby)
                {
                    if (n.GetPhotonView().isMine)
                    {
                        // StartCoroutine(ResourceTime(10.0f, n, seed));

                        // Lock player position to prevent them flying away during a vote
                        n.GetComponent<MovementScript>().canMove = false;

                        InitiateNewVote(totalVoteTime, n, seed);

                        n.GetComponent<PlayerStatTracker>().timesInVote += 1;
                    }
                }

            }

            // Constantly check the vote state
            CheckVoteState();
        }
    }

    public void AddResource(GameObject player)
    {
  
        Debug.Log("player " + player.GetComponent<MovementScript>().playerNum + " has recieved " + amount);
        playerResource = player.GetComponent<PlayerResource>();
        playerResource.resource += amount;

        // Display hint box on players that receive fuel from the crate
        if (player.GetPhotonView().isMine)
        {
            GameObject.Find("HintBox").GetComponent<UIHintBox>().DisplayHint("FUEL RECEIVED!", "YOU COLLECTED " + amount.ToString() + " FUEL \nFROM THIS CRATE\nDEPOSIT OR COLLECT MORE!", 6.0f);
        }

        player.GetComponent<PlayerStatTracker>().timeSinceLastFuelCratePickup = 0.0f;
        player.GetComponent<PlayerStatTracker>().overallCollectedFuel += amount;
        player.GetComponent<PlayerStatTracker>().fuelCratesOpened += 1;
        player.GetComponent<PlayerStatTracker>().fuelCratesOpenedBySize[requirement] += 1;           
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

    // Activate vote and start timer
    void InitiateNewVote(float voteLengthTime, GameObject player, int thisSeed)
    {
        // Call the vote via the voting system
        player.GetComponent<VotingSystem>().CallVote(playersCurrentlyVoting, voteLengthTime + 5.0f);
        remainingVoteTime = voteLengthTime;
        voteIsCalled = true;

        Debug.Log("NEW VOTING SYSTEM: Vote Initiated Successfully!");
    }

    // Check for vote timer expiration or if other circumstances require vote to end
    void CheckVoteState()
    {
        if (remainingVoteTime > 0.0f && voteIsCalled)
        {
            remainingVoteTime -= Time.deltaTime;
            GameObject.Find("Text - Vote Time Remaining").GetComponent<Text>().text = "TIME REMAINING: " + remainingVoteTime.ToString("0.0") + "s";
        }

        if (remainingVoteTime <= 0.0f && voteIsCalled)
        {
            ConcludeVote();
        }

    }

    void ConcludeVote()
    {
        if (!voteConcluded)
        {
            foreach (GameObject n in playersCurrentlyVoting)
            {
                // Fetch the number of the player to vote out
                int boot = n.GetComponent<VotingSystem>().CheckVote(seed);

                // Display voted out screen to voted out player
                if (boot == n.GetComponent<MovementScript>().playerNum)
                {
                    n.GetComponent<VotingSystem>().DisplayVotedOutScreen();
                }

                // Give fuel to all players not voted out
                if (playersCurrentlyVoting.Count > requirement && boot != n.GetComponent<MovementScript>().playerNum)
                {
                    AddResource(n);
                }

                // Hide voting screen now that voting has concluded
                n.GetComponent<VotingSystem>().voteCard.SetActive(false);

                // Allow the player to move again
                n.GetComponent<MovementScript>().canMove = true;
            }

            voteConcluded = true;
            playersCurrentlyVoting.Clear();

            DestroyThis();
        }
    }

    /*
    IEnumerator ResourceTime(float waitTime, GameObject player, int thisSeed)
    {
        player.GetComponent<VotingSystem>().CallVote();
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Timer Waited for, votecard should shut down now");
        player.GetComponent<VotingSystem>().voteCard.SetActive(false);
        int boot = player.GetComponent<VotingSystem>().CheckVote(thisSeed);
        foreach (GameObject n in nearby)
        {
            if (boot == n.GetComponent<MovementScript>().playerNum && photonView.isMine)
            {
                votedOut.SetActive(true);
                
                
            }
            if (nearby.Count == requirement && boot != n.GetComponent<MovementScript>().playerNum)
            {
                AddResource(player);
                
            }
            else if (nearby.Count > requirement)
            {
                StartCoroutine(ResourceTime(waitTime, player, thisSeed));
            }
        }
        DestroyThis();
    }
    */  

    [PunRPC]
    public void ReceiveSeed(int recieved)
    {
        seed = recieved;
        Debug.Log("Seed for this crate " + name + " is " + seed);
    }

    public void DestroyThis()
    {
       GameObject particleEffectObject = (GameObject)Instantiate(particleEffectPrefab, gameObject.transform.position, Random.rotation);
        // votedOut.SetActive(false);
        //PhotonNetwork.Destroy(this.gameObject);
        attachedImages = this.GetComponentsInChildren<Image>();
        attachedSprites = this.GetComponentsInChildren<SpriteRenderer>();
        isRunning = false;
        thisMainSprite.enabled = false;
        foreach (Image n in attachedImages)
        {
            n.enabled = false;
        }
        foreach (SpriteRenderer n in attachedSprites)
        {
            n.enabled = false;
        }
    }
    public void ResetThis()
    {
        thisMainSprite.enabled = true;
        foreach (Image n in attachedImages)
        {
            n.enabled = true;
        }
        foreach (SpriteRenderer n in attachedSprites)
        {
            n.enabled = true;
        }
        isRunning = true;
    }
}