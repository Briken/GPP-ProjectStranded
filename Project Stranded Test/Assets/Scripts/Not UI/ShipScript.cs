using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ShipScript : Photon.PunBehaviour
{
    public int shipNum;
    public GameObject player;
    public MovementScript playerMoves;

    public int currentFuel = 0;
    public int maxFuel = 100;
    public int totalFuel = 0;

    public bool claimed = false;


	// Use this for initialization
	void Start ()
    {
        EventManager.Reset += ResetThis;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (player != null && playerMoves == null)
        {
            playerMoves = player.GetComponent<MovementScript>();
            shipNum = playerMoves.playerNum;
        }	

	}

    [PunRPC]
    public void DepositFuel(int pResource)
    {
        currentFuel += pResource;
        totalFuel += currentFuel;
    }

    public void ResetThis()
    {
        currentFuel = 0;
    }
}
