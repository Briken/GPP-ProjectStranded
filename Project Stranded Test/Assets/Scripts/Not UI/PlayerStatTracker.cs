using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Malee;

public class PlayerStatTracker : Photon.PunBehaviour
{

    [System.Serializable]
    public class AccoladeStat
    {
        public string statBeingUsed;
        public string accoladeName;
        public string accoladeDescription;
        public float currentValue;
        public float significanceValue;
    }

    [System.Serializable]
    public class AccoladeStatList : ReorderableArray<AccoladeStat>
    {
    }

    [Reorderable]
    public AccoladeStatList accoladeList;

    [Reorderable]
    public AccoladeStatList[] playersAccoladeList;

    // Statistics to track for accolades or other uses
    [Header("Time-based Fuel Stats")]
    public float timeSinceLastNearFuelCrate;
    public float timeSinceLastFuelCratePickup;
    public float timeSinceLastFuelDeposit;
    public float timeSpentNearFuelCrates;
    public float timeSpentOpeningFuelCrates;

    [Header("Fuel Stats")]
    public int overallCollectedFuel;
    public int overallFuelDeposited;
    public int timesDepositingFuel;
    public int maxFuelCarriedAtOnce;
    public int maxFuelDepositedAtOnce;
    public float earliestDepositTimeRemaining = 0;
    public float latestDepositTimeRemaining = 9999;

    [Header("Fuel Crate Stats")]
    public int fuelCratesOpened;
    public int[] fuelCratesOpenedBySize;

    [Header("Movement")]
    public float timeSpentMoving;
    public float timeSpentNotMoving;
    public int timesManuallyStoppedMoving;

    [Header("Voting")]
    public int timesInVote;
    public int votesPlaced;
    public int votesNotPlaced;

    [Header("Comms")]
    public int timesActivatingComms;
    public int timesPressingCommsButton;

    [Header("Player Proximity")]
    public float[] timeSpentNearPlayer;
    public float timeSpentNearPlayers;

    [Header("Environment")]
    public float timeSpentNearPlanets;

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

