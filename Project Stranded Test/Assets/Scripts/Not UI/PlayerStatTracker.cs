using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatTracker : MonoBehaviour {

    // Statistics to track for accolades or other uses
    [Header("Time-based Fuel Stats")]
    public float timeSinceLastNearFuelCrate;
    public float timeSinceLastFuelCratePickup;
    public float timeSinceLastFuelDeposit;

    [Header("Fuel Stats")]
    public int overallCollectedFuel;
    public int overallFuelDeposited;
    public int timesDepositingFuel;
    public int maxFuelCarriedAtOnce;
    public int maxFuelDepositedAtOnce;

    [Header("Fuel Crate Stats")]
    public int fuelCratesOpened;
    public int[] fuelCratesOpenedBySize;

    [Header("Movement")]
    public float timeSpentMoving;
    public float timeSpentNotMoving;

    [Header("Voting")]
    public int timesInVote;
    public int votesPlaced;
    public int votesNotPlaced;

    [Header("Comms")]
    public int timesActivatingComms;
    public int timesPressingCommsButton;

    [Header("Player Proximity")]
    public float[] timeSpentNearPlayer;

    [Space(30)]
    [Header("Config & Debug")]
    public GameObject[] inGamePlayers;
    public float playerCloseProximity = 13.0f;

    float gameTimeElapsed = 0.0f;

    // Use this for initialization
    void Start ()
    {
        fuelCratesOpenedBySize = new int[6];
        timeSpentNearPlayer = new float[5];
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameTimeElapsed += Time.deltaTime;
        votesNotPlaced = timesInVote - votesPlaced;

        TrackProximityToPlayers();
    }

    void TrackProximityToPlayers()
    {
        inGamePlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in inGamePlayers)
        {
            if (player.GetComponent<MovementScript>().playerNum != gameObject.GetComponent<MovementScript>().playerNum)
            {
                if (Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position) < playerCloseProximity)
                {
                    timeSpentNearPlayer[player.GetComponent<MovementScript>().playerNum - 1] += Time.deltaTime;
                }
            }
        }
    }
}
