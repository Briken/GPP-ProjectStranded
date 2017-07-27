using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;
using System;

public class UIBarScript : PunBehaviour {

    public GameObject fuelCarriedText;
    public GameObject fuelCarriedBar;
    public GameObject fuelShipText;
    public GameObject fuelShipBar;
    public float barMaxValue = 20.0f;
    public float barValue = 0.0f;
    float currentPlayerFuel;
    float currentShipFuel;
    float maxShipFuel;

    // Temp UI
    public GameObject tempCarriedFuelText;
    public GameObject tempShipFuelText;

    // Use this for initialization
    void Start ()
    {
        fuelCarriedText = GameObject.Find("Text - Carried Fuel");
        fuelCarriedBar = GameObject.Find("Slider - Fuel");
        fuelShipText = GameObject.Find("Text - Ship Fuel");
        fuelShipBar = GameObject.Find("Slider - Ship Fuel");
        tempCarriedFuelText = GameObject.Find("Text - Fuel Amount");
        tempShipFuelText = GameObject.Find("Text - Ship Fuel Amount");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (photonView.isMine)
        {
            
            /*
            // Fuel bar handling:
            if (barValue > this.gameObject.GetComponent<PlayerResource>().resource / barMaxValue)
            {
                barValue = Mathf.Clamp(barValue - (0.4f * Time.deltaTime), 0.0f, 1.0f);
            }
            else if (barValue <= this.gameObject.GetComponent<PlayerResource>().resource / barMaxValue)
            {
                barValue = Mathf.Clamp(barValue + (0.15f * Time.deltaTime), 0.0f, 1.0f);
            }
            */

            // Convert player fuel int value to float so we can divide properly
            currentPlayerFuel = gameObject.GetComponent<PlayerResource>().resource;

            fuelCarriedBar.GetComponent<Slider>().value = currentPlayerFuel / barMaxValue;

            fuelCarriedText.GetComponent<Text>().text = Mathf.Round((currentPlayerFuel / barMaxValue)*100).ToString() + "%";


            // Convert ship fuel int values to float so we can divide properly
            currentShipFuel = gameObject.GetComponent<MovementScript>().myShip.GetComponent<ShipScript>().currentFuel;
            maxShipFuel = gameObject.GetComponent<MovementScript>().myShip.GetComponent<ShipScript>().maxFuel;

            fuelShipBar.GetComponent<Slider>().value = currentShipFuel / maxShipFuel;

            fuelShipText.GetComponent<Text>().text = ((currentShipFuel / maxShipFuel) * 100).ToString() + "%";

            // Display raw fuel values
            // tempCarriedFuelText.GetComponent<Text>().text = currentPlayerFuel.ToString();
            // tempShipFuelText.GetComponent<Text>().text = currentShipFuel.ToString() + " / " + maxShipFuel.ToString();         
        }
    }
}
