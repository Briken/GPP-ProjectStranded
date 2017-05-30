using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PulseColourChange : PunBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void ChangeColour(int pNum)
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetComponent<MovementScript>().playerNum == pNum)
            {
                GetComponent<SpriteRenderer>().color = n.GetComponent<MovementScript>().myColour;
            }

        }
    }
}
