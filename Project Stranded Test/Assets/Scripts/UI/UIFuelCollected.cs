using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFuelCollected : MonoBehaviour {

    public GameObject fuelAmountCollectedText;
    public GameObject fuelDepositedAmountText;

    public GameObject fuelCarriedFuelText;
    public GameObject fuelShipAmountText;

    bool animationIsPlaying = false;
    public float secondAnimationTriggerDelay = 1.8f;
    float defaultAnimationTriggerDelay;
    int animationActionState = 0; // 0 for fuel pickup & 1 for depositing

	// Use this for initialization
	void Start ()
    {
        fuelAmountCollectedText.SetActive(false);
        fuelDepositedAmountText.SetActive(false);

        defaultAnimationTriggerDelay = secondAnimationTriggerDelay;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (animationIsPlaying)
        {
            if (secondAnimationTriggerDelay > 0.0f)
            {
                secondAnimationTriggerDelay -= Time.deltaTime;
            }
            else
            {
                if (animationActionState == 0)
                {
                    fuelCarriedFuelText.GetComponent<Animation>().Stop();
                    fuelCarriedFuelText.GetComponent<Animation>().Play();

                    foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (player.GetPhotonView().isMine)
                        {
                            fuelCarriedFuelText.GetComponent<Text>().text = player.GetComponent<PlayerResource>().resource.ToString();
                        }
                    }
                }

                if (animationActionState == 1)
                {
                    foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        fuelShipAmountText.GetComponent<Animation>().Stop();
                        fuelShipAmountText.GetComponent<Animation>().Play();

                        if (player.GetPhotonView().isMine)
                        {
                            int currentShipFuel = player.GetComponent<MovementScript>().myShip.GetComponent<ShipScript>().currentFuel;
                            int maxShipFuel = player.GetComponent<MovementScript>().myShip.GetComponent<ShipScript>().maxFuel;

                            fuelShipAmountText.GetComponent<Text>().text = currentShipFuel.ToString() + " / " + maxShipFuel.ToString();
                        }
                    }
                }

                animationIsPlaying = false;
            }
        }
	}

    public void DisplayFuelCollected(int fuelAmount)
    {
        fuelAmountCollectedText.GetComponent<Text>().text = "+" + fuelAmount.ToString();
        fuelAmountCollectedText.SetActive(true);
        fuelAmountCollectedText.GetComponent<Animation>().Stop();
        fuelAmountCollectedText.GetComponent<Animation>().Play();

        animationActionState = 0;
        animationIsPlaying = true;
        secondAnimationTriggerDelay = defaultAnimationTriggerDelay;
    }

    public void DisplayFuelDeposited(int fuelAmount)
    {
        fuelDepositedAmountText.GetComponent<Text>().text = "+" + fuelAmount.ToString();
        fuelDepositedAmountText.SetActive(true);
        fuelDepositedAmountText.GetComponent<Animation>().Stop();
        fuelDepositedAmountText.GetComponent<Animation>().Play();

        fuelCarriedFuelText.GetComponent<Animation>().Stop();
        fuelCarriedFuelText.GetComponent<Animation>().Play();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetPhotonView().isMine)
            {
                fuelCarriedFuelText.GetComponent<Text>().text = "0";
            }
        }

        animationActionState = 1;
        animationIsPlaying = true;
        secondAnimationTriggerDelay = defaultAnimationTriggerDelay;
    }
}
