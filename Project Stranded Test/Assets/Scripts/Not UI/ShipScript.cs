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

    //public void OnCollisionEnter2D(Collision2D playerCol)
    //{
    //    if (playerCol.gameObject.GetComponent<MovementScript>().playerNum == shipNum)
    //    {
    //        currentFuel += player.gameObject.GetComponent<PlayerResource>().resource;
    //        totalFuel += currentFuel;
    //    }
    //}

    [PunRPC]
    public void DepositFuel()
    {
        currentFuel += player.gameObject.GetComponent<PlayerResource>().resource;
        totalFuel += currentFuel;
    }
}