        // Set up each player's accolade list based on our reorderable list
        for (int playerNum = 0; playerNum < 5; playerNum++)
        {
            for (int i = 0; i < accoladeList.Length; i++)
            {
                playersAccoladeList[playerNum][i].statBeingUsed = accoladeList[i].statBeingUsed;
                playersAccoladeList[playerNum][i].accoladeName = accoladeList[i].accoladeName;
                playersAccoladeList[playerNum][i].accoladeDescription = accoladeList[i].accoladeDescription;
                playersAccoladeList[playerNum][i].currentValue = accoladeList[i].currentValue;
                playersAccoladeList[playerNum][i].significanceValue = accoladeList[i].significanceValue;
            }
        }   
    }
	
	// Update is called once per frame
	void Update ()
    {
        gameTimeElapsed += Time.deltaTime;
        votesNotPlaced = timesInVote - votesPlaced;

        TrackProximityToPlayers();
    }

    public void InitiateAccoladeCheck()
    {
        int thisPlayerNum = gameObject.GetComponent<MovementScript>().playerNum - 1;
        // Set our local stats

        AssignStatsToAccoladeList(playersAccoladeList[thisPlayerNum]);

        // Send our accolade values to other players if it's above the significance value
        for (int i = 0; i < accoladeList.Length; i++)
        {
            //if (accoladeList[i].currentValue > accoladeList[i].significanceValue)
            if (gameObject.GetPhotonView().isMine)
            {
                gameObject.GetPhotonView().RPC("SendAccoladeData", PhotonTargets.All, thisPlayerNum, i, playersAccoladeList[thisPlayerNum][i].currentValue);
            }
        }        
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
                    timeSpentNearPlayers += Time.deltaTime;
                }
            }
        }
    }

    void AssignStatsToAccoladeList(AccoladeStatList playerAccoladeList)
    {
        foreach (AccoladeStat thisAccoladeStat in playerAccoladeList)
        {
            if (thisAccoladeStat.statBeingUsed == "overallCollectedFuel")
            {
                thisAccoladeStat.currentValue = overallCollectedFuel;
            }
            else if (thisAccoladeStat.statBeingUsed == "overallFuelDeposited")
            {
                thisAccoladeStat.currentValue = overallFuelDeposited;
            }
            else if (thisAccoladeStat.statBeingUsed == "timesDepositingFuel")
            {
                thisAccoladeStat.currentValue = timesDepositingFuel;
            }
            else if (thisAccoladeStat.statBeingUsed == "maxFuelCarriedAtOnce")
            {
                thisAccoladeStat.currentValue = maxFuelCarriedAtOnce;
            }
            else if (thisAccoladeStat.statBeingUsed == "maxFuelDepositedAtOnce")
            {
                thisAccoladeStat.currentValue = maxFuelDepositedAtOnce;
            }
            else if (thisAccoladeStat.statBeingUsed == "maxFuelDepositedAtOnce >= maxFuel")
            {
                thisAccoladeStat.currentValue = 0;
            }
            else if (thisAccoladeStat.statBeingUsed == "earliestDepositTimeRemaining")
            {
                thisAccoladeStat.currentValue = earliestDepositTimeRemaining;
            }
            else if (thisAccoladeStat.statBeingUsed == "latestDepositTimeRemaining")
            {
                thisAccoladeStat.currentValue = latestDepositTimeRemaining;
            }
            else if (thisAccoladeStat.statBeingUsed == "fuelCratesOpened")
            {
                thisAccoladeStat.currentValue = fuelCratesOpened;
            }
            else if (thisAccoladeStat.statBeingUsed == "fuelCratesOpenedBySize[1]")
            {
                thisAccoladeStat.currentValue = fuelCratesOpenedBySize[1];
            }
            else if (thisAccoladeStat.statBeingUsed == "fuelCratesOpenedBySize[2]")
            {
                thisAccoladeStat.currentValue = fuelCratesOpenedBySize[2];
            }
            else if (thisAccoladeStat.statBeingUsed == "fuelCratesOpenedBySize[3]")
            {
                thisAccoladeStat.currentValue = fuelCratesOpenedBySize[3];
            }
            else if (thisAccoladeStat.statBeingUsed == "fuelCratesOpenedBySize[4]")
            {
                thisAccoladeStat.currentValue = fuelCratesOpenedBySize[4];
            }
            else if (thisAccoladeStat.statBeingUsed == "timeSpentMoving")
            {
                thisAccoladeStat.currentValue = timeSpentMoving;
            }
            else if (thisAccoladeStat.statBeingUsed == "timeSpentNotMoving")
            {
                thisAccoladeStat.currentValue = timeSpentNotMoving;
            }
            else if (thisAccoladeStat.statBeingUsed == "timesManuallyStoppedMoving")
            {
                thisAccoladeStat.currentValue = timesManuallyStoppedMoving;
            }
            else if (thisAccoladeStat.statBeingUsed == "timesInVote")
            {
                thisAccoladeStat.currentValue = timesInVote;
            }
            else if (thisAccoladeStat.statBeingUsed == "votesNotPlaced")
            {
                thisAccoladeStat.currentValue = votesNotPlaced;
            }
            else if (thisAccoladeStat.statBeingUsed == "timesActivatingComms")
            {
                thisAccoladeStat.currentValue = timesActivatingComms;
            }
            else if (thisAccoladeStat.statBeingUsed == "timeSpentNearPlayers")
            {
                thisAccoladeStat.currentValue = timeSpentNearPlayers;
            }
            else if (thisAccoladeStat.statBeingUsed == "timeSpentNearFuelCrates")
            {
                thisAccoladeStat.currentValue = timeSpentNearFuelCrates;
            }
            else if (thisAccoladeStat.statBeingUsed == "timeSpentOpeningFuelCrates")
            {
                thisAccoladeStat.currentValue = timeSpentOpeningFuelCrates;
            }
            else if (thisAccoladeStat.statBeingUsed == "timeSpentNearPlanets")
            {
                thisAccoladeStat.currentValue = timeSpentNearPlanets;
            }
        }
    }

    [PunRPC]
    public void SendAccoladeData(int playerNumber, int accoladeIndex, float accoladeValue)
    {
        playersAccoladeList[playerNumber][accoladeIndex].currentValue = accoladeValue;

        Debug.Log("Received value of " + accoladeValue.ToString() + " for Accolade " + accoladeIndex.ToString() + " for Player " + playerNumber.ToString());
    }
}
