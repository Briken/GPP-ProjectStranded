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
    public AccoladeStatList accoladeList; // The list we use to adjust the order of accolades

    [Reorderable]
    public AccoladeStatList[] playersAccoladeList; // Accolade list per player (5 in total)

    public AccoladeStatList playerFinalAccolade; // Final accolade per player (based on player count)

    public bool[] playerHasFinalAccolade;

    // Live tracked statistics, later transferred to accolade list
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
                // Copy array information rather than array object
                playersAccoladeList[playerNum][i].statBeingUsed = accoladeList[i].statBeingUsed;
                playersAccoladeList[playerNum][i].accoladeName = accoladeList[i].accoladeName;
                playersAccoladeList[playerNum][i].accoladeDescription = accoladeList[i].accoladeDescription;
                playersAccoladeList[playerNum][i].currentValue = 0;
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
        playerHasFinalAccolade = new bool[GameObject.FindGameObjectsWithTag("Player").Length];

        // Set our local stats
        AssignStatsToAccoladeList(playersAccoladeList[thisPlayerNum]);

        // Send our accolade values to other players if it's above the significance value
        for (int i = 0; i < accoladeList.Length; i++)
        {
            if (gameObject.GetPhotonView().isMine)
            {
                gameObject.GetPhotonView().RPC("SendAccoladeData", PhotonTargets.Others, thisPlayerNum, i, playersAccoladeList[thisPlayerNum][i].currentValue);
            }
        }

        StartCoroutine(CheckHighestAccoladeValues());               
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

    IEnumerator CheckHighestAccoladeValues()
    {
        if (gameObject.GetPhotonView().isMine)
        {
            // Give data 2 seconds to arrive before determining accolade winners
            yield return new WaitForSeconds(2);

            // Begin check with default significance values
            DetermineAccoladeWinners(1.0f);
        }
    }

    [PunRPC]
    void SendAccoladeData(int playerNumber, int accoladeIndex, float accoladeValue)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetPhotonView().isMine)
            {
                player.GetComponent<PlayerStatTracker>().playersAccoladeList[playerNumber][accoladeIndex].currentValue = accoladeValue;
            }
        }       
    }

    void DetermineAccoladeWinners(float significanceValueAdjust)
    {
        Debug.Log("INITIATED ACCOLADE CHECK OF SIGNIFICANCE VALUE: " + significanceValueAdjust.ToString());

        bool everyPlayerHasFinalAccolade = true;
        float currentSignificanceValueAdjust = significanceValueAdjust;

        for (int currentAccoladeIndex = 0; currentAccoladeIndex < accoladeList.Length; currentAccoladeIndex++)
        {
            float currentHighest = 0;
            int currentHighestPlayerNum = 0;
            bool hasFoundWinnerOfAccolade = false;

            // Check the values for each player
            for (int playerNum = 0; playerNum < GameObject.FindGameObjectsWithTag("Player").Length; playerNum++)
            {
                // Debug.Log("ACCOLADE CHECK: " + playersAccoladeList[0][currentAccoladeIndex].accoladeName + " - Player " + (playerNum + 1).ToString() + " has value of " + playersAccoladeList[playerNum][currentAccoladeIndex].currentValue.ToString());

                if (playersAccoladeList[playerNum][currentAccoladeIndex].currentValue > (playersAccoladeList[playerNum][currentAccoladeIndex].significanceValue * currentSignificanceValueAdjust))
                {
                    // Check if the player's value for this accolade beats the current highest
                    if (playersAccoladeList[playerNum][currentAccoladeIndex].currentValue > currentHighest)
                    {
                        currentHighest = playersAccoladeList[playerNum][currentAccoladeIndex].currentValue;
                        currentHighestPlayerNum = playerNum;
                        hasFoundWinnerOfAccolade = true;

                        Debug.Log("HIGHEST FOR ACCOLADE: Player " + (currentHighestPlayerNum + 1).ToString() + " for " + playersAccoladeList[0][currentAccoladeIndex].accoladeName + " with value of " + currentHighest.ToString());
                    }
                }
            }

            // Set the final accolade for the player if we have a winner and they don't currently have an accolade
            if (hasFoundWinnerOfAccolade && !playerHasFinalAccolade[currentHighestPlayerNum])
            {
                playerFinalAccolade[currentHighestPlayerNum] = playersAccoladeList[currentHighestPlayerNum][currentAccoladeIndex];
                playerHasFinalAccolade[currentHighestPlayerNum] = true;

                Debug.Log("AWARDED ACCOLADE: " + playerFinalAccolade[currentHighestPlayerNum].accoladeName + " to Player " + (currentHighestPlayerNum + 1).ToString());
            }

            // Check if every player has an accolade based on our bool array
            everyPlayerHasFinalAccolade = true;

            for (int x = 0; x < playerHasFinalAccolade.Length; x++)
            {
                if (playerHasFinalAccolade[x] == false)
                {
                    everyPlayerHasFinalAccolade = false;
                    break;
                }
            }

            // If every player has an accolade, break the loop.
            if (everyPlayerHasFinalAccolade)
            {
                break;
            }

        }

        if (everyPlayerHasFinalAccolade)
        {
            /*
            Debug.Log("FINAL ACCOLADE FOR PLAYER 1: " + playerFinalAccolade[0].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 2: " + playerFinalAccolade[1].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 3: " + playerFinalAccolade[2].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 4: " + playerFinalAccolade[3].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 5: " + playerFinalAccolade[4].accoladeName);
            */
        }
        else if (currentSignificanceValueAdjust > 0) // Do another check with lower significance values
        {
            DetermineAccoladeWinners(currentSignificanceValueAdjust - 0.22f);
        }
        else // Give the player a fake accolade (shouldn't happen... but just a precaution)
        {
            int fakeAccoladesGiven = 0;

            for (int x = 0; x < playerHasFinalAccolade.Length; x++)
            {
                if (playerHasFinalAccolade[x] == false)
                {
                    switch (fakeAccoladesGiven)
                    {
                        case 0:
                            playerFinalAccolade[x].accoladeName = "Rocky";
                            playerFinalAccolade[x].accoladeDescription = "Most time near meteors";
                            fakeAccoladesGiven += 1;
                            break;

                        case 1:
                            playerFinalAccolade[x].accoladeName = "World Tour";
                            playerFinalAccolade[x].accoladeDescription = "Most planets visited";
                            fakeAccoladesGiven += 1;
                            break;

                        case 2:
                            playerFinalAccolade[x].accoladeName = "What Way?";
                            playerFinalAccolade[x].accoladeDescription = "Most changes in direction";
                            fakeAccoladesGiven += 1;
                            break;

                        case 3:
                            playerFinalAccolade[x].accoladeName = "Explorer";
                            playerFinalAccolade[x].accoladeDescription = "Most area explored";
                            fakeAccoladesGiven += 1;
                            break;

                        case 4:
                            playerFinalAccolade[x].accoladeName = "Technician";
                            playerFinalAccolade[x].accoladeDescription = "Most time near space tech";
                            break;
                    }
                }
            }

            /*
            Debug.Log("FINAL ACCOLADE FOR PLAYER 1: " + playerFinalAccolade[0].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 2: " + playerFinalAccolade[1].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 3: " + playerFinalAccolade[2].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 4: " + playerFinalAccolade[3].accoladeName);
            Debug.Log("FINAL ACCOLADE FOR PLAYER 5: " + playerFinalAccolade[4].accoladeName);
            */
        }
    }
}
